using Clients;
using IdentityClient;
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

            var discoverableConnection = await ServerConnection.DiscoverEndpointsAsync(Constants.Authority);


            // Scopes:
            // ======
            //
            // Scope list (space separated) must oly contain values registered for that client.
            // Ex. scope 'role' is not registered for client 'Test' and will receive error.
            // 
            // The call /connect/userinfo requires scope 'openid' and will return only subject :
            // {"sub":"ad111111-3986-4980-6301-283888811531"}
            //  'sub' stands for subject and returns [IdentityDb].[dbo].[AspNetUsers].[Id] value. Not much.
            //
            // Since our client 'Test' allows 'profile' scope, the result is more verbose:
            // {"sub":"ad111111-3986-4980-6301-283888811531","preferred_username":"admin@nonsense.com","name":"admin@nonsense.com"}
            // Allowed scopes for userinfo are:
            // sub name family_name given_name middle_name nickname preferred_username profile picture website gender birthdate zoneinfo locale updated_at
            //
            // Scope 'admin' will be required later... TODO

            var passwordBasedConnection = await discoverableConnection.RequestPasswordTokenAsync(
                    new ServerConnection.ClientInfo("Test", "" /*"CACEBCF1-DA16-44A9-9ADD-9D453AA716CF"*/, "openid profile"),
                    new ServerConnection.ClientCredentials("admin@nonsense.com", "Qwerty1234!")
                    );

            var userInfo = await passwordBasedConnection.GetUserInfo();
            Console.WriteLine(userInfo);


            var response = await passwordBasedConnection.CallGetMethodAsync(
                    "/api/Example/Test",
                    content: null,
                    new ServerConnection.Realms(["api1"], [], []));
            Console.WriteLine(response);

            var adminResponse1 = await passwordBasedConnection.CallGetMethodAsync("/api/Example/AdminOnly", null, new ServerConnection.Realms([], [], []));

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.GetErrorMessage());
        }
        Console.WriteLine("===");
        Console.WriteLine("Press ENTER to exit");
        Console.ReadLine();
    }

/*
    static async Task<TokenResponse> RequestTokenAsync()
    {
        var disco = await httpClient.GetDiscoveryDocumentAsync(Constants.Authority);
        if (disco.IsError) throw new Exception(disco.Error);

        
        //var response = await httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        //{
        //    Address = disco.TokenEndpoint,

        //    ClientId = "Test",
        //    ClientSecret = "CACEBCF1-DA16-44A9-9ADD-9D453AA716CF",
        //    Scope = "admin",
        //});
        

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
*/
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