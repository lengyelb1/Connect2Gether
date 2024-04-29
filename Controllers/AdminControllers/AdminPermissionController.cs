using Connect2Gether_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Connect2Gether_API.Controllers.AdminControllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminPermissionController : ControllerBase
    {
        [HttpGet("AllPermission")]
        public IActionResult AllPermission()
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var permissions = context.Permissions.ToList();
                    return Ok(permissions);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}
