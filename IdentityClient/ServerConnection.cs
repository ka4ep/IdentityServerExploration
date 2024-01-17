using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static IdentityClient.ServerConnection;

namespace IdentityClient;

public class ServerConnection
{
    private static readonly HttpClient httpClient = new();
    private static string baseAddress = string.Empty;


    public static async Task<DiscoverableServerConnection> DiscoverEndpointsAsync(string authorityAddress)
    {
        var discoveryResponse = await httpClient.GetDiscoveryDocumentAsync(authorityAddress);
        if (discoveryResponse.IsError) throw new Exception(discoveryResponse.Error);
        baseAddress = authorityAddress;
        return new DiscoverableServerConnection(discoveryResponse);
    }

    public readonly struct ClientInfo(string clientId, string clientSecret, string scope)
    {
        internal string ClientId { get; } = clientId;
        internal string ClientSecret { get; } = clientSecret;
        internal string Scope { get; } = scope;
    }

    public readonly struct ClientCredentials(string userName, string password)
    {
        internal string UserName { get; } = userName;
        internal string Password { get; } = password;
    }

    public readonly struct Realms(string[] scopes, string[] profiles, string[] roles)
    {
        internal string[] Scopes { get; } = scopes;
        internal string[] Profiles { get; } = profiles;
        internal string[] Roles { get; } = roles;
    }

    //public abstract class ResponseType { protected ResponseType(string? value) { Value = value; } public string? Value { get; } }
    //public sealed class StringResponse(string? value) : ResponseType(value) { }
    //public sealed class JsonResponse(string? value) : ResponseType(value) { }

    public class DiscoverableServerConnection(DiscoveryDocumentResponse documentResponse)
    {
        private readonly DiscoveryDocumentResponse discovery = documentResponse;

        public async Task<TokenizedServerConnection> RequestClientCredentialsTokenAsync(ClientInfo clientInfo)
        {
            var response = await httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = discovery.TokenEndpoint,
                ClientId = clientInfo.ClientId,
                ClientSecret = clientInfo.ClientSecret,
                Scope = clientInfo.Scope,
            });
            if (response.IsError) ThrowException(response);
            return new TokenizedServerConnection(discovery, response);
        }

        public async Task<TokenizedServerConnection> RequestPasswordTokenAsync(ClientInfo clientInfo, ClientCredentials clientCredentials)
        {
            var response = await httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = discovery.TokenEndpoint,
                ClientId = clientInfo.ClientId,
                ClientSecret = clientInfo.ClientSecret,
                Scope = clientInfo.Scope,
                GrantType = "password",
                UserName = clientCredentials.UserName,
                Password = clientCredentials.Password,
            });
            if (response.IsError) ThrowException(response);
            return new TokenizedServerConnection(discovery, response);
        }

        private static void ThrowException(TokenResponse response)
        {
            var message = $"{(int)response.HttpStatusCode}] {response.HttpStatusCode:G}, {response.ErrorType:G} : " +
                response.Raw ?? $"{response.Error}, {response.ErrorDescription}";
            throw new Exception(message);
        }



        public class TokenizedServerConnection(DiscoveryDocumentResponse discovery, TokenResponse tokenResponse)
        {
            private async Task<string> CallMethodAsync(HttpMethod method, Uri url, HttpContent? content, Realms realms)
            {
                using var request = new HttpRequestMessage
                {
                    Content = content,
                    Method = method,
                    RequestUri = url,
                };
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);
                if (realms.Scopes?.Length > 0) request.Headers.Add("scope", string.Join(" ", realms.Scopes));

                using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                if (response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();
                throw new Exception($"[{(int)response.StatusCode}] {response.StatusCode:G}{Environment.NewLine}{response.Headers.Select(h => $"[{h.Key}] : {string.Join(" ; ", h.Value)}")}");
            }


            public async Task<string> CallGetMethodAsync(string relativeUrl, HttpContent? content, Realms realms)
            {
                return await CallMethodAsync(HttpMethod.Get, new Uri(new Uri(baseAddress, UriKind.Absolute), new Uri(relativeUrl, UriKind.Relative)), content, realms);
            }

            public async Task<string> CallPostMethodAsync(string relativeUrl, HttpContent? content, Realms realms)
            {
                return await CallMethodAsync(HttpMethod.Post, new Uri(new Uri(baseAddress, UriKind.Absolute), new Uri(relativeUrl, UriKind.Relative)), content, realms);
            }

            public async Task<string> GetUserInfo()
            {
                if (string.IsNullOrWhiteSpace(discovery.UserInfoEndpoint)) return string.Empty;
                return await CallMethodAsync(HttpMethod.Get, new Uri(discovery.UserInfoEndpoint, UriKind.Absolute), null, new Realms(["openid"], [], []));
            }


        }

    }




}
