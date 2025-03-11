using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SurveyAPI.Constants;
using SurveyAPI.Models.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SurveyAPI.Services {
    public class AuthService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) {
        public async Task<string> Login(LoginModel model) {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null) {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            var result = await signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (!result.Succeeded) {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            return GenerateJwtToken(user);
        }

        public async Task RegisterUser(RegisterModel model) {
            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) {
                throw new InvalidOperationException("User registration failed");
            }

            await userManager.AddToRoleAsync(user, RoleConstants.USER);
        }

        public async Task RegisterAdmin(RegisterModel model) {
            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) {
                throw new InvalidOperationException("Admin registration failed");
            }

            await userManager.AddToRoleAsync(user, RoleConstants.ADMIN);
        }

        public async Task Logout() {
            await signInManager.SignOutAsync();
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

        public async Task<IdentityUser> GetCurrentUser(ClaimsPrincipal user) {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null) {
                throw new UnauthorizedAccessException("User not authenticated");
            }

            var currentUser = await userManager.FindByIdAsync(userId);

            if (currentUser == null) {
                throw new InvalidOperationException("User not found");
            }

            return currentUser;
        }
    }
}
