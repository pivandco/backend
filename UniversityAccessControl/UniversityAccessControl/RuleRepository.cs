using UniversityAccessControl.Models;

namespace UniversityAccessControl;

public sealed class RuleRepository : IRuleRepository
{
    private readonly AccessControlDbContext _dbContext;

    public RuleRepository(AccessControlDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<Rule> FindRulesRelatedToSubjectAndPassage(Subject subject, Passage passage)
    {
        var area = passage.Area;
        var groups = subject.Groups;
        return _dbContext.Rules.Where(r =>
            (r.Subject == subject || groups.Contains(r.Group!)) && (r.Passage == passage || r.Area == area));
    }
}