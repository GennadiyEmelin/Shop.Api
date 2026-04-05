using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Shop.Api.DTO.Auth;
using Shop.Api.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace Shop.Api.Services
{
    public class AuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;

        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }

        public async Task Register(RegisterDTO dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new Exception($"Ошибка регистрации: {errors}");
            }

            await _userManager.AddToRoleAsync(user, "User");
        }

        public async Task<string> Login(LoginDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                throw new Exception("Неверный логин или пароль");

            return GenerateJwtToken(user);
        }

        public async Task GetAdmin(LoginDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if(user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                throw new Exception("Неверный логин или пароль");

            await _userManager.AddToRoleAsync(user, "Admin");
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            var jwtSettings = _config.GetSection("Jwt");
            var roles = _userManager.GetRolesAsync(user).Result;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    int.Parse(jwtSettings["ExpiresInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
