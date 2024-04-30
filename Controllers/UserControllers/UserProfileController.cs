using Connect2Gether_API.Controllers.Utilities;
using Connect2Gether_API.Models;
using Connect2Gether_API.Models.Dtos;
using Connect2Gether_API.Models.Dtos.UserDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Validations;
using System.Net.Mail;

namespace Connect2Gether_API.Controllers.UserControllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        [HttpGet("UserProfileById")]
        [Authorize(Roles = "Default, Admin, Moderator")]
        public IActionResult UserProfileById(int id)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var user = context.Users.FirstOrDefault(x => x.Id == id);
                    if (user == null) 
                    {
                        return StatusCode(404, "This user does not exist!");
                    }

                    var userPostCount = context.UserPosts.Where(x => x.UserId == user!.Id).ToList().Count;
                    var userCommentCount = context.Comments.Where(x => x.UserId == user!.Id).ToList().Count;
                    var userRank = context.Ranks.FirstOrDefault(x => x.Id == user.RankId);
                    var userProfileDto = new UserProfileDto
                    {
                        UserName = user.Username,
                        Email = user.Email,
                        Points = user.Point,
                        Rank = userRank,
                        RegistrationDate = user.RegistrationDate,
                        LastLogin = user.LastLogin,
                        PostCount = userPostCount,
                        CommentCount = userCommentCount,
                    };
                    return Ok(userProfileDto);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPut("UserChangePassword")]
        [Authorize(Roles = "Default, Admin, Moderator")]
        public async Task<IActionResult>UserChangePassword(int UserId,ChangePasswordDto changedUser)
        {
            try
            {
                using (var context = new Connect2getherContext())
                {
                    var requestUser = await context.Users.FirstOrDefaultAsync(x => x.Id == UserId);
                    var oldHash = BCrypt.Net.BCrypt.Verify(changedUser.OldPassword, requestUser!.Hash);
                    if (!oldHash)
                    {
                        return BadRequest("The old password incorrect!");

                    }
                    if (!changedUser.NewPassword!.Equals(changedUser.NewPasswordAgain))
                    {
                        return BadRequest("The two passwords not match!");
                    
                    }
                    if (PasswordChecker.CheckPassword(changedUser.NewPassword))
                    {
                        var newHashedPassword = BCrypt.Net.BCrypt.HashPassword(changedUser.NewPassword);
                        requestUser.Hash = newHashedPassword;
                        context.Update(requestUser);
                        context.SaveChanges();

                        MailMessage mail = new MailMessage();
                        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
                        mail.From = new MailAddress("connectgether@gmail.com");
                        mail.To.Add(requestUser.Email);
                        mail.Subject = "Jelszó változtatás";
                        mail.Body = "Sikeresen megváltoztattad a jelszavadat!";
                        smtpServer.Credentials = new System.Net.NetworkCredential("connectgether@gmail.com", "sdph etlk bmbw vopl");
                        smtpServer.Port = 587;
                        smtpServer.EnableSsl = true;
                        smtpServer.Send(mail);

                        return Ok("Your password changed!");
                    }
                    else 
                    {
                        return BadRequest("The password does not meet the criteria!");
                    }

                }
                    
            }catch (Exception ex) 
            {
                return BadRequest(ex.Message);
                
            }
        }

        [HttpPost("ForgetPasswordEmailSender")]
        public IActionResult ForgetPasswordEmailSender(string email,string url)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var user = context.Users.FirstOrDefault(x => x.Email == email);
                    if (user == null)
                    {
                        return BadRequest("This email does not exist!");
                    }

                    MailMessage mail = new MailMessage();
                    SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
                    mail.From = new MailAddress("connectgether@gmail.com");
                    mail.To.Add(user.Email!);
                    mail.Subject = "Elfelejtett jelszó változtatása";
                    mail.Body = $"Kedves Felhasználó!\n\nSajnálattal értesültünk, hogy elfelejtette jelszavát!\nAz alábbi linken megváltoztathatod a jelszavadat: {url+user.Id}";
                    smtpServer.Credentials = new System.Net.NetworkCredential("connectgether@gmail.com", "sdph etlk bmbw vopl");
                    smtpServer.Port = 587;
                    smtpServer.EnableSsl = true;
                    smtpServer.Send(mail);

                    return Ok("Email sending successfully!");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPut("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordDto forgetPasswordDto)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var requestUser = await context.Users.FirstOrDefaultAsync(x => x.Id == forgetPasswordDto.userId);
                    if (requestUser == null) 
                    {
                        return BadRequest("This user does not exist!");
                    }
                    if (requestUser.Email == null)
                    {
                        return BadRequest("This email does not exist!");
                    }
                    await Console.Out.WriteLineAsync(forgetPasswordDto.password);
                    if (PasswordChecker.CheckPassword(forgetPasswordDto.password))
                    {
                        var newPassword = BCrypt.Net.BCrypt.HashPassword(forgetPasswordDto.password);
                        requestUser.Hash = newPassword;
                        context.Update(requestUser);
                        context.SaveChanges();

                        return Ok("Your password changed!");
                    }
                    else
                    {
                        return BadRequest("The password does not meet the criteria!");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPut("ChangeUser")]
        [Authorize(Roles = "Default, Admin, Moderator")]
        public IActionResult ChangeUser(UserPutDto userPutDto, int userId)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var user = context.Users.FirstOrDefault(x => x.Id == userId);
                    if (user == null)
                    {
                        return BadRequest("This user does not exist!");
                    }
                    user!.Id = userId;
                    user.Username = userPutDto.UserName;
                    user.Email = userPutDto.Email;

                    if (context.Users.FirstOrDefault((x) => x.Email == user.Email) != null)
                    {
                        return BadRequest("User existing!");
                    }

                    context.Users.Update(user);
                    context.SaveChanges();
                    return Ok("Changes successfully!");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}
