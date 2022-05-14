using Blazored.LocalStorage;
using BookStoreApp.Blazor.Server.Ui.Providers;
using BookStoreApp.Blazor.Server.Ui.Services.Authentication;
using BookStoreApp.Blazor.Ui.Services.Base;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

SetupAndConfigureBlazored(builder);

//Register and configure HttpClient
builder.Services.AddHttpClient<IClient, Client>(cl => cl
    .BaseAddress = new Uri("https://localhost:7144"));

//AuthenticationService

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ApiAuthenticationStateProvider>();

// Setup and configure AuthenticationStateProvider
builder.Services.AddScoped<AuthenticationStateProvider>(p => 
    p.GetRequiredService<ApiAuthenticationStateProvider>());

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

void SetupAndConfigureBlazored(WebApplicationBuilder webApplicationBuilder)
{
    webApplicationBuilder.Services.AddBlazoredLocalStorage();
}
