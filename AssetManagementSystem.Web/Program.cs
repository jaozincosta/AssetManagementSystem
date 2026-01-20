using AssetManagementSystem.Web.Components;
using AssetManagementSystem.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configurar HttpClient para a API
builder.Services.AddHttpClient<UserService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7282/");
});

builder.Services.AddHttpClient<AssetService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7282/");
});

builder.Services.AddHttpClient<AllocationService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7282/");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();