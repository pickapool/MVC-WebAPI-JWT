using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authentication.Cookies;
using MVC.Services.BaseService;
using MVC.Services.LoginServices;
using MVC.Services.TokenProviderServices;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration.GetValue<string>("BaseAPI")) });
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenProvider, TokenProvider>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/";
        options.AccessDeniedPath = "/Home/Index";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Patient}/{action=Patients}/{id?}")
    .WithStaticAssets();


app.Run();
