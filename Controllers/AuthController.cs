using Connect2Gether_API.Models.Dtos;
using Connect2Gether_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Data;

namespace Connect2Gether_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration) { 
            _configuration = configuration;
        }

        [HttpPost("register")]

        public ActionResult<User> Register(RegistrationRequestDto registrationRequestDto)
        {
            //try
            //{
                using (var context = new Connect2getherContext())
                {
                    Permission defaultPermission = new Permission();
                    defaultPermission.Id = 1;
                    defaultPermission.Name = "Default";

                    if (registrationRequestDto.Password.Length <8)
                    {
                        return BadRequest("The password need to be 8 character lenght!");
                    }else if (!(registrationRequestDto.Password.Any(char.IsUpper) && registrationRequestDto.Password.Any(char.IsLower)))
                    {
                        return BadRequest("The password need to contain upper and lower character!");
                    }else if (!registrationRequestDto.Password.Any(char.IsDigit))
                    {
                        return BadRequest("The password need to contain number!");
                    }

                    string passwordHash = BCrypt.Net.BCrypt.HashPassword(registrationRequestDto.Password,3);
                    User user = new User();
                    user.Username = registrationRequestDto.UserName;
                    user.Hash = passwordHash;
                    user.Email = registrationRequestDto.Email;
                    user.RegistrationDate = DateTime.Today;

                    if (context.Permissions.FirstOrDefault((x) => x.Id == defaultPermission.Id && x.Name == defaultPermission.Name) != null)
                    {
                        user.PermissionId = context.Permissions.FirstOrDefault((x) => x.Id == defaultPermission.Id && x.Name == defaultPermission.Name).Id;
                        user.Permission = context.Permissions.FirstOrDefault((x) => x.Id == defaultPermission.Id && x.Name == defaultPermission.Name);
                    }
                    else
                    {
                        user.PermissionId = defaultPermission.Id;
                    }

                    if (context.Users.FirstOrDefault((x)=> x.Username == user.Username) != null)
                    {
                        return BadRequest("User existing!");
                    }

                    context.Users.Add(user);
                    context.SaveChanges();
                    return Ok("User added!");
                }
            /*}
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            */
        }
        [HttpPost("login")]

        public ActionResult<User> Login(LoginRequestDto loginRequestDto)
        {
            try
            {
                using (var context = new Connect2getherContext())
                {
                    User user = context.Users.FirstOrDefault((x) => x.Username == loginRequestDto.UserName);

                    if (user == null)
                    {
                        return BadRequest("User not found!");
                    }

                    if (!BCrypt.Net.BCrypt.Verify(loginRequestDto.Password, user.Hash))
                    {
                        return BadRequest("Wrong password!");
                    }

                    string token = CreateToken(user);

                    return Ok(token);
                }
                

            }
            catch (Exception)
            {
                return BadRequest("Wrong username or password!");   
            }
            
        }

        private string CreateToken(User user)
        {
            using (var context = new Connect2getherContext())
            {
                string permission = context.Permissions.FirstOrDefault((x)=> x.Id == context.Users.FirstOrDefault((x) => x.Username == user.Username).PermissionId).Name;

                List<Claim> claims = new List<Claim>()
                {
                    new Claim("Name",user.Username),
                    new Claim("Permission",permission)
                };


                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));

                var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);

                var token = new JwtSecurityToken(
                    claims:claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                    );

                var jwt = new JwtSecurityTokenHandler().WriteToken(token);

                return jwt;
            }

        }
    }
}
