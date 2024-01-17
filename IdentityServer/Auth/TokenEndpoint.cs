using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace IdentityServer.Auth;

public static class TokenEndpoint
{

    public static async Task<IResult> Connect(HttpContext context, JwtConfigurator jwtConfigurator)
    {
        throw new NotImplementedException("CONNECT");
    }

}
