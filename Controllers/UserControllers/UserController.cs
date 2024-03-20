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
        [HttpGet("KeresoWithNevOrCim")]
        public IActionResult SearchWithNameOrTitle(string keresettErtek)
        {
            // Error 4001 nincs @ de van @
            using (var context = new Connect2getherContext())
            {
                try
                {
                    string[] strings = keresettErtek.Split(' ');
                    bool vankuk = false;
                    bool nincskuk = false;
                    string usernev = "";
                    string cim = "";
                    foreach (string s in strings)
                    {
                        if (s.StartsWith("@"))
                        {
                            vankuk = true;
                            usernev = s.Trim('@');
                        }
                        else
                        {
                            nincskuk = true;
                            cim += " " + s;

                        }
                    }
                    if (vankuk == true && nincskuk == false)
                    {
                        return Ok(context.Users.Where(x => x.Username.Contains(keresettErtek.TrimStart('@'))).ToList());
                    }
                    else if (vankuk == false && nincskuk == true)
                    {
                        return Ok(context.UserPosts.Include(x => x.User).Include(x => x.User!.Permission).Where(x => x.Title.Contains(keresettErtek)).ToList());
                    }
                    else if (vankuk == true && nincskuk == true)
                    {
                        return Ok(context.UserPosts.Include(x => x.User).Include(x => x.User!.Permission).Where(x => x.Title.ToLower().Contains(cim.ToLower().TrimStart())).ToList());
                    }
                    else
                    {
                        return BadRequest("Something went wrong! #4001");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

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

        [HttpGet("UserGetPosts")]
        public IActionResult UserGetPosts(int id)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var request = context.UserPosts.Include(x => x.Comments).Where(x => x.User!.Id == id).ToList();
                    return Ok(request);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}
