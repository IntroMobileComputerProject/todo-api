using Microsoft.AspNetCore.Mvc;
using to_do_api.Models;
using Microsoft.AspNetCore.Authorization;
using to_do_api.DTOs;
using System.Security.Claims;
namespace to_do_api.Controllers;

[ApiController]
[Route("[controller]")]
public class ActivityController : ControllerBase
{

    private readonly ILogger<ActivityController> _logger;

    public ActivityController(ILogger<ActivityController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    [Authorize(Roles = "user")]
    public IActionResult Post([FromBody] DTOs.ActivitiesDTO data)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);

        var db = new ToDoDbContext();
        var activity = new Models.Activities();
        activity.ActivityName = data.ActivityName;
        activity.ActivitiesTime = data.ActivitiesTime;
        activity.UserId = userId;
        db.Activities.Add(activity);
        db.SaveChanges();
        return Ok(activity);
    }
    [HttpGet]
    [Authorize(Roles = "user")]
    public IActionResult Get()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);

        var db = new ToDoDbContext();
        var activities = db.Activities.Where(a => a.UserId == userId).ToList();
        return Ok(activities);
    }
    [HttpGet("{id}")]
    [Authorize(Roles = "user")]
    public IActionResult Get(int id)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
        var db = new ToDoDbContext();
        var activities = db.Activities.Where(a => a.UserId == userId).ToList();
        foreach (var activity in activities)
        {
            if (activity.ActivityId == id)
            {
                return Ok(activity);
            }
        }
        return NotFound();
    }
    [HttpPut("{id}")]
    [Authorize(Roles = "user")]
    public IActionResult Put(int id, [FromBody] DTOs.ActivitiesDTO data)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
        var db = new ToDoDbContext();
        var activities = db.Activities.Where(a => a.UserId == userId).ToList();
        foreach (var activity in activities)
        {
            if (activity.ActivityId == id)
            {
                activity.ActivityName = data.ActivityName;
                activity.ActivitiesTime = data.ActivitiesTime;
                db.SaveChanges();
                return Ok(activity);
            }
        }
        return NotFound();
    }
    [HttpDelete("{id}")]
    [Authorize(Roles = "user")]
    public IActionResult Delete(int id)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
        var db = new ToDoDbContext();
        var activities = db.Activities.Where(a => a.UserId == userId).ToList();
        foreach (var activity in activities)
        {
            if (activity.ActivityId == id)
            {
                db.Activities.Remove(activity);
                db.SaveChanges();
                return Ok();
            }
        }
        return NotFound();
    }
}
