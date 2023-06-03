using UniversityAccessControl.Models;
using UniversityAccessControl.Services;

namespace UniversityAccessControl.Tests;

public sealed class AccessCheckServiceTest
{
    private static readonly Group Group = new() { Name = "Group" };
    private static readonly Subject Subject = new()
        { FirstName = "John", LastName = "Doe", DateOfBirth = new DateOnly(2002, 4, 25) };

    private static readonly Area Area = new() { Name = "Area" };
    private static readonly Passage Passage = new() { Name = "Passage", Area = Area };

    [Fact]
    public async void CanPass_NoMatchingRules_Denies()
    {
        // Arrange
        var ruleRepository = Mock.Of<IRuleRepository>(r =>
            r.FindRulesRelatedToSubjectAndPassage(Subject, Passage) == Array.Empty<Rule>().AsQueryable());
        var service = new AccessCheckService(ruleRepository);

        // Act & Assert
        (await service.CanPassAsync(Subject, Passage)).Should().BeFalse();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async void CanPass_RuleForSubjectAndPassage_ReturnsRulesValue(bool allow)
    {
        // Arrange
        var rule = new Rule { Subject = Subject, Passage = Passage, Allow = allow };
        var ruleRepository = GetRuleRepositoryWithRules(rule);
        var service = new AccessCheckService(ruleRepository);

        // Act & Assert
        (await service.CanPassAsync(Subject, Passage)).Should().Be(allow);
    }

    [Fact]
    public async void CanPass_TwoRulesForSameSubjectAndPassageButDifferentAllowValues_PrioritizesDenyRules()
    {
        // Arrange
        var denyRule = new Rule { Subject = Subject, Passage = Passage, Allow = false };
        var allowRule = new Rule { Subject = Subject, Passage = Passage, Allow = true };
        var ruleRepository = GetRuleRepositoryWithRules(allowRule, denyRule);
        var service = new AccessCheckService(ruleRepository);

        // Act & Assert
        (await service.CanPassAsync(Subject, Passage)).Should().BeFalse();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async void CanPass_RuleForSubjectAndArea_ReturnsRulesValue(bool allow)
    {
        // Arrange
        var rule = new Rule { Subject = Subject, Area = Area, Allow = allow };
        var ruleRepository = GetRuleRepositoryWithRules(rule);
        var service = new AccessCheckService(ruleRepository);

        // Act & Assert
        (await service.CanPassAsync(Subject, Passage)).Should().Be(allow);
    }
    
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async void CanPass_RuleForGroupAndPassage_ReturnsRulesValue(bool allow)
    {
        // Arrange
        var rule = new Rule { Group = Group, Passage = Passage, Allow = allow };
        var ruleRepository = GetRuleRepositoryWithRules(rule);
        var service = new AccessCheckService(ruleRepository);

        // Act & Assert
        (await service.CanPassAsync(Subject, Passage)).Should().Be(allow);
    }
    
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async void CanPass_RuleForGroupAndArea_ReturnsRulesValue(bool allow)
    {
        // Arrange
        var rule = new Rule { Group = Group, Area = Area, Allow = allow };
        var ruleRepository = GetRuleRepositoryWithRules(rule);
        var service = new AccessCheckService(ruleRepository);

        // Act & Assert
        (await service.CanPassAsync(Subject, Passage)).Should().Be(allow);
    }
    
    [Fact]
    public async void CanPass_CascadingRules_FavorsConcreteRules()
    {
        // Arrange
        var broadRule = new Rule { Group = Group, Area = Area, Allow = true };
        var fineRule = new Rule { Subject = Subject, Passage = Passage, Allow = false };
        var ruleRepository = GetRuleRepositoryWithRules(broadRule, fineRule);
        var service = new AccessCheckService(ruleRepository);

        // Act & Assert
        (await service.CanPassAsync(Subject, Passage)).Should().BeFalse();
    }
    
    private static IRuleRepository GetRuleRepositoryWithRules(params Rule[] rules) =>
        Mock.Of<IRuleRepository>(r =>
            r.FindRulesRelatedToSubjectAndPassage(Subject, Passage) == rules.AsQueryable());
}