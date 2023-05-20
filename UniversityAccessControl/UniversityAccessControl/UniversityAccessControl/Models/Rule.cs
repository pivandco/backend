using System.ComponentModel.DataAnnotations;

namespace UniversityAccessControl.Models;

public sealed class Rule : IValidatableObject
{
    public int Id { get; set; }
    public Subject? Subject { get; set; }
    public Group? Group { get; set; }
    public Area? Area { get; set; }
    public Passage? Passage { get; set; }
    public RuleType Type { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Area != null && Passage != null)
        {
            yield return new ValidationResult("Passage and Area fields are mutually exclusive.",
                new[] { nameof(Area), nameof(Passage) });
        }

        if (Area == null && Passage == null)
        {
            yield return new ValidationResult("Neither Passage nor Area are set.",
                new[] { nameof(Area), nameof(Passage) });
        }

        if (Subject != null && Group != null)
        {
            yield return new ValidationResult("Subject and Group fields are mutually exclusive.",
                new[] { nameof(Subject), nameof(Group) });
        }

        if (Subject == null && Group == null)
        {
            yield return new ValidationResult("Neither Subject nor Group are set.",
                new[] { nameof(Subject), nameof(Group) });
        }
    }
}