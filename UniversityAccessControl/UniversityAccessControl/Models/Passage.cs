namespace UniversityAccessControl.Models;

public sealed class Passage
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public Area? Area { get; set; }
}