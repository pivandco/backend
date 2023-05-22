namespace UniversityAccessControl.Dto;

public class AccessLogEntryRequest
{
    public required DateTimeOffset AccessedAt { get; set; }
    public required int SubjectId { get; set; }
    public required int PassageId { get; set; }
}