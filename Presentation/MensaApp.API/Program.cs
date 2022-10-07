using MensaApp.API.Extensions;
using MensaApp.API.Filters;
using MensaApp.API.Middlewares;
using MensaApp.Application;
using MensaApp.Infrastructure;
using MensaApp.Persistence;
using MensaApp.Persistence.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using NToastNotify;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
CookieBuilder cookieBuilder = new();


// Add services to the container.
builder.Services.AddControllersWithViews(opts =>
{
    opts.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});

builder.Services.AddPersistenceServices();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddValidationServices();
builder.Services.AddDbContext<MensaDbContext>(options => options.UseSqlServer(Configuration.ConnectionString));
builder.Services.Configure<IPList>(builder.Configuration.GetSection("IPList"));
builder.Services.AddScoped<CheckWhiteList>();
builder.Services.AddMvc();

Logger logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console(
    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}--{UserName}")
    .WriteTo.File("D://Mensa_Log/log.txt",
    rollingInterval: RollingInterval.Day,
    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}--{UserName}")
    .WriteTo.Seq(builder.Configuration["Seq:ServerUrl"])
    .MinimumLevel.Information()
    .CreateLogger();

builder.Host.UseSerilog(logger);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Admin", options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidAudience = builder.Configuration["Token:Audience"],
            ValidIssuer = builder.Configuration["Token:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
            LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow : false,

            NameClaimType = ClaimTypes.Name
        };
    });

builder.Services.AddMvc().AddNToastNotifyNoty(new NotyOptions
{
    ProgressBar = true,
    Timeout = 5000,
    Theme = "mint"
});

cookieBuilder.Name = "MensaAdmin";
cookieBuilder.HttpOnly = false;
cookieBuilder.SecurePolicy = CookieSecurePolicy.SameAsRequest;
cookieBuilder.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict;

builder.Services.ConfigureApplicationCookie(opts =>
{
    opts.LoginPath = new PathString("/auth/index");
    opts.LogoutPath = new PathString("/auth/logout");
    opts.Cookie = cookieBuilder;
    opts.ExpireTimeSpan = System.TimeSpan.FromDays(10);
    opts.SlidingExpiration = true;
});

builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
});

builder.Services.AddCors(opts =>
{
    opts.AddPolicy("AllowSites", builder =>
    {
        builder.WithOrigins("https://localhost:7104", "http://localhost:5104").WithHeaders(HeaderNames.ContentType, "x-custom-header").AllowAnyMethod();
    });

    opts.AddPolicy("AllowSites2", builder =>
    {
        builder.WithOrigins("https://localhost:7104", "http://localhost:5104").WithMethods("POST", "GET").AllowAnyHeader();
    });

    opts.AddPolicy("AllowSites3", builder =>
    {
        builder.WithOrigins("http://localhost:49158/", "https://localhost:49157/").WithMethods("POST", "GET").AllowAnyHeader();
    });
});

builder.Services.AddDataProtection();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.ConfiguraExceptionHandler<Program>(app.Services.GetRequiredService<ILogger<Program>>());

app.UseStaticFiles();

app.UseSerilogRequestLogging();

app.UseStatusCodePagesWithReExecute("/Error/PageNotFound", "?code={0}");

//app.UseMiddleware<IPSafeMiddleware>();

app.UseNToastNotify();

app.UseRouting();

// app.UseCors("AllowSites3");

app.UseAuthentication();

app.UseAuthorization();

app.Use(async (context, next) =>
{
    var username = context.User.Identity.IsAuthenticated ? context.User.Identity.Name : "anonymous";

    LogContext.PushProperty("UserName", username);

    await next.Invoke();
});

app.MapControllerRoute(
    name: "MyArea",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();