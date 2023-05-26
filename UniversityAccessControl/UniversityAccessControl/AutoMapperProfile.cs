using AutoMapper;
using UniversityAccessControl.Dto;
using UniversityAccessControl.Models;

namespace UniversityAccessControl;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<AccessLogEntry, AccessLogEntryResponse>();
        CreateMap<AccessLogEntryRequest, AccessLogEntry>();
        CreateMap<Area, AreaDto>().ReverseMap();
        CreateMap<Group, GroupDto>().ReverseMap();
        CreateMap<Passage, PassageResponse>();
        CreateMap<PassageRequest, Passage>();
        CreateMap<Rule, RuleResponse>();
        CreateMap<RuleRequest, Rule>();
        CreateMap<Subject, SubjectResponse>();
        CreateMap<SubjectRequest, Subject>().ForMember(r => r.Groups,
            options => options.MapFrom(r => r.GroupIds.Select(id => new Group { Id = id })));
    }
}