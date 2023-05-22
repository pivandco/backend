namespace UniversityAccessControl.Dto;

public class PassageRequest
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int? AreaId { get; set; }
}