using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SurveyAPI.Constants;
using SurveyAPI.Models.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SurveyAPI.Controllers {
    [Route("api/auth")]
    [ApiController]
    public class AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) : ControllerBase {
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginModel model) {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null) {
                return Unauthorized("Invalid credentials");
            }

            var result = await signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (!result.Succeeded) {
                return Unauthorized("Invalid credentials");
            }

            string token = GenerateJwtToken(user);

            return Ok(token);
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterModel model) {
            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) {
                return BadRequest(result.Errors);
            }

            await userManager.AddToRoleAsync(user, RoleConstants.USER);

            return Ok("User registered successfully");
        }

        [HttpPost("register-admin"), Authorize(Roles = RoleConstants.ADMIN)]
        public async Task<ActionResult> RegisterAdmin([FromBody] RegisterModel model) {
            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) {
                return BadRequest(result.Errors);
            }

            await userManager.AddToRoleAsync(user, RoleConstants.ADMIN);

            return Ok("Admin registered successfully with role 'admin'");
        }

        [HttpPost("logout")]
        public async Task<ActionResult> Logout() {
            await signInManager.SignOutAsync();
            return Ok("User logged out successfully");
        }

        private string GenerateJwtToken(IdentityUser user) {
            if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Email)) {
                throw new InvalidOperationException("UserName or Email cannot be null.");
            }

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var roles = userManager.GetRolesAsync(user).Result;
            foreach (var role in roles) {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT__Key")!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: Environment.GetEnvironmentVariable("JWT__Issuer"),
                audience: Environment.GetEnvironmentVariable("JWT__Audience"),
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
