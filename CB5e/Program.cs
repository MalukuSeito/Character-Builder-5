using BlazorDB;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using CB5e;
using CB5e.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton<ContextService>();
builder.Services.AddSingleton<ConfigService>();
builder.Services.AddSingleton<SourceService>();
builder.Services.AddSingleton<ImageService>();

builder.Services.AddBlazorDB(options =>
{
    options.Name = "StorageTest";
    options.Version = 1;
    options.StoreSchemas = new List<StoreSchema>()
    {
        new StoreSchema()
        {
            Name = "Test2",
            PrimaryKey = "name",
            PrimaryKeyAuto = false
        }
    };
});

builder.Services.AddBlazoredSessionStorage();
builder.Services.AddBlazoredLocalStorage();


await builder.Build().RunAsync();
