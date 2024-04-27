using Microsoft.EntityFrameworkCore;
using NotificationWebsite.DataAccess.Data;
using NotificationWebsite.Utility.Configuration.Jwt;
using NotificationWebsite.Utility.Helpers.Validation;
using NotificationWebsite.Utility.Helpers.Jwt;

var builder = WebApplication.CreateBuilder(args);

//Services:
builder.Services.AddCors();

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UsersConnection"))); // connect to UsersDb

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILoginValidation, CheckValidation>();

builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));//get jwt sections from appsettings

builder.Services.AddScoped<HttpClient>();

var jwtAuthenticationService = new JwtConfiguration();
jwtAuthenticationService.ConfigureJwtAuthentication(builder.Services, builder.Configuration);
//builder.Services.AddSingleton(builder.Configuration);

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Main}/{action=Home}/{id?}");

app.Run();