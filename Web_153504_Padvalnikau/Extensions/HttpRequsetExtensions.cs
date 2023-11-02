namespace Web_153504_Padvalnikau.Extensions;

public static class HttpRequestExstension
{
    public static bool IsAjaxRequest(this HttpRequest request)
    {
        return request.Headers["x-requested-with"].ToString().ToLower().Equals("xmlhttprequest");
    }
}