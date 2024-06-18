using Microsoft.EntityFrameworkCore;
using NotificationWebsite.DataAccess.Data;
using NotificationWebsite.Utility.Helpers.Validation;
using NotificationWebsite.Utility.Helpers.NotificationActions;
using NotificationWebsite.Utility.Jwt;
using NotificationWebsite.Utility.Jwt.JwtConfiguration;
using Hangfire;
using Newtonsoft.Json;
using NotificationWebsite.Utility.Oauth.Configuration;
using Telegram.Bot;
using NotificationWebsite.Utility.Configuration.TelegramBot;
using NotificationWebsite.Utility.Configuration;

var builder = WebApplication.CreateBuilder(args);

//Services:
builder.Services.AddCors();

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UsersConnection"))); // connect to UsersDb

builder.Services.AddHangfire(config => config.UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection")));
builder.Services.AddHangfireServer();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILoginValidation, CheckValidation>();

builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));//get jwt sections from appsettings

builder.Services.AddScoped<ITelegramBotConfiguration, TelegramBotConfiguration>();

builder.Services.AddScoped<HttpClient>();

var telegramBotToken = builder.Configuration.GetSection("Telegram_token");// get telegram bot token form json settings
builder.Services.AddSingleton<ITelegramBotClient>(new TelegramBotClient($"{telegramBotToken.Value}"));// add telegram bot (DI)

GlobalConfiguration.Configuration.UseSerializerSettings// to avoid reference recursion exception
(
    new JsonSerializerSettings
    {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    }
);

var jwtAuthenticationService = new JwtConfiguration();
jwtAuthenticationService.ConfigureJwtAuthentication(builder.Services, builder.Configuration);

var oauthConfigration = new Oauth2Configuration();
oauthConfigration.ConfigureOauth2(builder.Services, builder.Configuration);

builder.Services.AddScoped<INotificationActions, NotificationActions>();

builder.Services.AddHttpContextAccessor();


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

app.UseCors(options => options
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
);

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();