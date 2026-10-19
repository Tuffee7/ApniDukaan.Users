using ApniDukaan.Core.Entities;
using ApniDukaan.Core.RepositoryContracts;
using ApniDukaan.Core.RequestDTO;
using ApniDukaan.Core.ResponseDTO;
using ApniDukaan.Core.ServiceContracts;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ApniDukaan.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, 
                           IMapper mapper,
                           IConfiguration configuration)
        { 
            _userRepository = userRepository;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<AuthenticationResponse?> Register(RegisterRequest registerRequest)
        {
            ApplicationUser applicationUser = _mapper.Map<ApplicationUser>(registerRequest);

            // Hash the password before storing it in Database
            var hashedPassword = new PasswordHasher<ApplicationUser>().HashPassword(applicationUser, registerRequest?.Password);
            applicationUser.PasswordHash = hashedPassword;

            ApplicationUser? registeredUser = await _userRepository.AddUser(applicationUser);

            if (registeredUser is null)
                return null;

            var authenticationResponse = _mapper.Map<AuthenticationResponse>(registeredUser);
            authenticationResponse.AccessToken = CreateToken(registeredUser); // In a real application, generate a JWT or similar token here
            authenticationResponse.RefreshToken = await GenerateAndSaveRefreshTokenAsync(registeredUser);

            return authenticationResponse;
        }

        public async Task<AuthenticationResponse?> Login(LoginRequest loginRequest)
        {

            ApplicationUser? user = await _userRepository.GetUserByEmail(loginRequest.Email);

            if (user == null)
                return null; // Invalid Email Id

            if (new PasswordHasher<ApplicationUser>().VerifyHashedPassword(user, user.PasswordHash, loginRequest.Password) == PasswordVerificationResult.Failed)
            {
                return null; // Invalid password
            }

            var authenticationResponse = _mapper.Map<AuthenticationResponse>(user);
            authenticationResponse.AccessToken = CreateToken(user); // In a real application, generate a JWT or similar token here
            authenticationResponse.RefreshToken = await GenerateAndSaveRefreshTokenAsync(user);

            return authenticationResponse;
        }

        public async Task<UserDTO?> GetUserByUserID(Guid? userID)
        {
            ApplicationUser? user = await _userRepository.GetUserByUserID(userID);

            if (user == null)
                return null;

            return _mapper.Map<UserDTO>(user);
        }

        public async Task<IEnumerable<UserDTO>?> GetAllUsers()
        {
            IEnumerable<ApplicationUser>? users = await _userRepository.GetAllUsers();
            if (users == null)
                return null;
            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }

        public async Task<AuthenticationResponse?> RefreshToken(RefreshTokenRequestDto refreshTokenRequest)
        {
            ApplicationUser? user = await ValidateRefreshToken(refreshTokenRequest.UserId, refreshTokenRequest.RefreshToken);

            if (user is null)
                return null; // Invalid refresh token or expired

            var authenticationResponse = _mapper.Map<AuthenticationResponse>(user);
            authenticationResponse.AccessToken = CreateToken(user); // Generate a new JWT
            authenticationResponse.RefreshToken = await GenerateAndSaveRefreshTokenAsync(user); // Generate a new refresh token
            
            return authenticationResponse;
        }

        private string CreateToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.PersonName),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
                //new Claim(ClaimTypes.Email, user.Email) commenting email field so that email is not exposed if someone decodes the token. This to prevent leaking of email ids.
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JwtSettings:SecretKey"))); // Replace with your secret key of having length of 512 bits or 64 bytes/32characters because we're using HMACSHA512 algorithm

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("JwtSettings:Issuer"),
                audience: _configuration.GetValue<string>("JwtSettings:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        private async Task<string> GenerateAndSaveRefreshTokenAsync(ApplicationUser user)
        {
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            bool isUpdated  = await _userRepository.UpdateUser(user);

            if (!isUpdated)
            {
                throw new Exception("Failed to update user with refresh token.");
            }

            return refreshToken;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }

        private async Task<ApplicationUser?> ValidateRefreshToken(Guid userId, string refreshToken)
        {
            ApplicationUser? user = await _userRepository.GetUserByUserID(userId);
            if (user is null 
                || user.RefreshToken != refreshToken 
                || user.RefreshTokenExpiry < DateTime.UtcNow)
            {
                return null; // Invalid refresh token or expired
            }

            return user;
        }
    }
}