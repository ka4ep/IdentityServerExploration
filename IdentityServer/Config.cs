using IdentityServer.Services;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer;

public static class Config
{


    public static readonly IEnumerable<ApiScope> Scopes = [
        new ApiScope(name: "admin", "roles") { UserClaims = ["ap1"], Properties = new Dictionary<string, string> { ["Prop1"] = "Value1" } },
        new ApiScope(name: "invoice.read", displayName:"Can read invoices"),
        new ApiScope(name: "invoice.pay", displayName:"Can pay invoices"),
    ];

    public static readonly IEnumerable<ApiResource> Resources = [
        new ApiResource("api1", "API 1", new[] { "name", "role" }) { ApiSecrets = [new Secret("{C43005AB-BB6B-4FC7-BE98-B76D836509CB}".Sha256())]}
    ];


    public static readonly IEnumerable<IdentityResource> IdentityResources = [
        new IdentityResources.OpenId(),
        new IdentityResources.Email(),
        new IdentityResources.Profile(),
        new IdentityResource("roles", ["role"])
    ];

    public static IEnumerable<Client> GetClients(HostAddressService hostAddressService)
    {
        var address = hostAddressService.HostAddress ?? "http://localhost:5000";
        return [
        new Client
        {
            ClientId = "Test",
            ClientName = "Test Client",
            ClientSecrets = [new Secret("CACEBCF1-DA16-44A9-9ADD-9D453AA716CF".Sha256(), "Random client secret")],
            AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
            AllowAccessTokensViaBrowser = true,
            RequireClientSecret = true,
            AlwaysSendClientClaims = true,
            // от этой настройки зависит размер токена, 
            // при false можно получить недостающую информацию через UserInfo endpoint
            AlwaysIncludeUserClaimsInIdToken = true,
            
            // белый список адресов на который клиентское приложение может попросить
            // перенаправить User Agent, важно для безопасности
            RedirectUris = {
                // адрес перенаправления после логина
                $"{address}/callback.html",
                // адрес перенаправления при автоматическом обновлении access_token через iframe
                $"{address}/callback-silent.html"
            },
            PostLogoutRedirectUris = { $"{address}/index.html" },
            // адрес клиентского приложения, просим сервер возвращать нужные CORS-заголовки
            AllowedCorsOrigins = { $"{address}" },
            // список scopes, разрешённых именно для данного клиентского приложения
            AllowedScopes =
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                "api1",
                "admin",
                "invoice.read",
                "invoice.pay"
            },

            AccessTokenLifetime = 3600, // секунд, это значение по умолчанию
            IdentityTokenLifetime = 300, // секунд, это значение по умолчанию

            // разрешено ли получение refresh-токенов через указание scope offline_access
            AllowOfflineAccess = false,
            
        }
        ];
    }

}