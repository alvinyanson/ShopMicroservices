using AuthService.AsyncDataServices;
using AuthService.Dtos;
using AuthService.Models;
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
        private readonly IHttpContextHelper _httpContextHelper;
        private readonly IMessageBusClient _messageBusClient;
        private readonly IAccountService _accountService;
        private readonly IJWTService _jwtService;
        private readonly IMapper _mapper;

        public AuthController(
            IHttpContextHelper httpContextHelper,
            IMessageBusClient messageBusClient,
            IAccountService accountService, 
            IJWTService jwtService,
            IMapper mapper)
        {
            _httpContextHelper = httpContextHelper;
            _messageBusClient = messageBusClient;
            _accountService = accountService;
            _jwtService = jwtService;
            _mapper = mapper;
        }

        [HttpPost(nameof(Login))]
        public async Task<ActionResult> Login(Login request)
        {
            // Attempt to login
            var loginResult = await _accountService.LoginAsync(request);

            if (!loginResult.Succeeded)
            {
                return Unauthorized(new { success = false, message = "Invalid email address or password!" });
            }

            try
            {
                // Generate JWT token
                string jwt = await GenerateJWT(request.Email);

                // Success logged in
                return Ok(new { success = true, message = "User logged in successfully!", result = jwt });

            }
            catch
            {
                return StatusCode(500, new { success = false, message = $"Something went wrong!" });
            }
        }

        [HttpPost(nameof(Register))]
        public async Task<ActionResult<string>> Register(Register request)
        {
            try
            {
                // Check if email already exist in database
                var existingUser = await _accountService.FindByEmailAsync(request.Email);

                if(existingUser != null)
                {
                    return Ok(new { success = false, message = $"The email {request.Email} is already registered!" });
                }

                // Register the user
                var registerResult = await _accountService.RegisterAsync(request);

                if (!registerResult.Succeeded)
                {
                    return BadRequest(new { success = false, message = registerResult.Errors.FirstOrDefault()?.Description });
                }

                // Generate JWT token
                string jwt = await GenerateJWT(request.Email);

                // Async event AuthService -> ProductCatalogService (listen for new user, send a welcome email or special offers on ProductCatalogService)
                var userSignUpDto = _mapper.Map<RegisterUserDto>(request);
                _messageBusClient.UserSignUp(userSignUpDto);

                // Success register
                return Ok(new { success = true, message = "User registered successfully!", result = jwt });
            }

            catch
            {
                return StatusCode(500, new { success = false, message = "Something went wrong!" });
            }
        }

        [HttpPost(nameof(ChangePassword))]
        public async Task<ActionResult<string>> ChangePassword(ChangePasswordDto request)
        {
            try
            {
                // Check if token is valid
                string token = _httpContextHelper.GetTokenFromHeaders();

                string splitToken = token.ToString().Split(' ', 2)[1];

                bool isValidToken = _jwtService.ValidateJwtToken(splitToken);

                if (string.IsNullOrEmpty(token) || !isValidToken)
                {
                    return Unauthorized();
                }


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
                return StatusCode(500, new { success = false, message = "Something went wrong!" });
            }
        }

        [HttpPost(nameof(ChangeEmailAndUsername))]
        public async Task<ActionResult<string>> ChangeEmailAndUsername(ChangeEmailAndUsernameDto request)
        {
            try
            {
                // Check if token is valid
                string token = _httpContextHelper.GetTokenFromHeaders();

                string splitToken = token.ToString().Split(' ', 2)[1];

                bool isValidToken = _jwtService.ValidateJwtToken(splitToken);

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
                return StatusCode(500, new { success = false, message = "Something went wrong!" });
            }
        }

        private async Task<string> GenerateJWT(string email)
        {
            var identityUser = await _accountService.FindByEmailAsync(email);

            var principal = await _accountService.CreateUserPrincipalAsync(identityUser);

            return _jwtService.GenerateJwtToken(principal.Claims);
        }


        //HTTP CONNECTIONS FROM PRODUCT CATALOG SERVICE

        [HttpGet(nameof(TestConnection))]
        public ActionResult<string> TestConnection()
        {
            return Ok(new { success = true, message = "Connection established" });
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

        [HttpGet(nameof(IsTokenValid))]
        public ActionResult<bool> IsTokenValid(string token)
        {
            try
            {
                string splitToken = token.ToString().Split(' ', 2)[1];

                return _jwtService.ValidateJwtToken(splitToken);
            }
            catch
            {
                return Ok(false);
            }
        }
    }
}
