using Connect2Gether_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Connect2Gether_API.Controllers.AdminControllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminSuspiciousUsersController : ControllerBase
    {
        [HttpGet("AllSuspiciousUser")]
        public IActionResult AllSuspicious()
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var user = context.UserSuspicious.Include(x => x.User).Include(x => x.User.Permission).ToList();
                    return Ok(user);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpGet("SuspiciousById")]
        public IActionResult SuspiciousById(int id)
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

        [HttpPost("AddSuspicious")]
        public IActionResult AddSuspicious(int id)
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

        [HttpDelete("DeleteSuspiciousById")]
        public ActionResult DeleteSuspiciousById(int id)
        {
            using (var context = new Connect2getherContext())
            {
                var suspiciousUser = context.UserSuspicious.FirstOrDefault(x => x.Id == id);
                context.UserSuspicious.Remove(suspiciousUser!);
                context.SaveChanges();
                return Ok($"User deleted!");
            }
        }
    }
}
