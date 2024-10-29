global using D20Tek.Functional;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WealthTracker;
using WealthTracker.Persistence;

await CreateBlazorApplication(args)
        .Map(builder => builder.Build())
        .Map(app => app.RunAsync()).Get();

Identity<WebAssemblyHostBuilder> CreateBlazorApplication(string[] args) =>
    WebAssemblyHostBuilder.CreateDefault(args)
                          .ToIdentity()
                          .Iter(b => b.RootComponents.Add<App>("#app"))
                          .Iter(b => b.RootComponents.Add<HeadOutlet>("head::after"))
                          .Iter(b => b.Services.AddScoped(sp =>
                                new HttpClient { BaseAddress = new Uri(b.HostEnvironment.BaseAddress) })
                                .AddRepository());
