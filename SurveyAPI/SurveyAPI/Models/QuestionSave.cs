namespace SurveyAPI.Models {
    public class QuestionSave {
        public string Name { get; set; } = string.Empty;
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Multiple { get; set; } = false;
        public List<ChoiceSave> Choices { get; set; } = new List<ChoiceSave>();
    }
}
