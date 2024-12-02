namespace SurveyAPI.Entities {
    public class ChoiceEntity {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid IdQuestion { get; set; }

        public QuestionEntity? Question { get; set; }
        public ICollection<ResponseEntity>? Responses { get; set; }
    }
}
