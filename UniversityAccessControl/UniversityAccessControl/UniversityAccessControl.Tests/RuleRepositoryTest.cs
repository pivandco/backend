using Moq.EntityFrameworkCore;
using UniversityAccessControl.Models;

namespace UniversityAccessControl.Tests;

public sealed class RuleRepositoryTest
{
    private static readonly Group Group1 = new() { Name = "Group" };
    private static readonly Group Group2 = new() { Name = "Group" };

    private static readonly Subject Subject = new()
    {
        FirstName = "John", LastName = "Doe", DateOfBirth = new DateOnly(2002, 4, 25),
        Groups = new List<Group> { Group1, Group2 }
    };

    private static readonly Area Area = new() { Name = "Area" };

    private static readonly Passage Passage = new() { Name = "Passage", Area = Area };
    private readonly Mock<AccessControlDbContext> _dbContext = new();
    private readonly RuleRepository _repository;

    public RuleRepositoryTest()
    {
        _repository = new RuleRepository(_dbContext.Object);
    }

    [Fact]
    public void FindRulesRelatedToSubjectAndPassage_RulesPreciselyForSubjectAndPassage_ReturnsThem()
    {
        // Arrange
        var unrelatedSubject = new Subject
            { FirstName = "Random", LastName = "Dude", DateOfBirth = new DateOnly(2002, 4, 25) };
        var rule1 = new Rule { Subject = Subject, Passage = Passage, Allow = false };
        var rule2 = new Rule { Subject = Subject, Passage = Passage, Allow = true };
        var unrelatedRule = new Rule { Subject = unrelatedSubject, Passage = Passage, Allow = true };
        _dbContext.Setup(c => c.Rules).ReturnsDbSet(new[] { rule1, rule2, unrelatedRule });

        // Act
        var rules = _repository.FindRulesRelatedToSubjectAndPassage(Subject, Passage);

        // Assert
        rules.Should().Equal(rule1, rule2);
    }

    [Fact]
    public void FindRulesRelatedToSubjectAndPassage_RulesForSubjectAndArea_ReturnsThem()
    {
        // Arrange
        var rule1 = new Rule { Subject = Subject, Area = Area, Allow = false };
        var rule2 = new Rule { Subject = Subject, Passage = Passage, Allow = true };
        var unrelatedArea = new Area { Name = "Other" };
        var unrelatedRule = new Rule { Subject = Subject, Area = unrelatedArea, Allow = true };
        _dbContext.Setup(c => c.Rules).ReturnsDbSet(new[] { rule1, rule2, unrelatedRule });

        // Act
        var rules = _repository.FindRulesRelatedToSubjectAndPassage(Subject, Passage);

        // Assert
        rules.Should().Equal(rule1, rule2);
    }

    [Fact]
    public void FindRulesRelatedToSubjectAndPassage_RulesForGroupAndPassage_ReturnsThem()
    {
        // Arrange
        var rule1 = new Rule { Group = Group1, Passage = Passage, Allow = false };
        var rule2 = new Rule { Group = Group2, Passage = Passage, Allow = false };
        var rule3 = new Rule { Subject = Subject, Passage = Passage, Allow = true };
        var unrelatedSubject = new Subject
            { FirstName = "Random", LastName = "Dude", DateOfBirth = new DateOnly(2002, 4, 25) };
        var unrelatedRule = new Rule { Subject = unrelatedSubject, Passage = Passage, Allow = true };
        _dbContext.Setup(c => c.Rules).ReturnsDbSet(new[] { rule1, rule2, rule3, unrelatedRule });

        // Act
        var rules = _repository.FindRulesRelatedToSubjectAndPassage(Subject, Passage);

        // Assert
        rules.Should().Equal(rule1, rule2, rule3);
    }
}