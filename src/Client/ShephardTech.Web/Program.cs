using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ShephardTech.Web.helpers;
using ShephardTech.Web.Middlewares;
using System.Net.Http.Headers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "ShepardTechApp",
        ValidAudience = "http://localhost:5000",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SHEPARD_TECH_JWT_ciuntmkucuqzzomvghfakprhgdvoajjmlphxpsAQADyMMaVBKqy7A2LyXlirpSPDI4u1HiPLXFAyouaw6Km0VQTTpcdljyjzdiqagvkwcjgxumfwhrjmlphxpsAQADyMMaVBKqy7A2LyXlir"))
    };
}).AddCookie(opt =>
{
    opt.ExpireTimeSpan = TimeSpan.FromDays(1);
    opt.Cookie.HttpOnly = true;
    opt.SlidingExpiration = true;
    opt.AccessDeniedPath = "auth/login";
    opt.LoginPath = "auth/login";
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IJwtUtils, JwtUtils>();
builder.Services.AddSingleton<CookieHelper>();


builder.Services.AddHttpClient("shepardClient", c =>
{
    c.BaseAddress = new System.Uri(builder.Configuration.GetSection("ApiBaseUri").Value);
    c.DefaultRequestHeaders.Add("Accept", "application/json");
    c.DefaultRequestHeaders.Add(StaticKeys.APIKEY_KEY_NAME, StaticKeys.APIKEY_KEY_VALUE);
    c.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
});





var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseJwtMiddleware();

app.UseAuthentication();
app.UseAuthorization();


app.UseEndpoints(endpoints =>
{

    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
