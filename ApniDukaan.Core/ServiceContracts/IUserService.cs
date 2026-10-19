using ApniDukaan.Core.RequestDTO;
using ApniDukaan.Core.ResponseDTO;

namespace ApniDukaan.Core.ServiceContracts
{
    public interface IUserService
    {
        /// <summary>
        /// Registers a new user with the provided registration request details and returns an authentication response.
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        Task<AuthenticationResponse?> Register(RegisterRequest loginRequest);

        /// <summary>
        /// Logs in a user with the provided login request details and returns an authentication response.
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        Task<AuthenticationResponse?> Login(LoginRequest loginRequest);

        /// <summary>
        /// Retrieves user details based on the provided user ID and returns a UserDTO object.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        Task<UserDTO?> GetUserByUserID(Guid? userID);

        /// <summary>
        /// Retrieves all users and returns a list of UserDTO objects.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<UserDTO>?> GetAllUsers();

        /// <summary>
        /// Asynchronously refreshes the authentication token using the specified refresh token request.
        /// </summary>
        /// <remarks>Performs server-side validation of the refresh token; returns null for invalid or
        /// expired tokens.</remarks>
        /// <param name="refreshTokenRequest">The refresh token request containing the refresh token and any required metadata.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is an AuthenticationResponse containing
        /// the new access and refresh tokens, or null if the refresh fails.</returns>
        Task<AuthenticationResponse?> RefreshToken(RefreshTokenRequestDto refreshTokenRequest);
    }
}
