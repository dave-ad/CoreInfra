using CoreInfra.Shared.DTOs;
using CoreInfra.Shared.Models;

namespace CoreInfra.Client.Services;

public interface IAuthService
{
    Task<RegisterResult> Register(RegisterModel registerModel);
    Task<LoginResult> Login(LoginModel loginModel);
    Task Logout();
}
