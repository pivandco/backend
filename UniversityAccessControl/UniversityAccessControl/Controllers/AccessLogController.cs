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

    public AccessLogController(AccessControlDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    [Authorize]
    public IEnumerable<AccessLogEntry> Get()
    {
        return _db.AccessLogEntries;
    }

    [HttpPost]
    [Authorize]
    public async Task Post([FromBody] AccessLogEntryRequest entry)
    {
        _db.AccessLogEntries.Add(await entry.ToModelAsync(_db.Passages, _db.Subjects));
        await _db.SaveChangesAsync();
    }
}