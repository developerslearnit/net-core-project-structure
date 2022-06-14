namespace ShephardTech.Financials.API.Installer
{
    public class CorsInstaller : IInstaller
    {
        public void InstallServices(WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("ShepardCorsPolicy", builder =>

                  builder.AllowAnyOrigin()
                         .AllowAnyMethod()
                         .AllowAnyHeader());
            });
        }
    }
}
