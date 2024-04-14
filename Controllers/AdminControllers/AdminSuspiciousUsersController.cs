using Connect2Gether_API.Models;
using Connect2Gether_API.Models.Dtos.UserPostDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;

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
                    var user = context.UserSuspicious.Include(x => x.User).ToList();
                    var simplifiedUser = user.Select(userSuspicious => new
                    {
                        userSuspicious.Id,
                        userSuspicious.UserId,
                        User = userSuspicious.User != null ? new { userSuspicious.User.Username } : null,

                    }).ToList();
                    return Ok(simplifiedUser);
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
                    List<UserSuspiciou> userSuspicious = new List<UserSuspiciou>();
                    var request = context.UserSuspicious.Include(x => x.User).FirstOrDefault(x => x.Id == id);
                    userSuspicious.Add(request!);
                    var simplifiedUser = userSuspicious.Select(userSuspicious => new
                    {
                        userSuspicious.Id,
                        userSuspicious.UserId,
                        User = userSuspicious.User != null ? new { userSuspicious.User.Username } : null,

                    }).ToList();
                    return Ok(simplifiedUser);
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
