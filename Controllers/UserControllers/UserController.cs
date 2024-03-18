using Connect2Gether_API.Models;
using Connect2Gether_API.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Connect2Gether_API.Controllers.UserControllers
{

    /* Oldal funkciók */
    /* Jelszó / azonosítóval való lekérés*/
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "Default")]
    public class UserController : ControllerBase
    {
        [HttpGet("nev")]
        public IActionResult GetNev(string nev)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    return Ok(context.Users.Where(x => x.Username.Contains(nev)).ToList());
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpGet("postnev")]
        public IActionResult GetPostNev(string nev)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    return Ok(context.UserPosts.Where(x => x.Title.Contains(nev)).ToList());
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}
