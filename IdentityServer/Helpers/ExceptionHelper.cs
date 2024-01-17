#nullable enable
using System.Text;
using System;
using System.Collections;

namespace IdentityServer.Helpers;

internal static class ExceptionHelper
{
    internal static string GetErrorMessage(this Exception? exception)
    {
        var sb = new StringBuilder(256);
        while (exception is not null)
        {
            try
            {
                sb.AppendLine(exception.Message);
                foreach (DictionaryEntry item in exception.Data)
                    sb.AppendLine($" * [{item.Key}] : {item.Value}");
                
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
#nullable restore