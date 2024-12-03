namespace SurveyAPI.Models {
    public class ChoiceStats {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int VoteCount { get; set; }
    }
}
