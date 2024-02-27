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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var result = await context.Users.Include(f => f.UserPosts).Include(f => f.Comments).Include(f => f.Alertmessages).ToListAsync();
                    context.SaveChanges();
                    return Ok(result);
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
                if (userPutDto.Password.Length < 8)
                {
                    return BadRequest("The password need to be 8 character lenght!");
                }
                else if (!(userPutDto.Password.Any(char.IsUpper) && userPutDto.Password.Any(char.IsLower)))
                {
                    return BadRequest("The password need to contain upper and lower character!");
                }
                else if (!userPutDto.Password.Any(char.IsDigit))
                {
                    return BadRequest("The password need to contain number!");
                }
                else if (!userPutDto.Password.Any(char.IsSymbol))
                {
                    return BadRequest("The password need to contain special character!");
                }

                string passwordHash = BCrypt.Net.BCrypt.HashPassword(userPutDto.Password, 4);
                User user = new User();
                user.Id = id;
                user.Username = userPutDto.UserName;
                user.Hash = passwordHash;
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
    }
}
