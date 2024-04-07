using Connect2Gether_API.Controllers.Utilities;
using Connect2Gether_API.Models;
using Connect2Gether_API.Models.Dtos;
using Connect2Gether_API.Models.Dtos.UserDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Connect2Gether_API.Controllers.UserControllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class UserProfileController : ControllerBase
    {
        [HttpGet("UserProfileById")]
        public IActionResult UserProfile(int id)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var user = context.Users.FirstOrDefault(x => x.Id == id);
                    if (user == null) 
                    {
                        return StatusCode(404, "Nincs ilyen user");
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
        [HttpPut("UserId")]
        public async Task<IActionResult>UserChangePassword(int UserId,ChangePasswordDto changedUser)
        {
            try
            {
                using (var context = new Connect2getherContext())
                {
                    var requestUser = await context.Users.FirstOrDefaultAsync(x => x.Id == UserId);
                    var oldHash = BCrypt.Net.BCrypt.Verify(changedUser.OldPassword, requestUser.Hash);
                    if (!oldHash)
                    {
                        return BadRequest("A régi jelszó helytelen!");

                    }
                    if (!changedUser.NewPassword.Equals(changedUser.NewPasswordAgain))
                    {
                        return BadRequest("A két jelszó nem egyezik!");
                    
                    }
                    if (PasswordChecker.CheckPassword(changedUser.NewPassword))
                    {
                        var newHashedPassword = BCrypt.Net.BCrypt.HashPassword(changedUser.NewPassword);
                        requestUser.Hash = newHashedPassword;
                        context.Update(requestUser);
                        context.SaveChanges();
                        return Ok("A jelszavad sikeresen megváltozott");
                    }
                    else 
                    {
                        return BadRequest("A jelszó nem felel meg a kritériumoknak!");
                    }

                }
                    
            }catch (Exception ex) 
            {
                return BadRequest(ex.Message);
                
            }

           
        }
       




    }
}
