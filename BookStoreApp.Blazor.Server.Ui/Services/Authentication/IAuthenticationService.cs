using BookStoreApp.Blazor.Ui.Services.Base;

namespace BookStoreApp.Blazor.Server.Ui.Services.Authentication;

public interface IAuthenticationService
{
    Task<bool> AuthenticateAsync(LoginUserDto loginUserDto);
    Task Logout();

}