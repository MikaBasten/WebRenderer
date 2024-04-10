using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Core.Models;
using Core.IRepository;
using Dall.Repository;
using Core.IServices;
using Core.Services;
using Dall.DB;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configuration
builder.Configuration.AddJsonFile("appsettings.json", optional: false);
var siteBUrl = builder.Configuration["SiteBUrl"];

// Register your connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<YourDbContext>(options =>
    options.UseSqlServer(connectionString));

// Retrieve secret key from configuration
var secretKey = builder.Configuration["JwtSettings:SecretKey"];

// Register repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Register services
builder.Services.AddScoped<IUserService, UserService>(provider =>
    new UserService(
        provider.GetRequiredService<IUserRepository>(),
        provider.GetRequiredService<IPasswordHasher<User>>(),
        secretKey
    )
);

// Register the PasswordHasher
builder.Services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();

var app = builder.Build();

// Enable CORS with a policy that allows access only from SiteBUrl
app.UseCors(builder => builder.WithOrigins(siteBUrl).AllowAnyHeader().AllowAnyMethod());


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "login",
    pattern: "/login",
    defaults: new { controller = "User", action = "Login" }
);

app.MapControllerRoute(
    name: "register",
    pattern: "/register",
    defaults: new { controller = "User", action = "Register" }
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();