using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using UniversityAccessControl.Services;

namespace UniversityAccessControl.Controllers;

[ApiController]
[Route("[controller]")]
public class AccessCheckController : ControllerBase
{
    private readonly AccessControlDbContext _db;
    private readonly AccessCheckService _accessCheckService;

    public AccessCheckController(AccessControlDbContext db, AccessCheckService accessCheckService)
    {
        _db = db;
        _accessCheckService = accessCheckService;
    }

    [HttpGet]
    public async Task<ActionResult<bool>> CanSubjectAccessPassage([Required] int subjectId, [Required] int passageId)
    {
        var subject = await _db.Subjects.FindAsync(subjectId);
        if (subject == null)
        {
            return new NotFoundObjectResult("Subject not found");
        }
        
        var passage = await _db.Passages.FindAsync(passageId);
        if (passage == null)
        {
            return new NotFoundObjectResult("Passage not found");
        }

        return _accessCheckService.CanPass(subject, passage);
    }
}