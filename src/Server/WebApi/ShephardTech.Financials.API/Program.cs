using Microsoft.AspNetCore.HttpOverrides;
using ShephardTech.Financials.API.Extensions;
using ShephardTech.Financials.API.Installer;
using ShephardTech.Financials.Application;

var builder = WebApplication.CreateBuilder(args);

builder.InstallServicesInAssembly();

builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("JWTSettings"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("ShepardCorsPolicy");
app.UseForwardedHeaders(
    new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.All
    });


app.UseOwaspSecurityConfig();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();