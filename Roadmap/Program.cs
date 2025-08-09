using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

var environment = builder.Environment;
var configuration = builder.Configuration;
var services = builder.Services;

// In production, this app will sit behind a Nginx reverse proxy with HTTPS
if (!environment.IsDevelopment())
    builder.WebHost.UseUrls("http://localhost:5012");

services.AddControllersWithViews();

services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

var app = builder.Build();

app.UseForwardedHeaders();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.MapStaticAssets();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
        "default",
        "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();