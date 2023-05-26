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
        CreateMap<AreaPostRequest, Area>();
        CreateMap<Group, GroupDto>().ReverseMap();
        CreateMap<GroupPostRequest, Group>();
        CreateMap<Passage, PassageResponse>();
        CreateMap<PassagePutRequest, Passage>();
        CreateMap<PassagePostRequest, Passage>();
        CreateMap<Rule, RuleResponse>();
        CreateMap<RulePostRequest, Rule>();
        CreateMap<RulePutRequest, Rule>();
        CreateMap<Subject, SubjectResponse>();
        CreateMap<SubjectPostRequest, Subject>().ForMember(r => r.Groups, GroupIdsToGroups);
        CreateMap<SubjectPutRequest, Subject>().ForMember(r => r.Groups, GroupIdsToGroups);
    }

    private static void GroupIdsToGroups<T>(
        IProjectionMemberConfiguration<T, Subject, List<Group>> options) where T : SubjectPostRequest =>
        options.MapFrom(r => r.GroupIds.Select(id => new Group { Id = id }));
}