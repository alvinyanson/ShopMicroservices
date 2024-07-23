using AuthService.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AuthService.Services.Contracts
{
    public interface IAccountService
    {
        Task<IdentityResult> RegisterAsync(RegisterAccount registerAccount);

        Task<SignInResult> LoginAsync(LoginAccount loginAccount);

        Task SignOutAsync();

        Task<bool> IsSignedIn(ClaimsPrincipal claimsPrincipal);

        Task<IdentityUser> FindByEmailAsync(string email);

        Task<IdentityUser> FindByIdAsync(string ownerId);

        Task<ClaimsPrincipal> CreateUserPrincipalAsync(IdentityUser identityUser);

        Task<IdentityResult> ChangePasswordAsync(IdentityUser identityUser, string password, string newPassword);

        Task<IdentityResult> ChangeEmail(IdentityUser ownerId, string email);

        Task<IdentityResult> ChangeUsername(IdentityUser ownerId, string email);
    }
}
