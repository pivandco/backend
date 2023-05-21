using UniversityAccessControl.Models;

namespace UniversityAccessControl;

public interface IRuleRepository
{
    IEnumerable<Rule> FindRulesRelatedToSubjectAndPassage(Subject subject, Passage passage);
}