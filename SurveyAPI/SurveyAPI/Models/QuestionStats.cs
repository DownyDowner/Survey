namespace SurveyAPI.Models {
    public class QuestionStats {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Multiple { get; set; } = false;
        public List<ChoiceStats> Choices { get; set; } = new List<ChoiceStats>();
    }
}
