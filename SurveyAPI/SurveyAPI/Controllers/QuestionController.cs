﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SurveyAPI.Constants;
using SurveyAPI.Models;
using SurveyAPI.Services;

namespace SurveyAPI.Controllers {
    [Route("api/questions")]
    [ApiController]
    public class QuestionController(QuestionService service, UserManager<IdentityUser> userManager) : ControllerBase {
        [HttpPost, Authorize(Roles = RoleConstants.ADMIN)]
        public async Task<ActionResult<Guid>> Create(QuestionFull question) {
            return await service.Create(question);
        }

        [HttpGet("active"), Authorize]
        public async Task<ActionResult<List<QuestionList>>> GetAllActiveQuestions() {
            return await service.GetAllActiveQuestions();
        }

        [HttpGet("closed"), Authorize]
        public async Task<ActionResult<List<QuestionList>>> GetAllClosedQuestions() {
            return await service.GetAllClosedQuestions();
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
