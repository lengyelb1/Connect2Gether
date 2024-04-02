using Connect2Gether_API.Models;
using Connect2Gether_API.Models.Dtos.UserDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Connect2Gether_API.Controllers.UserControllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        [HttpGet("UserProfile")]
        public IActionResult UserProfile(int id, UserProfileDto userProfileDto)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var user = context.Users.FirstOrDefault(x => x.Id == id);
                    var userPostDb = context.UserPosts.Where(x => x.UserId == user!.Id).ToList().Count;
                    var userCommentDb = context.Comments.Where(x => x.UserId == user!.Id).ToList().Count;
                    if (user == null) 
                    {
                        return StatusCode(404, "Nincs ilyen user");
                    }
                    else
                    {
                        user.Username = userProfileDto.UserName;
                        user.Email = userProfileDto.Email;
                        user.Point = userProfileDto.Points;
                        user.RankId = userProfileDto.RankId;
                        user.RegistrationDate = userProfileDto.RegistrationDate;
                        userPostDb = userProfileDto.PostDb;
                        userCommentDb = userProfileDto.CommentDb;
                        return Ok(user);
                    }    
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}
