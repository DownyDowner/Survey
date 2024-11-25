namespace SurveyAPI.Entities {
    public class QuestionEntity {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime BeginDate { get; set; } = DateTime.Now.ToUniversalTime();
        public DateTime EndDate { get; set; }
        public bool Multiple { get; set; } = false;

        public ICollection<ChoiceEntity>? Choices { get; set; }
    }
}
