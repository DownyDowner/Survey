using Microsoft.AspNetCore.Identity;

namespace SurveyAPI.Entities {
    public class ResponseEntity {
        public Guid Id { get; set; }
        public string IdUser { get; set; } = string.Empty;
        public Guid IdChoice { get; set; }
        public DateTime ResponseDate { get; set; } = DateTime.Now.ToUniversalTime();

        public ChoiceEntity? Choice { get; set; }
        public IdentityUser? User { get; set; }
    }
}
