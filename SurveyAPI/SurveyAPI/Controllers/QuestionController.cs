using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurveyAPI.Models;
using SurveyAPI.Services;

namespace SurveyAPI.Controllers {
    [Route("api/questions")]
    [ApiController]
    public class QuestionController(QuestionService service) : ControllerBase {
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
    }
}
