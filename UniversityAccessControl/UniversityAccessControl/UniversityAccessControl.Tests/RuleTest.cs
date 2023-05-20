using System.ComponentModel.DataAnnotations;
using UniversityAccessControl.Models;

namespace UniversityAccessControl.Tests;

public class RuleTest
{
    private static readonly Passage Passage = new() { Name = "Passage" };
    private static readonly Area Area = new() { Name = "Area" };

    private static readonly Subject Subject = new()
        { FirstName = "John", LastName = "Doe", DateOfBirth = new DateOnly(2002, 4, 25) };

    private static readonly Group Group = new() { Name = "Group" };

    [Fact]
    public void Validate_RuleWithBothPassageAndAreaSet_ComplainsAboutMutuallyExclusiveFields()
    {
        // Arrange
        var rule = new Rule { Area = Area, Passage = Passage, Subject = Subject };

        // Act
        var errors = rule.Validate(new ValidationContext(rule));

        // Assert
        errors.Should().OnlyContain(error => error.ErrorMessage == "Passage and Area fields are mutually exclusive.");
    }

    [Fact]
    public void Validate_RuleWithBothPassageAndAreaNotSet_ComplainsAboutNeitherOfThemSet()
    {
        // Arrange
        var rule = new Rule { Subject = Subject };

        // Act
        var errors = rule.Validate(new ValidationContext(rule));

        // Assert
        errors.Should().OnlyContain(error => error.ErrorMessage == "Neither Passage nor Area are set.");
    }

    [Fact]
    public void Validate_RuleWithBothSubjectAndGroupSet_ComplainsAboutMutuallyExclusiveFields()
    {
        // Arrange
        var rule = new Rule { Area = Area, Subject = Subject, Group = Group };

        // Act
        var errors = rule.Validate(new ValidationContext(rule));

        // Assert
        errors.Should().OnlyContain(error => error.ErrorMessage == "Subject and Group fields are mutually exclusive.");
    }

    [Fact]
    public void Validate_RuleWithSubjectAndGroupNotSet_ComplainsAboutNeitherOfThemSet()
    {
        // Arrange
        var rule = new Rule { Area = Area };
        
        // Act
        var errors = rule.Validate(new ValidationContext(rule));
        
        // Assert
        errors.Should().OnlyContain(error => error.ErrorMessage == "Neither Subject nor Group are set.");
    }
}