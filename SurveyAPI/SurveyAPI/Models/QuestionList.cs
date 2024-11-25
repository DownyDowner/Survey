namespace SurveyAPI.Models {
    public class QuestionList {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime BeginDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; }
    }
}
