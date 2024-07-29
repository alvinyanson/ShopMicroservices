using AuthService.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AuthService.Services.Contracts
{
    public interface IAccountService
    {
        Task<IdentityResult> RegisterAsync(Register registerAccount);

        Task<SignInResult> LoginAsync(Login loginAccount);

        Task<IdentityUser> FindByEmailAsync(string email);

        Task<IdentityUser> FindByIdAsync(string ownerId);

        Task<ClaimsPrincipal> CreateUserPrincipalAsync(IdentityUser identityUser);

        Task<IdentityResult> ChangePasswordAsync(IdentityUser identityUser, string password, string newPassword);

        Task<IdentityResult> ChangeEmail(IdentityUser ownerId, string email);

        Task<IdentityResult> ChangeUsername(IdentityUser ownerId, string email);
    }
}
