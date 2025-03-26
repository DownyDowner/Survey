using SurveyAPI.Entities;

namespace SurveyAPI.Models {
    public static partial class ExtensionsMethods {
        public static QuestionList ToDTOList(this QuestionEntity entity) => new() {
            Id = entity.Id,
            Name = entity.Name,
            BeginDate = entity.BeginDate,
            EndDate = entity.EndDate,
        };

        public static QuestionEntity ToEntity(this QuestionSave question) => new() {
            Name = question.Name.Trim(),
            BeginDate = question.BeginDate,
            EndDate = question.EndDate,
            Multiple = question.Multiple,
            Choices = question.Choices.Select(c => c.ToEntity()).ToList(),
        };

        public static ChoiceEntity ToEntity(this ChoiceSave choice) => new() {
            Name = choice.Name.Trim(),
        };

        public static ResponseEntity ToEntity(string userId, Guid choiceId, DateTime responseDate) => new() {
            IdUser = userId,
            IdChoice = choiceId,
            ResponseDate = responseDate
        };

        public static QuestionFull ToDTOFull(this QuestionEntity entity, List<Guid> userVotes) => new() {
            Id = entity.Id,
            Name = entity.Name,
            BeginDate = entity.BeginDate,
            EndDate = entity.EndDate,
            Multiple = entity.Multiple,
            Choices = entity.Choices?.Select(c => c.ToDTOFull()).ToList() ?? new List<ChoiceFull>(),
            UserVotes = userVotes
        };

        public static ChoiceFull ToDTOFull(this ChoiceEntity entity) => new() {
            Id = entity.Id,
            Name = entity.Name
        };
    }
}
