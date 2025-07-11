using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MVC.Domain.Models;
using MVC.Services.ApplicationDBContextService;
using MVC.WebAPI.Data;
using MVC.WebAPI.Services.TokenServices;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Database connection
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("default")));

builder.Services.AddIdentity<ApplicationUserModel, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
   {
       options.SaveToken = true;
       options.RequireHttpsMetadata = false;
       options.TokenValidationParameters = new TokenValidationParameters
       {
           ValidateIssuer = true,
           ValidateAudience = true,
           ValidAudience = builder.Configuration["JWT:ValidAudience"],
           ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
           ClockSkew = TimeSpan.Zero,
           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:secret"]))
       };
   });

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
});
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(option =>
    {
        option.SwaggerEndpoint("/openapi/v1.json", "WebAPI v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await DbSeeder.SeedData(app);

ApplyMigrations(app);
app.Run();

void ApplyMigrations(IApplicationBuilder app)
{
    using var scope = app.ApplicationServices.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if(dbContext.Database.GetPendingMigrations().Count() > 0)
        dbContext.Database.Migrate();
}