namespace UniversityAccessControl.Models;

public sealed class Rule
{
    public int Id { get; set; }
    public Subject? Subject { get; set; }
    public Group? Group { get; set; }
    public Area? Area { get; set; }
    public Passage? Passage { get; set; }
    public RuleType Type { get; set; }
}