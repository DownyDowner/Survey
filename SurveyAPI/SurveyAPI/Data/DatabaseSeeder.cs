namespace SurveyAPI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SurveyAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class DatabaseSeeder {
    private readonly DataContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public DatabaseSeeder(DataContext context, UserManager<IdentityUser> userManager) {
        _context = context;
        _userManager = userManager;
    }

    public async Task SeedAsync() {
        _context.Database.EnsureCreated();
        _context.Set<ResponseEntity>().RemoveRange(_context.Set<ResponseEntity>());
        _context.Set<QuestionEntity>().RemoveRange(_context.Set<QuestionEntity>());
        _context.Set<ChoiceEntity>().RemoveRange(_context.Set<ChoiceEntity>());
        _context.Set<IdentityUser>().RemoveRange(_context.Set<IdentityUser>());

        await _context.SaveChangesAsync();

        var questions = new List<QuestionEntity>
        {
            new QuestionEntity
            {
                Id = Guid.NewGuid(),
                Name = "What is your favorite programming language?",
                BeginDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(10),
                Multiple = false,
                Choices = new List<ChoiceEntity>
                {
                    new ChoiceEntity { Id = Guid.NewGuid(), Name = "C#" },
                    new ChoiceEntity { Id = Guid.NewGuid(), Name = "Python" },
                    new ChoiceEntity { Id = Guid.NewGuid(), Name = "JavaScript" }
                }
            },
            new QuestionEntity
            {
                Id = Guid.NewGuid(),
                Name = "Do you prefer remote work or on-site work?",
                BeginDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(7),
                Multiple = false,
                Choices = new List<ChoiceEntity>
                {
                    new ChoiceEntity { Id = Guid.NewGuid(), Name = "Remote" },
                    new ChoiceEntity { Id = Guid.NewGuid(), Name = "On-site" }
                }
            },
            new QuestionEntity
            {
                Id = Guid.NewGuid(),
                Name = "What is your preferred development environment?",
                BeginDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(14),
                Multiple = true,
                Choices = new List<ChoiceEntity>
                {
                    new ChoiceEntity { Id = Guid.NewGuid(), Name = "Windows" },
                    new ChoiceEntity { Id = Guid.NewGuid(), Name = "MacOS" },
                    new ChoiceEntity { Id = Guid.NewGuid(), Name = "Linux" }
                }
            },
            new QuestionEntity
            {
                Id = Guid.NewGuid(),
                Name = "How often do you participate in coding competitions?",
                BeginDate = DateTime.UtcNow.AddDays(-30),
                EndDate = DateTime.UtcNow.AddDays(-15),
                Multiple = false,
                Choices = new List<ChoiceEntity>
                {
                    new ChoiceEntity { Id = Guid.NewGuid(), Name = "Frequently" },
                    new ChoiceEntity { Id = Guid.NewGuid(), Name = "Occasionally" },
                    new ChoiceEntity { Id = Guid.NewGuid(), Name = "Never" }
                }
            },
            new QuestionEntity
            {
                Id = Guid.NewGuid(),
                Name = "Which cloud platform do you use the most?",
                BeginDate = DateTime.UtcNow.AddDays(-60),
                EndDate = DateTime.UtcNow.AddDays(-30),
                Multiple = false,
                Choices = new List<ChoiceEntity>
                {
                    new ChoiceEntity { Id = Guid.NewGuid(), Name = "AWS" },
                    new ChoiceEntity { Id = Guid.NewGuid(), Name = "Azure" },
                    new ChoiceEntity { Id = Guid.NewGuid(), Name = "Google Cloud" }
                }
            }
        };

        await _context.Set<QuestionEntity>().AddRangeAsync(questions);
        await _context.SaveChangesAsync();

        var users = new List<IdentityUser>
        {
            new IdentityUser { UserName = "user1@example.com", Email = "user1@example.com" },
            new IdentityUser { UserName = "user2@example.com", Email = "user2@example.com" },
            new IdentityUser { UserName = "user3@example.com", Email = "user3@example.com" }
        };

        foreach (var user in users) {
            var result = await _userManager.CreateAsync(user, "Password@123");
            if (!result.Succeeded) {
                foreach (var error in result.Errors) {
                    Console.WriteLine(error.Description);
                }
            }
        }

        var user1 = await _userManager.FindByEmailAsync("user1@example.com");
        var choices = _context.Set<ChoiceEntity>().ToList();

        if (user1 != null && choices.Any()) {
            var responses = new List<ResponseEntity>
            {
                new ResponseEntity
                {
                    Id = Guid.NewGuid(),
                    IdUser = user1.Id,
                    IdChoice = choices.First().Id,
                    ResponseDate = DateTime.UtcNow
                }
            };

            await _context.Set<ResponseEntity>().AddRangeAsync(responses);
            await _context.SaveChangesAsync();
        }
    }
}