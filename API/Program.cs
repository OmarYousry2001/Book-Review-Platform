using Api;
using API;
using Bl.Middleware;
using DAL.ApplicationContext;
using DAL.Contracts.Repositories;
using DAL.Contracts.Repositories.Generic;
using DAL.Contracts.UnitOfWork;
using DAL.Repositories;
using DAL.Repositories.Generic;
using DAL.UnitOfWork;
using Domains.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Resources;
using Resources.Enumerations;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Globalization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

RegisterServicesHelper.RegisteredServices(builder);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddControllers();

var app = builder.Build();

// Use resources for multi-language support
ResourceManager.CurrentLanguage = Language.English;
// Use resources for multi-language support
var supportedCultures = new List<CultureInfo>
{
    new CultureInfo("en-US"),
    new CultureInfo("ar-EG")
};

app.UseRequestLocalization(new RequestLocalizationOptions
{
    // Default  For Language
    DefaultRequestCulture = new RequestCulture(ResourceManager.GetCultureName(ResourceManager.CurrentLanguage)),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures,
    RequestCultureProviders = new List<IRequestCultureProvider>
    {
        new CookieRequestCultureProvider(), // Check culture from cookies
        new AcceptLanguageHeaderRequestCultureProvider() // Fallback to header if no cookie
    }
});

app.Use(async (context, next) =>
{
    var feature = context.Features.Get<IRequestCultureFeature>();
    var culture = feature?.RequestCulture.Culture.Name ?? "ar-EG"; // Fallback to default if null

    // Proceed to the next middleware in the pipeline
    await next();
});
app.UseStaticFiles();
// Enable GZip compression
app.UseResponseCompression();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

// Configure the HTTP request pipeline.
app.UseSwagger();
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
}
else
{
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/Endpoint/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = "Endpoint"; // Set this to match your API path if needed
    });

    app.MapGet("/", async context =>
    {
        context.Response.Redirect("/Endpoint/Endpoint/index.html");
        await context.Response.CompleteAsync(); // Ensure the async Task is returned
    });
}

// Configure rate limiting middleware
app.UseMiddleware<RateLimitingMiddleware>();

app.MapControllers();

// Apply migrations and seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var dbContext = services.GetRequiredService<ApplicationDbContext>();

    // Apply migrations
    await dbContext.Database.MigrateAsync();

    // Seed data
    await ContextConfigurations.SeedDataAsync(dbContext, userManager, roleManager);
}

app.Run();
