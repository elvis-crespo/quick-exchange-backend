namespace quick_exchange_backend.Models.DTOs
{
    public class ServiceResponse
    {
        public record class GeneralResponse(bool IsSuccess, int StatusCode, string Message);
        public record class LoginGoogle(bool IsSuccess, int StatusCode, string Message, string AccessToken, int ExpiresIn, string RefreshToken, string Scope, string TokenType, string IdToken);
    }
}
