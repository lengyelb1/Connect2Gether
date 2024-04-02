using Connect2Gether_API.Models;
using Connect2Gether_API.Models.Dtos;
using Connect2Gether_API.Models.Dtos.UserDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Connect2Gether_API.Controllers.AdminControllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminUsersController : ControllerBase
    {
        public static User user = new User();

        [HttpGet("UserCount")]
        public IActionResult UserDB()
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var db = context.Users.ToList().Count();
                    return Ok(db);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpGet("AllUser")]
        public async Task<IActionResult> GetAll()
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var result = await context.Users.Include(f => f.UserPosts).Include(f => f.Permission).Include(f => f.Comments).Include(f => f.Alertmessages).ToListAsync();
                    context.SaveChanges();
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpGet("UserById")]
        public IActionResult UserById(int id)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var request = context.Users.Include(x => x.Permission).FirstOrDefault(x => x.Id == id);
                    return Ok(request);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpGet("SearchWithNameOrTitle")]
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

        [HttpGet("SearchName")]
        public IActionResult SearchName(string nev)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    return Ok(context.Users.Where(x => x.Username.Contains(nev)).Include(x => x.Permission).ToList());
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPost("RegisterWithOutCriterion")]
        public ActionResult<User> RegisterWithOutCriterion(RegistrationRequestDto registrationRequestDto)
        {
            using (var context = new Connect2getherContext())
            {
                Permission defaultPermission = new Permission();
                defaultPermission.Id = 1;
                defaultPermission.Name = "Default";
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(registrationRequestDto.Password, 4);
                User user = new User();
                user.Username = registrationRequestDto.UserName;
                user.Hash = passwordHash;
                user.Email = registrationRequestDto.Email;
                user.RegistrationDate = DateTime.Today;
                user.PermissionId = defaultPermission.Id;
                user.Permission = context.Permissions.FirstOrDefault((x) => x.Id == defaultPermission.Id && x.Name == defaultPermission.Name);

                if (context.Users.FirstOrDefault((x) => x.Username == user.Username) != null)
                {
                    return BadRequest("User existing!");
                }

                context.Users.Add(user);
                context.SaveChanges();
                return Ok("User added!");
            }
        }

        [HttpPut("ChangeRegister")]

        public ActionResult<User> ChangeRegister(UserPutDto userPutDto, int id)
        {
            using (var context = new Connect2getherContext())
            {
                User user = new User();
                user.Id = id;
                user.Username = userPutDto.UserName;
                user.Email = userPutDto.Email;
                user.RegistrationDate = DateTime.Today;
                user.PermissionId = userPutDto.PermissionId;
                user.Permission = context.Permissions.FirstOrDefault((x) => x.Id == id);

                if (context.Users.FirstOrDefault((x) => x.Username == user.Username) != null)
                {
                    return BadRequest("User existing!");
                }

                context.Users.Update(user);
                context.SaveChanges();
                return Ok("User changes!");
            }
        }

        [HttpDelete("DeleteUserById")]
        public ActionResult DeleteUserById(int id) 
        {
            using (var context = new Connect2getherContext())
            {
                User user = new User { Id = id };
                context.Users.Remove(user);
                context.SaveChanges();
                return Ok($"User deleted!");
            }
        }
    }
}
