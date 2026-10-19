using ApniDukaan.Core.RequestDTO;
using ApniDukaan.Core.ResponseDTO;
using ApniDukaan.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LoginRequest = ApniDukaan.Core.RequestDTO.LoginRequest;

namespace ApniDukaan.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            if (registerRequest is null)
            {
                return BadRequest("Invalid registration request.");
            }

            // Add Validation logic here (e.g., check if email is valid, password strength, etc.)

            // Call the UserService to register the user
            AuthenticationResponse? authResponse = await _userService.Register(registerRequest);
            if (authResponse is null)
            {
                return BadRequest("User Already Exists or Registration Failed.");
            }

            return Ok(authResponse);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            if (loginRequest is null 
                || String.IsNullOrEmpty(loginRequest.Email) 
                || String.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest("Invalid login request.");
            }

            // Add Validation logic here (e.g., check if email is valid, password is not empty, etc.)

            // Call the login service to authenticate the user
            AuthenticationResponse? authResponse = await _userService.Login(loginRequest);
            if (authResponse is null)
            {
                return BadRequest("Invalid email or password.");
            }

            return Ok(authResponse);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequestDto refreshTokenRequest)
        {
            if (refreshTokenRequest is null || String.IsNullOrEmpty(refreshTokenRequest.RefreshToken))
            {
                return BadRequest("Invalid refresh token request.");
            }

            // Call the refresh token service
            AuthenticationResponse? authResponse = await _userService.RefreshToken(refreshTokenRequest);
            if (authResponse is null)
            {
                return BadRequest("Invalid or expired refresh token.");
            }

            return Ok(authResponse);
        }

        [Authorize]
        [HttpGet("TestAuthentication")]
        public async Task<IActionResult> TestAuthentication()
        {
            return Ok("Authentication is working correctly.");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("AdminAccessOnly")]
        public async Task<IActionResult> AdminAccessOnly()
        {
            return Ok("Admin logged in. Authorization is working correctly.");
        }
    }
}
