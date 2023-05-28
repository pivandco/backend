using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public async Task<ActionResult<bool>> CanSubjectAccessPassage([Required] int subjectId, [Required] int passageId)
    {
        var errors = new List<string>();

        var subject = await _db.Subjects.FindAsync(subjectId);
        if (subject == null) errors.Add("Subject not found");

        var passage = await _db.Passages.FindAsync(passageId);
        if (passage == null) errors.Add("Passage not found");

        if (errors.Any())
            return new NotFoundObjectResult(new { errors });

        return await _accessCheckService.CanPassAsync(subject!, passage!);
    }
}