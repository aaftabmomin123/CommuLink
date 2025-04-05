
using _2FA_.Common;
using _2FA_.Interface;
using _2FA_.Middleware;
using _2FA_.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

// Configure session without cookie settings
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(1); 
    options.Cookie.HttpOnly = true; 
    options.Cookie.IsEssential = true; 
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
});

builder.Services.AddHttpContextAccessor(); 
builder.Services.AddScoped<ApiHelper>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IEmailTemplateService, EmailTemplateService>();//new added for emailtemplate
//builder.Services.AddScoped<IEmailTemplateRepository, EmailTemplateRepositoryservices.AddScoped<ICountryService, CountryService>();

builder.Services.AddScoped<ICountryService, countryService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();




var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession();
app.UseMiddleware<TokenValidationMiddleware>();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=login}/{id?}");

app.Run();
