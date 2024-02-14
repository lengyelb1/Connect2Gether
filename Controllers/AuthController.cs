using Connect2Gether_API.Models.Dtos;
using Connect2Gether_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

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

                    
                    
                    

                    string passwordHash = BCrypt.Net.BCrypt.HashPassword(registrationRequestDto.Password);
                    User user = new User();
                    user.Username = registrationRequestDto.UserName;
                    user.Hash = passwordHash;
                    user.Email = registrationRequestDto.Email;
                    user.RegistrationDate = DateTime.Today;

                    if (context.Permissions.FirstOrDefault((x) => x.Id == defaultPermission.Id && x.Name == defaultPermission.Name) != null)
                    {
                        user.PermissionId = context.Permissions.FirstOrDefault((x) => x.Id == defaultPermission.Id && x.Name == defaultPermission.Name).Id;
                    }
                    else
                    {
                        user.PermissionId = defaultPermission.Id;
                    }

                    // Duplicated user hiba van benne, vizsgálni kell hogy létezik e már a user
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
            catch (Exception e)
            {
                return BadRequest(e.Message);   
            }
            
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.Username),
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
