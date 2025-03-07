using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using tickets_bug_BSD2023_2025.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<tickets_bug_BSD2023_2025Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("tickets_bug_BSD2023_2025Context") ?? throw new InvalidOperationException("Connection string 'tickets_bug_BSD2023_2025Context' not found.")));

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(1);
    options.IOTimeout = TimeSpan.FromDays(1);
    options.Cookie.Name = "CookieConnexion";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Path = "/";
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
