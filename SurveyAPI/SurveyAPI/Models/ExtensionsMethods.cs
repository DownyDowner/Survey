using SurveyAPI.Entities;

namespace SurveyAPI.Models {
    public static partial class ExtensionsMethods {
        public static QuestionList ToDTOList(this QuestionEntity entity) => new() {
            Id = entity.Id,
            Name = entity.Name,
            BeginDate = entity.BeginDate,
            EndDate = entity.EndDate,
        };

        public static QuestionEntity ToEntity(this QuestionFull question) => new() {
            Id = question.Id,
            Name = question.Name,
            BeginDate = question.BeginDate,
            EndDate = question.EndDate,
            Multiple = question.Multiple,
            Choices = question.Choices.Select(c => c.ToEntity()).ToList(),
        };

        public static ChoiceEntity ToEntity(this ChoiceFull choice) => new() {
            Id = choice.Id,
            Name = choice.Name,
        };
    }
}
