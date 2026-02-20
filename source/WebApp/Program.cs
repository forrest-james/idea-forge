using Application.Common.Interfaces;
using Data.EF;
using Data.Persistence;
using Microsoft.EntityFrameworkCore;
using WebApp.Middleware;
using WebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=IdeaForge_Local;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False;Command Timeout=30");
    options.EnableSensitiveDataLogging();
    options.LogTo(Console.WriteLine);
});

builder.Services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<AppDbContext>());
builder.Services.AddScoped<IImageStorage, LocalImageStorage>();

builder.Services.AddMediatR(config =>
    config.RegisterServicesFromAssembly(typeof(Application.AssemblyMarker).Assembly));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{    
    app.UseHsts();
}

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
