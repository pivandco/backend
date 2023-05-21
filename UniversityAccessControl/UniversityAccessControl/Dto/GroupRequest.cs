using UniversityAccessControl.Models;

namespace UniversityAccessControl.Dto;

public class GroupRequest
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public Group ToModel()
    {
        return new Group { Id = Id, Name = Name };
    }
}