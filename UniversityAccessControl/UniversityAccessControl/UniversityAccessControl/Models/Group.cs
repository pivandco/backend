namespace UniversityAccessControl.Models;

public sealed class Group
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public List<Subject> Subjects { get; set; } = new();
}