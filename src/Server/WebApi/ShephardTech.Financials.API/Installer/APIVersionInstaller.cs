using Microsoft.AspNetCore.Mvc;

namespace ShephardTech.Financials.API.Installer
{
    public class APIVersionInstaller : IInstaller
    {
        public void InstallServices(WebApplicationBuilder builder)
        {
            builder.Services.AddApiVersioning(opt =>
            {
                opt.ReportApiVersions = true;
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
            });
        }
    }
}
