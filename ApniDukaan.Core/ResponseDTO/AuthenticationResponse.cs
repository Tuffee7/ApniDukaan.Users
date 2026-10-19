namespace ApniDukaan.Core.ResponseDTO
{
    public class AuthenticationResponse
    {
        public Guid UserID { get; set; }
        public string? Email { get; set; }
        public string? PersonName { get; set; }
        public string? Gender { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
