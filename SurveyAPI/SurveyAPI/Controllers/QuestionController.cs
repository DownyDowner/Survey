using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SurveyAPI.Models;
using SurveyAPI.Services;

namespace SurveyAPI.Controllers {
    [Route("api/questions")]
    [ApiController]
    public class QuestionController(QuestionService service, UserManager<IdentityUser> userManager) : ControllerBase {
        [HttpPost, Authorize]
        public async Task<ActionResult<Guid>> Create(QuestionFull question) {
            return await service.Create(question);
        }

        [HttpGet("active"), Authorize]
        public async Task<ActionResult<List<QuestionList>>> GetAllActiveQuestions() {
            return await service.GetAllActiveQuestions();
        }

        [HttpGet("expired"), Authorize]
        public async Task<ActionResult<List<QuestionList>>> GetAllExpiredQuestions() {
            return await service.GetAllExpiredQuestions();
        }

        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<QuestionFull>> GetQuestion([FromRoute] Guid id) { 
            return await service.GetQuestion(id);
        }

        [HttpGet("{id}/stats"), Authorize]
        public async Task<ActionResult<QuestionStats>> GetQuestionStats([FromRoute] Guid id) {
            return await service.GetQuestionStats(id);
        }

        [HttpPost("{id}/submit"), Authorize]
        public async Task<ActionResult> Submit([FromRoute] Guid id, [FromBody] List<Guid> choiceIds) {
            string? userId = userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId)) {
                return Unauthorized(new { Message = "User is not authorized" });
            }

            await service.Submit(id, userId, choiceIds);

            return Ok();
        }
    }
}
