using Microsoft.AspNetCore.Mvc;
using quick_exchange_backend.Services;

namespace quick_exchange_backend.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        
        
        [HttpGet("google-login")]
        public async Task<ActionResult> GoogleLoginHandle()
        {
            var response = await _authService.LoginGoogle();

            if (!response.IsSuccess) 
            {
                return StatusCode(response.StatusCode, new { message = response.Message });
            }
            
            return StatusCode(response.StatusCode, new { message = response.Message });
        }

        [HttpGet("callback")]
        public async Task<ActionResult> Callback([FromQuery] string code)
        {
            var response = await _authService.Callback(code);

            if (!response.IsSuccess)
            {
                return StatusCode(response.StatusCode, new { message = response.Message });
            }

            return Redirect(response.Message);
        }
    }
}