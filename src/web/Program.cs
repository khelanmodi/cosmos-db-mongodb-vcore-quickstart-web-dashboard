WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddTransient<IDataService, MongoDataService>();

builder.Services.AddHttpClient();

builder.Services.Configure<Settings>(
    builder.Configuration.GetSection(nameof(Settings))
);

WebApplication app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

await app.RunAsync();