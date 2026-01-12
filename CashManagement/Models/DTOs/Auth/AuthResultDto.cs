namespace CashManagement.Models.DTOs.Auth
{
    public class AuthResultDto
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiredAt { get; set; }    
    }
}
