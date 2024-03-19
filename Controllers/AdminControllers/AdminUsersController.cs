﻿using Connect2Gether_API.Models;
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

        [HttpGet("userdb")]
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

        [HttpGet("userpostdb")]
        public IActionResult UserPostDB()
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var db = context.UserPosts.ToList().Count();
                    return Ok(db);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpGet]
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

        [HttpGet("GetAllSuspicious")]
        public IActionResult GetAllSuspicious()
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

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
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

        [HttpGet("SuspiciousId")]
        public IActionResult GetByIdSuspicious(int id)
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

        [HttpPost("suspicious")]
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

        [HttpGet("nev")]
        public IActionResult GetNev(string nev)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    if (nev.StartsWith("@"))
                    {
                        return Ok(context.Users.Where(x => x.Username.Contains(nev)).ToList());
                    }
                    else
                    {
                        return Ok(context.UserPosts.Include(x => x.User).Include(x => x.User!.Permission).Where(x => x.Title.Contains(nev)).ToList());
                    }
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
                    return Ok(context.UserPosts.Include(x => x.User).Include(x => x.User!.Permission).Where(x => x.Title.Contains(nev)).ToList());
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPost("register")]

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

        [HttpPut("id")]

        public ActionResult<User> RegisterPut(UserPutDto userPutDto, int id)
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

        [HttpDelete("id")]
        public ActionResult Delete(int id) 
        {
            using (var context = new Connect2getherContext())
            {
                User user = new User { Id = id };
                context.Users.Remove(user);
                context.SaveChanges();
                return Ok($"User deleted!");
            }
        }

        [HttpDelete("SuspiciousId")]
        public ActionResult DeleteSuspicious(int id)
        {
            using (var context = new Connect2getherContext())
            {
                UserSuspiciou userSuspiciou = new UserSuspiciou { Id = id };
                context.UserSuspicious.Remove(userSuspiciou);
                context.SaveChanges();
                return Ok($"User deleted!");
            }
        }
    }
}
