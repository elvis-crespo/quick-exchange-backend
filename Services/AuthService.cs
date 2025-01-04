using quick_exchange_backend.Models.DTOs;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

namespace quick_exchange_backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly string clientId = "YOUR CLIENT ID";
        private readonly string clientSecret = "YOUR CLIENT SECRET";
        private readonly string redirectUri = "REDIRECT";

        //https://oauth2.googleapis.com/tokeninfo?access_token=

        public async Task<TokenResponse> GetTokenResponse(string code)
        {
            using (var client = new HttpClient())
            {
                var tokenRequestData = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret),
                new KeyValuePair<string, string>("redirect_uri", redirectUri),
                new KeyValuePair<string, string>("grant_type", "authorization_code")
            });

                var response = await client.PostAsync("https://oauth2.googleapis.com/token", tokenRequestData);
                var responseJson = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<TokenResponse>(responseJson);
                }

                return null;
            }
        }
    

        public async Task<ServiceResponse.GeneralResponse> LoginGoogle()
        { 
            var authUrl =
                $"{"https://accounts.google.com/o/oauth2/v2/auth"}"+
                $"?response_type=code&client_id={clientId}" +
                $"&redirect_uri={redirectUri}&scope=openid%20email%20profile";

            return new ServiceResponse.GeneralResponse(
                IsSuccess: true,
                StatusCode: StatusCodes.Status200OK,
                Message: $"{authUrl}"
            );
        }

        // Handling redirection from Google with authorization code
        public async Task<ServiceResponse.GeneralResponse> Callback(string code)
        {
            var tokenResponse = await GetTokenResponse(code);
            Console.WriteLine(tokenResponse);
            if (tokenResponse == null)
            {
                return new ServiceResponse.GeneralResponse(
                    IsSuccess: true,
                    StatusCode: StatusCodes.Status204NoContent,
                    Message: "Error al obtener los tokens."
                ); 
            }

            //var redirectUrl = $"http://localhost:5173";
            var redirectUrl = $"http://localhost:5173/home?accessToken={tokenResponse.AccessToken}&idToken={tokenResponse.IdToken}";

            return new ServiceResponse.GeneralResponse(
                IsSuccess: true,
                StatusCode: StatusCodes.Status200OK,
                Message: redirectUrl
            );

        }

    }
}
