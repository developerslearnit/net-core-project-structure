namespace ShephardTech.Financials.API.Extensions
{
    public static class OwaspSecurityMiddlewareExtension
    {
        public static void UseOwaspSecurityConfig(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                //Protects against Clickjacking attacks. ref.: http://stackoverflow.com/a/22105445/1233379
                context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");

                //Protects against XSS injections. ref.: https://www.veracode.com/blog/2014/03/guidelines-for-setting-security-headers/
                context.Response.Headers.Add("X-Xss-Protection", "1");

                //Protects against MIME - type confusion attack. ref.: https://www.veracode.com/blog/2014/03/guidelines-for-setting-security-headers/
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");

                //Protects against Clickjacking attacks. ref.: https://www.owasp.org/index.php/HTTP_Strict_Transport_Security_Cheat_Sheet
                context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000; includeSubDomains");

                //CSP modern XSS directive-based defence, used since 2014. ref.: http://content-security-policy.com/
                context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; connect-src *;font-src 'self' https://fonts.gstatic.com data:; frame-src *; img-src * data:; media-src *; object-src *; script-src * 'unsafe-inline' 'unsafe-eval'; style-src * 'unsafe-inline';");

                //Prevents from leaking referrer data over insecure connections. ref.: https://scotthelme.co.uk/a-new-security-header-referrer-policy/
                context.Response.Headers.Add("Referrer-Policy", "strict-origin");

                //Feature - Policy is a new header that allows a site to control which features and APIs can be used in the browser. ref.: https://wicg.github.io/feature-policy/
                context.Response.Headers.Add("Feature-Policy", "accelerometer 'none'; camera 'none'; geolocation 'none'; gyroscope 'none'; magnetometer 'none'; microphone 'none'; payment *; usb 'none'");



                await next();
            });
        }
    }
}
