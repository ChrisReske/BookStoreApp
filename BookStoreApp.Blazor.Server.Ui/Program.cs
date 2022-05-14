using Blazored.LocalStorage;
using BookStoreApp.Blazor.Server.Ui.Providers;
using BookStoreApp.Blazor.Server.Ui.Services.Authentication;
using BookStoreApp.Blazor.Ui.Services.Base;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

ConfigureAndRegisterBlazored(builder);

ConfigureAndRegisterHttpClient(builder);

ConfigureAndRegisterAuthenticationService(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days.
    // You may want to change this for production scenarios,
    // see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

void ConfigureAndRegisterBlazored(WebApplicationBuilder webApplicationBuilder)
{
    webApplicationBuilder.Services.AddBlazoredLocalStorage();
}

void ConfigureAndRegisterHttpClient(WebApplicationBuilder webApplicationBuilder)
{
    const string httpsLocalhost = "https://localhost:7144";
    webApplicationBuilder.Services.AddHttpClient<IClient, Client>(cl =>
    {
        
        cl
            .BaseAddress = new Uri(httpsLocalhost);
    });
}

void ConfigureAndRegisterAuthenticationService(WebApplicationBuilder webApplicationBuilder)
{
    webApplicationBuilder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
    webApplicationBuilder.Services.AddScoped<ApiAuthenticationStateProvider>();

    // Setup and configure AuthenticationStateProvider
    webApplicationBuilder.Services.AddScoped<AuthenticationStateProvider>(p =>
        p.GetRequiredService<ApiAuthenticationStateProvider>());
}
