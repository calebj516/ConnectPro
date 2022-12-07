using AspNetCore.ReCaptcha;
using ConnectPro.Helpers;
using ConnectPro.Models;
using ConnectPro.Services;
using ConnectPro.Services.Interfaces;
using ContactPro.Data;
using ContactPro.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//var connectionString = builder.Configuration.GetSection("pgSettings")["pgConnection"];
var connectionString = ConnectionHelper.GetConnectionString(builder.Configuration);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

// Custom Services
builder.Services.AddScoped<IImageService, ImageService>(); // create a new ImageService class on every request
builder.Services.AddScoped<IAddressBookService, AddressBookService>();
builder.Services.AddScoped<IEmailSender, EmailService>();

// With this line we are feeding the MailSettings class the information it needs from our user secrets file
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

// ReCaptcha
builder.Services.AddReCaptcha(builder.Configuration.GetSection("ReCaptcha"));

var app = builder.Build();
var scope = app.Services.CreateScope();

// This line keeps the database updated with the latest migration
await DataHelper.ManageDataAsync(scope.ServiceProvider);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Home/HandleError/{0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
