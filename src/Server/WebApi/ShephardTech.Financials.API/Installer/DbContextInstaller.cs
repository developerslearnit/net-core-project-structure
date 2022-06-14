using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ShephardTech.Financials.Persistence.StorageContexts.Financials;
using System.Data;

namespace ShephardTech.Financials.API.Installer
{
    public class DbContextInstaller:IInstaller
    {
        public void InstallServices(WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<ShepardFinContext>(options =>
                        options.UseSqlServer(builder.Configuration.GetSection("DBConnections:ShepardFinTechConnection").Value,
                          sqlServerOptionsAction: sqlOptions =>
                          {
                              sqlOptions.EnableRetryOnFailure(
                                  maxRetryCount: 10,
                                  maxRetryDelay: TimeSpan.FromSeconds(30),
                                  errorNumbersToAdd: null
                                  );
                          }));

           

            builder.Services.AddScoped<IDbConnection, SqlConnection>(db =>
                new SqlConnection(builder.Configuration.GetSection("DBConnections:ShepardFinTechConnection").Value));
        }
    }
}
