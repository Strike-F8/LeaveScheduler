using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using LeaveScheduler.Data;
using LeaveScheduler.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Storage;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<LeaveSchedulerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LeaveSchedulerContext") ?? throw new InvalidOperationException("Connection string 'LeaveSchedulerContext' not found.")));

// Add services to the container.

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/Error";
        options.LoginPath = "/Login";
        options.LogoutPath = "/Login/Logout";
        options.ReturnUrlParameter = "/Schedule";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ManagersOnly", policy => policy.RequireClaim("EmployeeID"));
    options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
});

builder.Services.AddMvc();
var app = builder.Build();

// create the database
ApplyMigrations(app);

static void ApplyMigrations(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<LeaveSchedulerContext>();
    db.Database.Migrate();
}

// Seed the database if it is empty
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    SeedData.Initialize(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.MapRazorPages();

app.MapDefaultControllerRoute();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
var cookiePolicyOptions = new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
};
app.UseCookiePolicy(cookiePolicyOptions);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
