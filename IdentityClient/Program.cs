using Clients;
using IdentityModel;
using IdentityModel.Client;
using System.Net.Http.Headers;
using System.Text;

namespace ConsoleClientCredentialsFlowCallingIdentityServerApi;

public static class Program
{
    public static readonly HttpClient httpClient = new();
    public static async Task Main()
    {
        try
        {
            Console.Title = "Console Client Credentials Flow calling IdentityServer API";

            var response = await RequestTokenAsync();
            response.Show();

            //Console.ReadLine();
            await CallServiceAsync(response.AccessToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.GetErrorMessage());
        }
        Console.WriteLine("===");
        Console.WriteLine("Press ENTER to exit");
        Console.ReadLine();
    }

    static async Task<TokenResponse> RequestTokenAsync()
    {
        var disco = await httpClient.GetDiscoveryDocumentAsync(Constants.Authority);
        if (disco.IsError) throw new Exception(disco.Error);

        /*
        var response = await httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        {
            Address = disco.TokenEndpoint,

            ClientId = "Test",
            ClientSecret = "CACEBCF1-DA16-44A9-9ADD-9D453AA716CF",
            Scope = "admin",
        });
        */

        var response = await httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest
        {
            Address = disco.TokenEndpoint,

            ClientId = "Test",
            ClientSecret = "CACEBCF1-DA16-44A9-9ADD-9D453AA716CF",
            Scope = "admin",

            GrantType = "password",
            UserName = "admin@nonsense.com",
            Password = "Qwerty1234!",
        });

        if (response.IsError)
        {
            var message = $"{(int)response.HttpStatusCode}] {response.HttpStatusCode:G}, {response.ErrorType:G} : " +
                            response.Raw ?? $"{response.Error}, {response.ErrorDescription}";
            throw new Exception(message);
        }
        return response;
    }

    static async Task CallServiceAsync(string token)
    {
        using var message = new HttpRequestMessage
        {
            Content = new StringContent("This is test"),
            Method = HttpMethod.Get,
            RequestUri = new Uri(new Uri(Constants.Authority, UriKind.Absolute), new Uri("/api/Redirect/Test", UriKind.Relative)),
        };
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        message.Headers.Add("scope", "api1");

        using var response = await httpClient.SendAsync(message, HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        "\n\nService claims:".ConsoleGreen();
        
        Console.WriteLine(content);
    }

    private static string GetErrorMessage(this Exception? exception)
    {
        var sb = new StringBuilder(256);
        while (exception is not null)
        {
            try
            {
                sb.AppendLine(exception.Message);
                exception = exception.InnerException;
            }
            catch
            {
                // Message might be throwing error
                break;
            }
        }
        return sb.ToString();
    }

}