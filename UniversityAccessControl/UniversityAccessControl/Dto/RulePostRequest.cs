namespace UniversityAccessControl.Dto;

public class RulePostRequest
{
    public int? SubjectId { get; set; }
    public int? GroupId { get; set; }
    public int? AreaId { get; set; }
    public int? PassageId { get; set; }
    public bool Allow { get; set; }
}