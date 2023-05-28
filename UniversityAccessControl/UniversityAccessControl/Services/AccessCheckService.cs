using Microsoft.EntityFrameworkCore;
using UniversityAccessControl.Models;

namespace UniversityAccessControl.Services;

public sealed class AccessCheckService
{
    private readonly IRuleRepository _ruleRepository;

    public AccessCheckService(IRuleRepository ruleRepository)
    {
        _ruleRepository = ruleRepository;
    }

    /// <summary>
    /// Returns <c>true</c> or <c>false</c> depending on whether a given subject is allowed to pass through a given
    /// passage, decided by the existing rules.
    /// </summary>
    public async Task<bool> CanPassAsync(Subject subject, Passage passage)
    {
        var rules = await _ruleRepository.FindRulesRelatedToSubjectAndPassage(subject, passage).ToArrayAsync();

        var attemptResults = new[]
        {
            TryAgainstRulesWhere(rules, r => r.Subject != null && r.Passage != null),
            TryAgainstRulesWhere(rules, r => r.Subject != null && r.Area != null),
            TryAgainstRulesWhere(rules, r => r.Group != null && r.Passage != null),
            TryAgainstRulesWhere(rules, r => r.Group != null && r.Area != null)
        };

        return attemptResults.SkipWhile(r => !r.HasValue).FirstOrDefault() ?? false;
    }

    private static bool? TryAgainstRulesWhere(IEnumerable<Rule> rules, Func<Rule, bool> predicate)
    {
        var rule = rules.Where(predicate).MinBy(r => r.Allow);
        return rule?.Allow;
    }
}