using Web_153504_Padvalnikau.Middleware;

namespace Web_153504_Padvalnikau.Extensions;

public static class LogExtensions
{
    public static IApplicationBuilder UseLogging(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<Middleware.LogMiddleware>();
    }
}