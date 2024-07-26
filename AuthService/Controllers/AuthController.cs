using AuthService.AsyncDataServices;
using AuthService.Dtos;
using AuthService.Models;
using AuthService.Services;
using AuthService.Services.Contracts;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IHttpContextHelper _httpContextHelper;
        private readonly IMessageBusClient _messageBusClient;
        private readonly IAccountService _accountService;
        private readonly JWTService _jwtService;
        private readonly IMapper _mapper;

        public AuthController(
            IHttpContextHelper httpContextHelper,
            IMessageBusClient messageBusClient,
            IAccountService accountService, 
            JWTService jwtService,
            IMapper mapper)
        {
            _httpContextHelper = httpContextHelper;
            _messageBusClient = messageBusClient;
            _accountService = accountService;
            _jwtService = jwtService;
            _mapper = mapper;
        }

        [HttpPost(nameof(Login))]
        public async Task<ActionResult> Login(LoginAccount account)
        {
            // Attemp to login
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
                return StatusCode(500, new { success = false, message = $"Something went wrong!" });
            }
        }

        [HttpPost(nameof(Register))]
        public async Task<ActionResult<string>> Register(RegisterAccount account)
        {
            try
            {
                // Check if email already exist in database
                var existingUser = await _accountService.FindByEmailAsync(account.Email);

                if(existingUser != null)
                {
                    return Ok(new { success = false, message = $"The email {account.Email} is already registered!" });
                }

                // Register the user
                var registerResult = await _accountService.RegisterAsync(account);

                if (!registerResult.Succeeded)
                {
                    return BadRequest(new { success = false, message = registerResult.Errors.FirstOrDefault()?.Description });
                }

                // Generate JWT token
                string jwt = await GenerateJWT(account.Email);

                // Async event AuthService -> ProductCatalogService (listen for new user, send a welcome email or special offers on ProductCatalogService)
                //var userSignUpDto = _mapper.Map<UserSignUpDto>(account);
                //_messageBusClient.UserSignUp(userSignUpDto);

                // Success register
                return Ok(new { success = true, message = "User registered successfully!", result = jwt });
            }

            catch
            {
                return StatusCode(500, new { success = false, message = $"Something went wrong!" });
            }
        }

        [HttpPost(nameof(ChangePassword))]
        public async Task<ActionResult<string>> ChangePassword(ChangePasswordDto request)
        {
            try
            {
                // Find existing user
                var existingUser = await _accountService.FindByEmailAsync(request.Email);
                if (existingUser == null)
                {
                    return NotFound(new { success = false, message = $"The email {request.Email} does not exist!" });
                }

                // Change password
                var changePasswordResult = await _accountService.ChangePasswordAsync(existingUser, request.Password, request.NewPassword);

                if (!changePasswordResult.Succeeded)
                {
                    return BadRequest(new { success = false, message = changePasswordResult.Errors.FirstOrDefault()?.Description });
                }

                // Success change of password
                return Ok(new { success = true, message = "Password changed successfully!" });
            }

            catch
            {
                return StatusCode(500, new { success = false, message = $"Something went wrong!" });
            }
        }

        [HttpPost(nameof(ChangeEmailAndUsername))]
        public async Task<ActionResult<string>> ChangeEmailAndUsername(ChangeEmailAndUsernameDto request)
        {
            try
            {
                // Check if token is valid
                string token = _httpContextHelper.GetBearerTokenFromHeaders();
                bool isValidToken = _jwtService.ValidateJwtToken(token);

                if (string.IsNullOrEmpty(token) || !isValidToken)
                {
                    return Unauthorized();
                }

                // Find existing user
                var existingUser = await _accountService.FindByEmailAsync(request.OldEmail);
                if (existingUser == null)
                {
                    return NotFound(new { success = false, message = $"The email {request.OldEmail} does not exist!" });
                }

                // Change Email
                var changeEmailResult = await _accountService.ChangeEmail(existingUser, request.NewEmail);
                if (!changeEmailResult.Succeeded)
                {
                    return BadRequest(new { success = false, message = changeEmailResult.Errors.FirstOrDefault()?.Description });
                }

                // Change Username
                var changeUsernameResult = await _accountService.ChangeUsername(existingUser, request.NewEmail);
                if (!changeUsernameResult.Succeeded)
                {
                    return BadRequest(new { success = false, message = changeUsernameResult.Errors.FirstOrDefault()?.Description });
                }

                // Success change of email username
                return Ok(new { success = true, message = $"Email and username have been changed to {request.NewEmail}!" });
            }

            catch
            {
                return StatusCode(500, new { success = false, message = $"Something went wrong!" });
            }
        }

        [HttpGet(nameof(GetProfile))]
        public async Task<ActionResult> GetProfile(string email)
        {
            try
            {
                // Check if token is valid
                string token = _httpContextHelper.GetBearerTokenFromHeaders();
                bool isValidToken = _jwtService.ValidateJwtToken(token);

                if (string.IsNullOrEmpty(token) || !isValidToken)
                {
                    return Unauthorized();
                }

                // Find existing user
                var existingUser = await _accountService.FindByEmailAsync(email);
                if (existingUser == null)
                {
                    return NotFound(new { success = false, message = $"The user with email {email} does not exist!" });
                }

                // Success get profile
                return Ok(new { success = true, message = "Profile retrieved!", result = existingUser });
            }

            catch
            {
                return StatusCode(500, new { success = false, message = $"Something went wrong!" });
            }
        }

        [HttpPost(nameof(Logout))]
        public async Task<ActionResult> Logout()
        {
            await _accountService.SignOutAsync();
            return Ok(new { success = true, message = "User logged out successfully!" });
        }

        [HttpGet(nameof(TestConnection))]
        public ActionResult<string> TestConnection()
        {
            return Ok("Connection established...");
        }

        private async Task<string> GenerateJWT(string email)
        {
            var identityUser = await _accountService.FindByEmailAsync(email);

            var principal = await _accountService.CreateUserPrincipalAsync(identityUser);

            return _jwtService.GenerateJwtToken(principal.Claims);
        }

    }
}
