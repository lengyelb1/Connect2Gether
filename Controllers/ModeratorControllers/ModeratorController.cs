using Connect2Gether_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Connect2Gether_API.Controllers.ModeratorControllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "Moderator")]
    public class ModeratorController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var request = context.UserSuspicious.ToList();
                    return Ok(request);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id) 
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var request = context.UserSuspicious.FirstOrDefault(x => x.Id == id);
                    return Ok(request);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPost("{suspicious}")]
        public IActionResult Post(int id)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var user = context.Users.FirstOrDefault(x => x.Id == id);
                    if (user == null)
                    {
                        return BadRequest("Nincs ilyen user!");
                    }
                    else
                    {
                        context.UserSuspicious.Add(new UserSuspiciou { UserId = user.Id });
                        context.SaveChanges();
                        return Ok("Sikeres hozzáadás!");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}
