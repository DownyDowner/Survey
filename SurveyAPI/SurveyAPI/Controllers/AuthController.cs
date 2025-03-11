using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurveyAPI.Constants;
using SurveyAPI.Models.Auth;
using SurveyAPI.Services;

namespace SurveyAPI.Controllers {
    [Route("api/auth")]
    [ApiController]
    public class AuthController(AuthService authService) : ControllerBase {
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginModel model) {
            try {
                var token = await authService.Login(model);
                return Ok(token);
            } catch (UnauthorizedAccessException) {
                return Unauthorized("Invalid credentials");
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterModel model) {
            try {
                await authService.RegisterUser(model);
                return Ok("User registered successfully");
            } catch (InvalidOperationException) {
                return BadRequest("Registration failed");
            }
        }

        [HttpPost("register-admin"), Authorize(Roles = RoleConstants.ADMIN)]
        public async Task<ActionResult> RegisterAdmin([FromBody] RegisterModel model) {
            try {
                await authService.RegisterAdmin(model);
                return Ok("Admin registered successfully");
            } catch (InvalidOperationException) {
                return BadRequest("Admin registration failed");
            }
        }

        [HttpPost("logout")]
        public async Task<ActionResult> Logout() {
            await authService.Logout();
            return Ok("User logged out successfully");
        }
    }
}
