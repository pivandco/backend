using Microsoft.EntityFrameworkCore;
using UniversityAccessControl.Models;

namespace UniversityAccessControl.Dto;

public sealed class RuleRequest
{
    public int Id { get; set; }
    public int? SubjectId { get; set; }
    public int? GroupId { get; set; }
    public int? AreaId { get; set; }
    public int? PassageId { get; set; }
    public bool Allow { get; set; }

    public async Task<Rule> ToModelAsync(IQueryable<Subject> subjects, IQueryable<Group> groups,
        IQueryable<Passage> passages, IQueryable<Area> areas) => new()
    {
        Id = Id,
        Allow = Allow,
        Subject = SubjectId != null ? await subjects.FirstAsync(s => s.Id == SubjectId) : null,
        Group = GroupId != null ? await groups.FirstAsync(g => g.Id == GroupId) : null,
        Area = AreaId != null ? await areas.FirstAsync(a => a.Id == AreaId) : null,
        Passage = PassageId != null ? await passages.FirstAsync(p => p.Id == PassageId) : null
    };
}