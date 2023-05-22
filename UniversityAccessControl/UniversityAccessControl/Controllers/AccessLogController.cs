using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityAccessControl.Dto;
using UniversityAccessControl.Models;

namespace UniversityAccessControl.Controllers;

[ApiController]
[Route("[controller]")]
public class AccessLogController : ControllerBase
{
    private readonly AccessControlDbContext _db;
    private readonly IMapper _mapper;

    public AccessLogController(AccessControlDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    [HttpGet]
    [Authorize]
    public IEnumerable<AccessLogEntryResponse> Get() => _mapper.ProjectTo<AccessLogEntryResponse>(_db.AccessLogEntries);

    [HttpPost]
    [Authorize]
    public async Task Post([FromBody] AccessLogEntryRequest entry)
    {
        _db.AccessLogEntries.Add(_mapper.Map<AccessLogEntry>(entry));
        await _db.SaveChangesAsync();
    }
}