using Microsoft.AspNetCore.Http;

namespace Venice.Orders.Api.Middleware;

public class SecurityHeadersMiddleware
{
    private const string XFrameOptions = "DENY";
    private const string XContentTypeOptions = "nosniff";
    private const string XXssProtection = "1; mode=block";
    private const string ReferrerPolicy = "strict-origin-when-cross-origin";
    private const string StrictTransportSecurity = "max-age=31536000; includeSubDomains";
    private const string ContentSecurityPolicy = "default-src 'self'; script-src 'self' 'unsafe-inline' 'unsafe-eval'; style-src 'self' 'unsafe-inline'; img-src 'self' data: https:; font-src 'self' data:;";

    private readonly RequestDelegate _next;

    public SecurityHeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.Headers.Append("X-Frame-Options", XFrameOptions);
        context.Response.Headers.Append("X-Content-Type-Options", XContentTypeOptions);
        context.Response.Headers.Append("X-XSS-Protection", XXssProtection);
        context.Response.Headers.Append("Referrer-Policy", ReferrerPolicy);
        
        if (context.Request.IsHttps)
        {
            context.Response.Headers.Append("Strict-Transport-Security", StrictTransportSecurity);
        }
        
        context.Response.Headers.Append("Content-Security-Policy", ContentSecurityPolicy);

        await _next(context);
    }
}

