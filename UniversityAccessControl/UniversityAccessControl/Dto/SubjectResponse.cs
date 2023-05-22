namespace UniversityAccessControl.Dto;

public class SubjectResponse
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public required string LastName { get; set; }
    public required DateOnly DateOfBirth { get; set; }
    public List<GroupDto> Groups { get; set; } = new();
}