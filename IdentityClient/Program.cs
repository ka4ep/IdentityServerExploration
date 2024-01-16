using Clients;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ConsoleClientCredentialsFlowCallingIdentityServerApi
{
    public class Program
    {
        public static readonly HttpClient httpClient = new HttpClient();
        public static async Task Main()
        {
            Console.Title = "Console Client Credentials Flow calling IdentityServer API";

            var response = await RequestTokenAsync();
            response.Show();

            Console.ReadLine();
            await CallServiceAsync(response.AccessToken);
        }

        static async Task<TokenResponse> RequestTokenAsync()
        {
            var disco = await httpClient.GetDiscoveryDocumentAsync(Constants.Authority);
            if (disco.IsError) throw new Exception(disco.Error);

            var response = await httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = "Test",
                ClientSecret = "CACEBCF1-DA16-44A9-9ADD-9D453AA716CF",
                Scope = "admin"
            });

            if (response.IsError) throw new Exception(response.Error);
            return response;
        }

        static async Task CallServiceAsync(string token)
        {
            //httpClient.SetBearerToken(token);
            //var response = await httpClient.GetStringAsync(Constants.Authority + "/api/Redirect/Test");

            using var message = new HttpRequestMessage
            {
                Content = new StringContent("This is test"),
                Method = HttpMethod.Get,
                RequestUri = new Uri(new Uri(Constants.Authority, UriKind.Absolute), new Uri("/api/Redirect/Test", UriKind.Relative)),
            };
            message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using var response = await httpClient.SendAsync(message, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            "\n\nService claims:".ConsoleGreen();
            
            Console.WriteLine(content);
        }
    }
}