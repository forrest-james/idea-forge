using Application.Common.Interfaces;
using Data.EF;
using Data.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Middleware;
using WebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// Middleware
builder.Services.AddScoped<ExceptionHandlingMiddleware>();

// DbContext
var connString = builder.Configuration.GetConnectionString("Default")
    ?? @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=IdeaForge_Local;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False;Command Timeout=30";

builder.Services.AddDbContext<AppDbContext>(options => { options.UseSqlServer(connString); });

// Application ports
builder.Services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<AppDbContext>());
builder.Services.AddScoped<IImageStorage, LocalImageStorage>();

// MediatR
builder.Services.AddMediatR(config =>
    config.RegisterServicesFromAssembly(typeof(Application.AssemblyMarker).Assembly));

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Identity
builder.Services
    .AddDefaultIdentity<IdentityUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireNonAlphanumeric = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/account/login";
    options.LogoutPath = "/account/logout";
    options.AccessDeniedPath = "/account/login";
});

var app = builder.Build();

// Global exception handling
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    await IdentitySeeder.SeedAsync(app.Services);
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Challenges}/{action=Index}/{id?}"
);

app.Run();