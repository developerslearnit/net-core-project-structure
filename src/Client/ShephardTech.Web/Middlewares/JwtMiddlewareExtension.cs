namespace ShephardTech.Web.Middlewares
{
	public static class JwtMiddlewareExtension
	{
        public static IApplicationBuilder UseJwtMiddleware(
        this IApplicationBuilder builder)
        {
            // initialise the middleware
            return builder.UseMiddleware<JwtMiddleware>();
        }
    }
}
