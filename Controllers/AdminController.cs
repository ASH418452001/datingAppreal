using datingAppreal.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;

namespace datingAppreal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public AdminController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        [Authorize(Policy ="RequiredAdminRole")]
        [HttpGet("user-with-roles")]
    
     public async  Task<ActionResult>  GetUserWithRoles()
        {

            var users = await _userManager.Users
                .OrderBy(u => u.UserName)
                .Select(u => new
                {
                    u.Id,
                    Username = u.UserName,
                    Roles = u.UserRoles.Select(r => r.Role.Name).ToList()
                }
                ).ToListAsync();

            return Ok(users);

        }



        [Authorize(Policy = "RequiredAdminRole")]
        [HttpPost("edit-roles/{username}")]

        public async Task<ActionResult> EditRoles(string username, [FromQuery]string roles)
        {
            if (string.IsNullOrEmpty(roles)) return BadRequest("you must select at least one role");

            var selectRoles = roles.Split(".").ToArray();

            var user = await _userManager.FindByNameAsync(username);

            if (user == null) return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);

            var result = await _userManager.AddToRolesAsync(user, selectRoles.Except(userRoles));

            if (!result.Succeeded) return BadRequest("failed to add to roles");
            
            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectRoles));

            if (!result.Succeeded) return BadRequest("failed to remove from roles");

            return Ok(await _userManager.GetRolesAsync(user));

        }


        [Authorize(Policy ="ModeratePhotoRole")]
        [HttpGet("photos-to-moderate")]
     public ActionResult GetPhotosForModration()
        {
            return Ok("Admins and modrations can see this");
        }
    
    
    
    
    }
}
