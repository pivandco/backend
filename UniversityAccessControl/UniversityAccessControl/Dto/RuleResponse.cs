namespace UniversityAccessControl.Dto;

public class RuleResponse
{
    public int Id { get; set; }
    public SubjectResponse? Subject { get; set; }
    public GroupDto? Group { get; set; }
    public AreaDto? Area { get; set; }
    public PassageResponse? Passage { get; set; }
    public bool Allow { get; set; }
}