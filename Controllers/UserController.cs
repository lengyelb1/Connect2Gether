using Connect2Gether_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Connect2Gether_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var result = await context.Users.Include(f => f.Alertmessages).Include(f => f.UserPosts).ToListAsync();
                    context.SaveChanges();
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}
