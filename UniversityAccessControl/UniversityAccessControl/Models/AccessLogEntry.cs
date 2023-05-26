namespace UniversityAccessControl.Models;

public class AccessLogEntry
{
    public int Id { get; set; }
    public required DateTimeOffset AccessedAt { get; set; }
    public int SubjectId { get; set; }
    public required Subject Subject { get; set; }
    public int PassageId { get; set; }
    public required Passage Passage { get; set; }
}