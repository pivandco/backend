namespace UniversityAccessControl.Models;

public class AccessLogEntry
{
    public DateTimeOffset AccessedAt { get; set; }
    public Subject Subject { get; set; }
    public Passage Passage { get; set; }
}