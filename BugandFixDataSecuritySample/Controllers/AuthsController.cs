using BugandFixDataSecuritySample.Abstraction;
using BugandFixDataSecuritySample.Models;
using Microsoft.AspNetCore.Mvc;

namespace BugandFixDataSecuritySample.Controllers;


[ApiController]
[Route("api/[controller]/[action]")]
public class AuthsController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthsController(IUserService userService)
    {
        _userService = userService;
    }


    // GET: api/auths/{id}
    [HttpGet]
    public IActionResult GetUserInfo(string userid)
    {
        var user = _userService.GetUser(userid);
        if (user == null)
            return NotFound();

        return Ok(user);
    }

    // POST: api/books
    [HttpPost]
    public IActionResult SaveUserInfo([FromBody] User user)
    {
        var opStat = _userService.SaveUser(user);

        return Ok(opStat);
    }
}
