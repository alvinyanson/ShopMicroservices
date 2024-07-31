using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;
using ShopWebApp.Services;
using ShopWebApp.Services.Contracts;
using ShopWebApp.Services.HttpClients;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<AuthService>();

// auth
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
    .AddCookie(options =>
    {
        options.LoginPath = $"/Identity/Auth/Login";
        options.LogoutPath = $"/Identity/Auth/Logout";
    })
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration.GetConnectionString("AuthService");
        options.Audience = builder.Configuration["Jwt:Issuer"];
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mysecretkey")),
            ValidateIssuer = true,
            ValidateAudience = true,
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddMvc(options =>
{
    var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

    options.Filters.Add(new AuthorizeFilter(policy));
}).AddXmlSerializerFormatters();


var app = builder.Build();


using (var serviceScope = builder.Services.BuildServiceProvider().CreateScope())
{
    var authService = serviceScope.ServiceProvider.GetService<AuthService>();

    List<IHttpServiceWrapper> httpServices = new List<IHttpServiceWrapper>
    {
        new HttpService<HttpAuthService>(builder.Configuration, authService),
        new HttpService<HttpProductCatalogService>(builder.Configuration, authService),
    };

    //foreach (var service in httpServices)
    //{
    //    string serviceStatus = await service.IsRunning() ? "UP" : "DOWN";
    //    Console.WriteLine($"\t{service.Service.NormalizedName} ({service}): {serviceStatus}");
    //}
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();
