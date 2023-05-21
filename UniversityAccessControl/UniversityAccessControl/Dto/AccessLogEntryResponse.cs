using UniversityAccessControl.Models;

namespace UniversityAccessControl.Dto;

public class AccessLogEntryResponse
{
    public required DateTimeOffset AccessedAt { get; set; }
    public required Subject Subject { get; set; }
    public required Passage Passage { get; set; }
}