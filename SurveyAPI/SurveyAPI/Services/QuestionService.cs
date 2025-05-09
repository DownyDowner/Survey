﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurveyAPI.Data;
using SurveyAPI.Models;

namespace SurveyAPI.Services {
    public class QuestionService(DataContext dataContext) {

        public async Task<Guid> Create(QuestionSave question) {
            var entity = question.ToEntity();
            await dataContext.Questions.AddAsync(entity);
            await dataContext.SaveChangesAsync();

            return entity.Id;
        }

        public async Task Delete(Guid id) {
            var question = await dataContext.Questions.FindAsync(id);

            if (question == null) {
                throw new KeyNotFoundException("Question not found");
            }

            dataContext.Questions.Remove(question);
            await dataContext.SaveChangesAsync();
        }

        public async Task<List<QuestionList>> GetAllActiveQuestions() {
            DateTime today = DateTime.Now.ToUniversalTime();

            var entities = await dataContext.Questions
                .Where(q => q.BeginDate <= today && q.EndDate >= today)
                .OrderBy(q => q.EndDate)
                .ToListAsync();

            return entities.Select(e => e.ToDTOList()).ToList();
        }

        public async Task<List<QuestionList>> GetAllClosedQuestions() {
            DateTime today = DateTime.Now.ToUniversalTime();

            var entities = await dataContext.Questions
                .Where(q => q.EndDate < today)
                .OrderByDescending(q => q.EndDate)
                .ToListAsync();

            return entities.Select(e => e.ToDTOList()).ToList();
        }

        public async Task<QuestionStats> GetQuestionStats(Guid id) {
            var question = await dataContext.Questions
                .Select(q => new QuestionStats {
                    Id = q.Id,
                    Name = q.Name,
                    BeginDate = q.BeginDate,
                    EndDate = q.EndDate,
                    Multiple = q.Multiple,
                    Choices = q.Choices != null
                    ? q.Choices.Select(c => new ChoiceStats {
                        Id = c.Id,
                        Name = c.Name,
                        VoteCount = c.Responses != null ? c.Responses.Count : 0
                    }).ToList() : new List<ChoiceStats>()
                })
                .FirstOrDefaultAsync(e => e.Id == id);

            if (question == null)
                throw new InvalidOperationException("Question not found.");

            if (question.EndDate > DateTime.Now)
                throw new InvalidOperationException("Statistics not available. Question is still active.");

            return question;
        }


        public async Task Submit(Guid id, string userId, List<Guid> choiceIds) {
            var entity = await dataContext.Questions
                .Include(e => e.Choices)
                .FirstOrDefaultAsync(e => e.Id == id);

            DateTime today = DateTime.Now.ToUniversalTime();

            if (entity == null)
                throw new InvalidOperationException("Question not found.");

            if (entity.Choices == null || !entity.Choices.Any())
                throw new InvalidOperationException("Choices not found.");

            if (today < entity.BeginDate || today > entity.EndDate)
                throw new InvalidOperationException("The current date is not within the valid date range for this question.");

            if (!entity.Multiple && choiceIds.Count() > 1)
                throw new InvalidOperationException("Multiple choices are not allowed for this question.");

            foreach (var choiceId in choiceIds) {
                if (!entity.Choices.Any(c => c.Id == choiceId))
                    throw new InvalidOperationException($"Choice {choiceId} is not valid for this question.");

                var response = ExtensionsMethods.ToEntity(userId, choiceId, today);

                dataContext.Responses.Add(response);
            }

            await dataContext.SaveChangesAsync();
        }

        public async Task<QuestionFull> GetQuestionWithUserVotes(Guid id, string userId) {
            var entity = await dataContext.Questions
            .Include(q => q.Choices!)
                .ThenInclude(c => c.Responses)
            .FirstOrDefaultAsync(q => q.Id == id);

            if (entity == null)
                throw new InvalidOperationException("Question not found.");

            if (DateTime.Now < entity.BeginDate || DateTime.Now > entity.EndDate)
                throw new InvalidOperationException("Question not active.");

            var userVotes = entity.Choices?
                .Where(c => c.Responses != null)
                .SelectMany(c => c.Responses!)
                .Where(r => r.IdUser == userId)
                .Select(r => r.IdChoice)
                .ToList() ?? new List<Guid>();

            return entity.ToDTOFull(userVotes);
        }
    }
}
