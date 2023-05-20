namespace UniversityAccessControl.Models;

public sealed class Area
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public List<Passage> Passages { get; set; } = new();
}