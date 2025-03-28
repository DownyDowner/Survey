﻿namespace SurveyAPI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SurveyAPI.Constants;
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

        var users = new (string Email, string Role)[]
        {
            ("admin@example.com", RoleConstants.ADMIN),
            ("user1@example.com", RoleConstants.USER),
            ("user2@example.com", RoleConstants.USER)
        };

        foreach (var (email, role) in users) {
            var user = new IdentityUser { UserName = email, Email = email };
            await _userManager.CreateAsync(user, "Password@123");
            await _userManager.AddToRoleAsync(user, role);
        }

        var admin = await _userManager.FindByEmailAsync("admin@example.com");
        var user1 = await _userManager.FindByEmailAsync("user1@example.com");
        var user2 = await _userManager.FindByEmailAsync("user2@example.com");
        var allUsers = new List<IdentityUser> { admin, user1, user2 };
        var random = new Random();

        foreach (var user in allUsers) {
            var responses = new List<ResponseEntity>();

            foreach (var question in questions) {
                if (question.Multiple) {
                    var selectedChoices = question.Choices
                        .OrderBy(x => random.Next())
                        .Take(random.Next(1, 3))
                        .ToList();

                    foreach (var choice in selectedChoices) {
                        responses.Add(new ResponseEntity {
                            Id = Guid.NewGuid(),
                            IdUser = user.Id,
                            IdChoice = choice.Id,
                            ResponseDate = DateTime.UtcNow
                        });
                    }
                } else {
                    var selectedChoice = question.Choices
                        .OrderBy(x => random.Next())
                        .FirstOrDefault();

                    if (selectedChoice != null) {
                        responses.Add(new ResponseEntity {
                            Id = Guid.NewGuid(),
                            IdUser = user.Id,
                            IdChoice = selectedChoice.Id,
                            ResponseDate = DateTime.UtcNow
                        });
                    }
                }
            }

            await _context.Set<ResponseEntity>().AddRangeAsync(responses);
            await _context.SaveChangesAsync();
        }
    }
}