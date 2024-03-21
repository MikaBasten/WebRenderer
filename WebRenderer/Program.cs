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

// Register your connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<YourDbContext>(options =>
    options.UseSqlServer(connectionString));

// Retrieve secret key from configuration
var secretKey = builder.Configuration["JwtSettings:SecretKey"];

// Register UserRepo
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Register the PasswordHasher
builder.Services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();

// Register the LoginService with the required dependencies
builder.Services.AddScoped<ILoginService>(serviceProvider =>
{
    var userRepository = serviceProvider.GetRequiredService<IUserRepository>();
    return new LoginService(userRepository, secretKey, serviceProvider.GetRequiredService<IPasswordHasher<User>>());
});

var app = builder.Build();

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
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
