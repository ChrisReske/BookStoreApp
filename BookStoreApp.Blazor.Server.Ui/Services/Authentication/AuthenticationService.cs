using Blazored.LocalStorage;
using BookStoreApp.Blazor.Server.Ui.Providers;
using BookStoreApp.Blazor.Ui.Services.Base;
using Microsoft.AspNetCore.Components.Authorization;

namespace BookStoreApp.Blazor.Server.Ui.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IClient _httpClient;
    private readonly ILocalStorageService _localStorageService;
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    public AuthenticationService(
        IClient httpClient, 
        ILocalStorageService localStorageService, 
        AuthenticationStateProvider authenticationStateProvider)
    {
        _httpClient = httpClient 
                      ?? throw new ArgumentNullException(nameof(httpClient));
        _localStorageService = localStorageService 
                               ?? throw new ArgumentNullException(nameof(localStorageService));
        this._authenticationStateProvider = authenticationStateProvider 
                                            ?? throw new ArgumentNullException(nameof(authenticationStateProvider));
    }

    public async Task<bool> AuthenticateAsync(LoginUserDto loginUserDto)
    {
        var response = await _httpClient.LoginAsync(loginUserDto);

        // Store Token
        await _localStorageService.SetItemAsync("accessToken", response.Token);

        // Change state of app
        await ((ApiAuthenticationStateProvider) _authenticationStateProvider).LoggedIn();

        return true;
    }

    public async Task Logout()
    {
        await ((ApiAuthenticationStateProvider)_authenticationStateProvider).LoggedOut();
    }
}