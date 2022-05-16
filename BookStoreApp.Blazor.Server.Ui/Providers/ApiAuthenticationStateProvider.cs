﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace BookStoreApp.Blazor.Server.Ui.Providers;

public class ApiAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorageService;
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

    public ApiAuthenticationStateProvider(
        ILocalStorageService localStorageService, 
        JwtSecurityTokenHandler jwtSecurityTokenHandler)
    {
        _localStorageService = localStorageService
                               ?? throw new ArgumentNullException(nameof(localStorageService));
          _jwtSecurityTokenHandler = jwtSecurityTokenHandler;

    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity());

        var savedToken = await _localStorageService.GetItemAsync<string>("accessToken"); 
        if (savedToken == null)
        {
            return new AuthenticationState(user);
        }

        var tokenContent = _jwtSecurityTokenHandler.ReadJwtToken(savedToken);

        if(tokenContent.ValidTo < DateTime.Now)
        {
            return new AuthenticationState(user);
        }

        var claims = tokenContent.Claims;

        user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));

        return new AuthenticationState(user);
    }

    public async Task LoggedIn()
    {
        var savedToken = await _localStorageService.GetItemAsync<string>("accessToken");
        var tokenContent = _jwtSecurityTokenHandler.ReadJwtToken(savedToken);
        var claims = tokenContent.Claims;
        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
        var authState = Task.FromResult(new AuthenticationState(user));
        NotifyAuthenticationStateChanged(authState);

    }

    public async Task LoggedOut()
    {
        await _localStorageService.RemoveItemAsync("accessToken");
        var nobody = new ClaimsPrincipal(new ClaimsIdentity());
        var authState = Task.FromResult(new AuthenticationState(nobody));
        NotifyAuthenticationStateChanged(authState);


    }
}