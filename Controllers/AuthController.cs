using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Api.DTO.Auth;
using Shop.Api.Services;

namespace Shop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            await _authService.Register(dto);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var token = await _authService.Login(dto);
            return Ok(new { token });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("get-admin")]
        public async Task<IActionResult> GetAdmin(LoginDTO dto)
        {
            await _authService.GetAdmin(dto);
            return Ok();
        }
    }
}
