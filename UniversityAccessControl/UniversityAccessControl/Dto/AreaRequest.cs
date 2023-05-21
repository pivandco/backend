using UniversityAccessControl.Models;

namespace UniversityAccessControl.Dto;

public class AreaRequest
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public Area ToModel()
    {
        return new Area { Id = Id, Name = Name };
    }
}