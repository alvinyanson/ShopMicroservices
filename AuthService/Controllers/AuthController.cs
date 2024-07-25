﻿using AuthService.AsyncDataServices;
using AuthService.Dtos;
using AuthService.Models;
using AuthService.Services;
using AuthService.Services.Contracts;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly JWTService _jwtService;
        private readonly IMessageBusClient _messageBusClient;
        private readonly IMapper _mapper;

        public AuthController(
            IAccountService accountService, 
            JWTService jwtService, 
            IMessageBusClient messageBusClient,
            IMapper mapper)
        {
            _accountService = accountService;
            _jwtService = jwtService;
            _messageBusClient = messageBusClient;
            _mapper = mapper;
        }

        [HttpPost(nameof(Login))]
        public async Task<ActionResult> Login(LoginAccount account)
        {
            var loginResult = await _accountService.LoginAsync(account);

            if (!loginResult.Succeeded)
            {
                return Unauthorized(new { success = false, message = "Invalid email address or password!" });
            }

            try
            {
                // Generate JWT token
                string jwt = await GenerateJWT(account.Email);

                // Success logged in
                return Ok(new { success = true, message = "User logged in successfully!", result = jwt });

            }
            catch
            {
                return StatusCode(500, new { success = false, message = $"Internal server error!" });
            }
        }


        [HttpPost(nameof(Register))]
        public async Task<ActionResult<string>> Register(RegisterAccount account)
        {
            try
            {

                var registerResult = await _accountService.RegisterAsync(account);

                if (!registerResult.Succeeded)
                {
                    return BadRequest(new { success = false, message = registerResult.Errors.FirstOrDefault()?.Description });
                }

                // Generate JWT token
                string jwt = await GenerateJWT(account.Email);

                var userSignUpDto = _mapper.Map<UserSignUpDto>(account);
                _messageBusClient.UserSignUp(userSignUpDto);

                // Success register
                return Ok(new { success = true, message = "User registered successfully!", result = jwt });
            }

            catch
            {
                return StatusCode(500, new { success = false, message = $"Internal server error!" });
            }
        }

        [HttpPost(nameof(ChangePassword))]
        public async Task<ActionResult<string>> ChangePassword(ChangePasswordAccount changePasswordAccount, string ownerId)
        {
            try
            {
                // Find user
                var user = await _accountService.FindByIdAsync(ownerId);
                if (user == null)
                {
                    return NotFound(new { success = false, message = "User not found" });
                }

                // Change Password
                var changePasswordResult = await _accountService.ChangePasswordAsync(user, changePasswordAccount.Password, changePasswordAccount.NewPassword);

                if (!changePasswordResult.Succeeded)
                {
                    return BadRequest(new { success = false, message = changePasswordResult.Errors.FirstOrDefault()?.Description });
                }

                // Success change of password
                return Ok(new { success = true, message = "Password changed successfully!" });
            }

            catch
            {
                return StatusCode(500, new { success = false, message = $"Internal server error!" });
            }
        }

        [HttpPost(nameof(ChangeEmailAndUsername))]
        public async Task<ActionResult<string>> ChangeEmailAndUsername(string ownerId, string email)
        {
            try
            {
                // Find user
                var user = await _accountService.FindByIdAsync(ownerId);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                // Change Email
                var changeEmailResult = await _accountService.ChangeEmail(user, email);
                if (!changeEmailResult.Succeeded)
                {
                    return BadRequest(new { success = false, message = changeEmailResult.Errors.FirstOrDefault()?.Description });
                }

                // Change Username
                var changeUsernameResult = await _accountService.ChangeUsername(user, email);
                if (!changeUsernameResult.Succeeded)
                {
                    return BadRequest(new { success = false, message = changeUsernameResult.Errors.FirstOrDefault()?.Description });
                }

                // Success change of email username
                return Ok(new { success = true, message = "Email and username have been changed!" });
            }

            catch
            {
                return StatusCode(500, new { success = false, message = $"Internal server error!" });
            }
        }

        [HttpGet(nameof(GetProfile))]
        public async Task<ActionResult> GetProfile(string ownerId)
        {
            try
            {
                // Find user
                var user = await _accountService.FindByIdAsync(ownerId);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                // Success get profile
                return Ok(new { success = true, message = "Profile retrieved!", result = user });
            }

            catch
            {
                return StatusCode(500, new { success = false, message = $"Internal server error!" });
            }
        }

        [HttpGet(nameof(GetUserId))]
        public ActionResult<string> GetUserId(string token)
        {
            string splitToken = token.ToString().Split(' ', 2)[1];

            var handler = new JwtSecurityTokenHandler();
            
            var jsonToken = handler.ReadJwtToken(splitToken);

            if (jsonToken is null)
            {
                return Unauthorized();
            }

            string id = jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? default!;
            return id;
        }


        [HttpPost(nameof(Logout))]
        public async Task<ActionResult> Logout()
        {
            await _accountService.SignOutAsync();
            return Ok(new { success = true, message = "User logged out successfully!" });
        }

        private async Task<string> GenerateJWT(string email)
        {
            // Generate JWT token
            var identityUser = await _accountService.FindByEmailAsync(email);

            var principal = await _accountService.CreateUserPrincipalAsync(identityUser);

            return _jwtService.GenerateJwtToken(principal.Claims);
        }

    }
}
