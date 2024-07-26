﻿using AuthService.Common;
using AuthService.Models;
using AuthService.Services.Contracts;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Claims;
using System.Text;

namespace AuthService.Services
{
    public class AccountService : IAccountService
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public AccountService(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<bool> IsSignedIn(ClaimsPrincipal claimsPrincipal)
        {
            IdentityUser? identityUser = await _userManager.GetUserAsync(claimsPrincipal);

            if (identityUser is null)
            {
                return false;
            }

            return _signInManager.IsSignedIn(claimsPrincipal);
        }

        public async Task<SignInResult> LoginAsync(LoginAccount loginAccount)
        {
            return await _signInManager.PasswordSignInAsync(
                loginAccount.Email,
                loginAccount.Password,
                isPersistent: false,
                lockoutOnFailure: false);
        }

        public async Task<IdentityResult> RegisterAsync(RegisterAccount registerAccount)
        {
            var identityUser = _mapper.Map<IdentityUser>(registerAccount);

            var identityRole = new IdentityRole { Name = registerAccount.Role };

            IdentityResult identityResult = await _userManager.CreateAsync(identityUser, registerAccount.Password);

            await _roleManager.CreateAsync(new IdentityRole { Name = registerAccount.Role });

            await _userManager.AddToRoleAsync(identityUser, registerAccount.Role);

            return identityResult;
        }

        public async Task<IdentityUser> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<IdentityUser> FindByIdAsync(string ownerId)
        {
            return await _userManager.FindByIdAsync(ownerId);
        }

        public async Task<ClaimsPrincipal> CreateUserPrincipalAsync(IdentityUser identityUser)
        {
            return await _signInManager.CreateUserPrincipalAsync(identityUser);
        }

        public async Task<IdentityResult> ChangePasswordAsync(IdentityUser identityUser, string password, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(identityUser, password, newPassword);
        }

        public async Task<IdentityResult> ChangeEmail(IdentityUser user, string email)
        {
            var code = await _userManager.GenerateChangeEmailTokenAsync(user, email);

            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            // Changing email... simulate as if user click on confirm email button and gets redirected to confirmation pages
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            return await _userManager.ChangeEmailAsync(user, email, code);
        }

        public async Task<IdentityResult> ChangeUsername(IdentityUser user, string email)
        {
            return await _userManager.SetUserNameAsync(user, email);
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

    }
}
