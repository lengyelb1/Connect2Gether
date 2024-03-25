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
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private readonly int expire_day = 1;

        public AuthController(IConfiguration configuration) { 
            _configuration = configuration;
        }

        [HttpPost("Register")]

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
                    }else if (!registrationRequestDto.Password.Any(char.IsSymbol))
                    {
                        return BadRequest("The password need to contain special character!");
                    }

                string passwordHash = BCrypt.Net.BCrypt.HashPassword(registrationRequestDto.Password,4);
                    User user = new User();
                    user.Username = registrationRequestDto.UserName;
                    user.Hash = passwordHash;
                    user.Email = registrationRequestDto.Email;
                    user.RegistrationDate = DateTime.Today;
                    user.PermissionId = defaultPermission.Id;
                    user.Permission = context.Permissions.FirstOrDefault((x) => x.Id == defaultPermission.Id && x.Name == defaultPermission.Name);

                    /*
                    if (context.Permissions.FirstOrDefault((x) => x.Id == defaultPermission.Id && x.Name == defaultPermission.Name) != null)
                    {
                        //user.PermissionId = context.Permissions.FirstOrDefault((x) => x.Id == defaultPermission.Id && x.Name == defaultPermission.Name).Id;
                        //user.Permission = context.Permissions.FirstOrDefault((x) => x.Id == defaultPermission.Id && x.Name == defaultPermission.Name);
                    }
                    else
                    {
                    }
                    */

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
        [HttpPost("Login")]

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
                        return BadRequest("Wrong password or Username!");
                    }

                    context.Users.FirstOrDefault((x) => x.Username == loginRequestDto.UserName).LastLogin = DateTime.Now;
                    string token = CreateToken(user);
                    context.UserTokens.Add(new UserToken { UserId = user.Id, Token = token, TokenExpireDate = DateTime.Now.AddDays(expire_day) });
                    context.SaveChanges();

                    return Ok(token);
                }
                

            }
            catch (Exception e)
            {
                return BadRequest("Something went wrong!");   
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
                    new Claim("role",permission),
                    new Claim("id", user.Id.ToString())
                };


                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AuthSettings:JwtOptions:Token").Value!));

                var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);

                var token = new JwtSecurityToken(
                    claims:claims,
                    expires: DateTime.Now.AddDays(expire_day),
                    signingCredentials: creds,
                    audience: _configuration.GetSection("AuthSettings:JwtOptions:Audience").Value,
                    issuer: _configuration.GetSection("AuthSettings:JwtOptions:Issuer").Value
                    );

                var jwt = new JwtSecurityTokenHandler().WriteToken(token);

                return jwt;
            }

        }
    }
}
