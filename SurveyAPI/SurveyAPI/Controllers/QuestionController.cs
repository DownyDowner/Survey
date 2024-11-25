using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SurveyAPI.Controllers {
    [Route("api/questions")]
    [ApiController]
    public class QuestionController : ControllerBase {
        [HttpGet("active")]
        public ActionResult GetAllActiveQuestions() {
            return Ok();
        }
    }
}
