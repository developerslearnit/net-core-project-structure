namespace ShephardTech.Financials.API.Installer
{
    public class ControllerInstaller : IInstaller
    {
        public void InstallServices(WebApplicationBuilder builder)
        {
            
            builder.Services.AddControllers
                (config =>
                {
                    config.RespectBrowserAcceptHeader = true;
                    config.ReturnHttpNotAcceptable = true;
                }).AddXmlDataContractSerializerFormatters();
        }
    }
}
