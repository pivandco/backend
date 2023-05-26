namespace UniversityAccessControl.Dto;

public class AccessLogEntryResponse
{
    public required DateTimeOffset AccessedAt { get; set; }
    public required SubjectResponse Subject { get; set; }
    public required PassageResponse Passage { get; set; }
}