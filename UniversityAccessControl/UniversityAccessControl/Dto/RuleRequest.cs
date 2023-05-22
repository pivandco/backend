namespace UniversityAccessControl.Dto;

public sealed class RuleRequest
{
    public int Id { get; set; }
    public int? SubjectId { get; set; }
    public int? GroupId { get; set; }
    public int? AreaId { get; set; }
    public int? PassageId { get; set; }
    public bool Allow { get; set; }
}