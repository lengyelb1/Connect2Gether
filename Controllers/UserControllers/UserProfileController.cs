using Connect2Gether_API.Models;
using Connect2Gether_API.Models.Dtos.UserDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Connect2Gether_API.Controllers.UserControllers
{
    [Route("[controller]")]
    [ApiController]
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
    }
}
