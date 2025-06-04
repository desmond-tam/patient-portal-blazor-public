using PatientPortalApp.Data;
using PatientPortalApp.Services;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

var builder = WebApplication.CreateBuilder(args);
var connection = String.Empty;

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddEnvironmentVariables().AddJsonFile("appsettings.Development.json");
    connection = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");
}
else
{
    connection = Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING");
}

builder.Services.AddBlazorBootstrap();

builder.Services.AddDbContext<PatientPortalContext>(options =>
    options.UseSqlServer(connection)
    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking),
    ServiceLifetime.Transient);

                //.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking),
                //ServiceLifetime.Transient

// Add services to the container.
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddControllersWithViews()
    .AddMicrosoftIdentityUI();

if (!builder.Environment.IsDevelopment())
{
    builder.Services.AddAuthorization(options =>
    {
        // By default, all incoming requests will be authorized according to the default policy
        options.FallbackPolicy = options.DefaultPolicy;
    });
}

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor()
    .AddMicrosoftIdentityConsentHandler();
builder.Services.AddScoped<WeatherForecastService>();
builder.Services.AddScoped<ISlotService, SlotService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<IBookingService, BookingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();


app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");


app.Run();
