using UniversityAccessControl.Models;

namespace UniversityAccessControl;

public interface IRuleRepository
{
    IQueryable<Rule> FindRulesRelatedToSubjectAndPassage(Subject subject, Passage passage);
}