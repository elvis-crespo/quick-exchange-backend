using quick_exchange_backend.Models.DTOs;

namespace quick_exchange_backend.Services
{
    public interface IAuthService
    {
        Task<ServiceResponse.GeneralResponse> LoginGoogle();
        Task<TokenResponse> GetTokenResponse(string code);
        Task<ServiceResponse.GeneralResponse> Callback(string code);
    }
}
