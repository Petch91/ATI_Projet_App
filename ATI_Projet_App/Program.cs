using ATI_Projet_App.Components;
using ATI_Projet_App.Services;
using ATI_Projet_App.Tools;
using ATI_Projet_Tools;
using ATI_Projet_Tools.Services.Interfaces;
using ATI_Projet_Tools.Tools;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.WebEncoders;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using ATI_Projet_Cultures.Tools;

var builder = WebApplication.CreateBuilder(args);


//builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services
    .AddBlazorise(options =>
    {
       options.Immediate = true;
    })
    .AddBootstrapProviders()
    .AddFontAwesomeIcons();
builder.Services.AddBlazorBootstrapPerso();
//builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://192.168.123.238:7001/api/") });
//TestDeveloppement 
//builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7214/api/") });
builder.Services.AddScoped<ProtectedLocalStorage>();
builder.Services.AddScoped<SessionManager>();
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
builder.Services.AddScoped<ApiRequester>();


builder.Services.AddHttpClient("api", c => { c.BaseAddress = new Uri("http://192.168.123.238:7001/api/"); });
builder.Services.AddHttpClient();
builder.Services.AddHttpClient("BC14", c =>
{
   c.BaseAddress = new Uri("https://bc14-test.eesb.be:7348/BC14-TEST-NUP/ODataV4/Company('ATI%20Indus')/");
   var username = "ATI_WS";
   var password = "U4VsMoxs4179tL7VDgkBcoffrAJVKVibmEoopIbPelw=";

   // Encodez les informations d'identification en Base64
   var authToken = Encoding.ASCII.GetBytes($"{username}:{password}");
   var authHeaderValue = Convert.ToBase64String(authToken);

   // Ajoutez l'en-tête d'autorisation
   c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
});

builder.Services.AddScoped<ISociete, SocieteService>();
builder.Services.AddScoped<ICommon, CommonService>();
builder.Services.AddScoped<IProjet, ProjetService>();
builder.Services.AddScoped<IPersonnel, PersonnelService>();

builder.Services.AddSingleton<VariablesGlobales>();

builder.Services.AddServerSideBlazor().AddCircuitOptions(option => { option.DetailedErrors = true; });

builder.Services.Configure<WebEncoderOptions>(options =>
{
   options.TextEncoderSettings = new TextEncoderSettings(
       UnicodeRanges.All);
});

builder.Services.AddLocalization();
builder.Services.AddScoped<LanguageChangeNotifier>();
builder.Services.AddScoped(typeof(IStringLocalizer<>), typeof(CustomStringLocalizer<>));
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
   options.SetDefaultCulture("fr-BE");
   options.AddSupportedCultures(new[] { "fr-BE", "en-US" });
   options.AddSupportedUICultures(new[] { "fr-BE", "en-US" });
});

var app = builder.Build();


app.UseRequestLocalization();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
   app.UseExceptionHandler("/Error", createScopeForErrors: true);
   // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
   app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

//app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();


app.Run();
