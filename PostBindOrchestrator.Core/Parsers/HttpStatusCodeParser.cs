using System.Net;

namespace PostBindOrchestrator.Core;

public static class HttpStatusCodeParser
{
    public static HttpStatusCode ParseElseException(string httpStatusCodeAsString)
    {
        if  (Enum.TryParse<HttpStatusCode>(httpStatusCodeAsString, out var httpStatusCode))
        {
            return httpStatusCode;
        }

        throw new HttpStatusCodeParseException($"Failed to parse http status code type from string with value: {httpStatusCodeAsString}");        
    }
}