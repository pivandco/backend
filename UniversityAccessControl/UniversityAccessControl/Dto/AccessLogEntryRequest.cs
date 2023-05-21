using Microsoft.EntityFrameworkCore;
using UniversityAccessControl.Models;

namespace UniversityAccessControl.Dto;

public class AccessLogEntryRequest
{
    public required DateTimeOffset AccessedAt { get; set; }
    public required int SubjectId { get; set; }
    public required int PassageId { get; set; }

    public async Task<AccessLogEntry> ToModelAsync(IQueryable<Passage> passages, IQueryable<Subject> subjects)
    {
        return new AccessLogEntry
        {
            AccessedAt = AccessedAt,
            Passage = await passages.FirstAsync(p => p.Id == PassageId),
            Subject = await subjects.FirstAsync(p => p.Id == SubjectId)
        };
    }
}