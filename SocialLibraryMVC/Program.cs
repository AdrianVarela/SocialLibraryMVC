using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialLibraryMVC.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

// Dependencies Injection for Authentication
builder.Services.AddAuthentication()
    .AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = builder.Configuration["Google:ClientId"];
        googleOptions.ClientSecret = builder.Configuration["Google:ClientSecret"];
    })
    .AddFacebook(facebookOptions =>
    {
        facebookOptions.ClientId = builder.Configuration["Facebook:AppId"];
        facebookOptions.ClientSecret = builder.Configuration["Facebook:AppSecret"];
    })
    .AddMicrosoftAccount(MicrosoftAccountOptions =>
    {
        MicrosoftAccountOptions.ClientId = builder.Configuration["MicrosoftAccount:ClientId"];
        MicrosoftAccountOptions.ClientSecret = builder.Configuration["MicrosoftAccount:ClientSecret"];
    })
    .AddTwitter(TwitterOptions =>
    {
        TwitterOptions.ConsumerKey = builder.Configuration["Twitter:APIkey"];
        TwitterOptions.ConsumerSecret = builder.Configuration["Twitter:APISecret"];
    });
builder.Services.AddAuthorization();

var app = builder.Build();

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

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Books}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
