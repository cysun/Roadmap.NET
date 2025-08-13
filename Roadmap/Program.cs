using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Roadmap.Security;
using Roadmap.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var environment = builder.Environment;
var configuration = builder.Configuration;
var services = builder.Services;

// In production, this app will sit behind a Nginx reverse proxy with HTTPS
if (!environment.IsDevelopment())
    builder.WebHost.UseUrls("http://localhost:5012");

builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration));

// Configure services

services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = 200 * 1024 * 1024; // 100MB (default 30MB)
});

services.AddControllersWithViews();

services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = Constants.AuthenticationScheme.Oidc;
    })
    .AddCookie(opt =>
    {
        opt.AccessDeniedPath = "/Home/AccessDenied";
        opt.Cookie.MaxAge = TimeSpan.FromDays(90);
    })
    .AddOpenIdConnect(Constants.AuthenticationScheme.Oidc, options =>
    {
        configuration.Bind("OIDC", options);
        options.ResponseType = "code";
        options.Scope.Add("email");
        options.Scope.Add("roadmap");

        options.Events = new OpenIdConnectEvents
        {
            OnRemoteFailure = context =>
            {
                context.Response.Redirect("/");
                context.HandleResponse();
                return Task.FromResult(0);
            }
        };
    });

services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
});

services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

services.AddSingleton<AppMapper>();

services.AddScoped<UserService>();

// Build App

var app = builder.Build();

// Configure Middleware Pipeline

app.UseForwardedHeaders();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// MapStaticAssets() introduced in .NET 9 is supposed to be better than UseStaticFiles(), but for some
// reason even when we call MapStaticAssets() before UseAuthentication(), it still requires authentication
// (from the FallbackPolicy) to access static files.
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
        "default",
        "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();