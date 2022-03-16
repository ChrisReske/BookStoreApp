using Blazored.LocalStorage;
using BookStoreApp.Blazor.Server.Ui.Providers;
using BookStoreApp.Blazor.Server.Ui.Services.Authentication;
using BookStoreApp.Blazor.Ui.Services.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

#region Blazored

builder.Services.AddBlazoredLocalStorage();

#endregion

#region Register and configure HttpClient

builder.Services.AddHttpClient<IClient, Client>(cl => cl
    .BaseAddress = new Uri("https://localhost:7144"));

#endregion

#region AuthenticationService

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ApiAuthenticationStateProvider>();

#endregion

#region Setup and configure AuthenticationStateProvider

builder.Services.AddScoped<AuthenticationStateProvider>(p => 
    p.GetRequiredService<ApiAuthenticationStateProvider>());


#endregion

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

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
