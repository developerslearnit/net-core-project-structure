using ShephardTech.Financials.Domain;

namespace ShephardTech.Financials.API.Installer
{
    public class RepositoryInstaller : IInstaller
    {
        public void InstallServices(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
        
        }
    }
}
