namespace UniversityAccessControl.Dto;

public class PassageResponse
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public AreaDto? Area { get; set; }
}