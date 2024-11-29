using Microsoft.EntityFrameworkCore;
using SurveyAPI.Data;
using SurveyAPI.Models;

namespace SurveyAPI.Services {
    public class QuestionService(DataContext dataContext) {

        public async Task<Guid> Create(QuestionFull question) { 
            var entity = question.ToEntity();
            await dataContext.Questions.AddAsync(entity);
            await dataContext.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<List<QuestionList>> GetAllActiveQuestions() {
            DateTime today = DateTime.Now.ToUniversalTime();

            var entities = await dataContext.Questions
                .Where(q => q.BeginDate <= today && q.EndDate >= today)
                .OrderBy(q => q.EndDate)
                .ToListAsync();

            return entities.Select(e => e.ToDTOList()).ToList();
        }

        public async Task<List<QuestionList>> GetAllExpiredQuestions() {
            DateTime today = DateTime.Now.ToUniversalTime();

            var entities = await dataContext.Questions
                .Where(q => q.EndDate < today)
                .OrderByDescending(q => q.EndDate)
                .ToListAsync();

            return entities.Select(e => e.ToDTOList()).ToList();
        }

        public async Task<QuestionFull> GetQuestion(Guid id) {
            var entity = await dataContext.Questions
                .Include(e => e.Choices)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (entity == null)
                throw new InvalidOperationException();

            return entity.ToDTOFull();
        }
    }
}
