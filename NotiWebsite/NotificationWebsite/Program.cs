using Microsoft.EntityFrameworkCore;
using NotificationWebsite.DataAccess.Data;
using NotificationWebsite.Utility.Helpers.Validation;
using NotificationWebsite.Utility.Helpers.NotificationActions;
using NotificationWebsite.Utility.Oauth.Configuration;
using NotificationWebsite.Utility.Jwt;
using NotificationWebsite.Utility.Jwt.JwtConfiguration;
using NotificationWebsite.Utility.Oauth.OauthHelpers;
using Hangfire;
using Newtonsoft.Json;
using Google.Apis.Services;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using Google.Apis.Gmail.v1;
using NotificationWebsite.Utility.Oauth.Load;

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
builder.Services.AddScoped<ISecrets, Secrets>();
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));//get jwt sections from appsettings

builder.Services.AddScoped<HttpClient>();

GlobalConfiguration.Configuration.UseSerializerSettings
(
    new JsonSerializerSettings
    {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    }
);

var jwtAuthenticationService = new JwtConfiguration();
var Oauth2Service = new Oauth2Configuration();
jwtAuthenticationService.ConfigureJwtAuthentication(builder.Services, builder.Configuration);
Oauth2Service.ConfigureOauth2(builder.Services, builder.Configuration);

var credentials = GoogleWebAuthorizationBroker.AuthorizeAsync(
    new ClientSecrets
    {
        ClientId = OAuthClientInfo.Load(builder.Configuration).ClientId,
        ClientSecret = OAuthClientInfo.Load(builder.Configuration).ClientSecret
    },
    new[] { GmailService.Scope.GmailSend, GmailService.Scope.GmailCompose },
    "me",
    CancellationToken.None,
    new FileDataStore("Gmail.Credentials")
).Result;

var initializer = new BaseClientService.Initializer
{
    HttpClientInitializer = credentials,
    ApplicationName = "SendNotification"
};

var gmailService = new GmailService(initializer);

builder.Services.AddSingleton(credentials);
builder.Services.AddSingleton(gmailService);




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