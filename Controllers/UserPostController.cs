using Connect2Gether_API.Models;
using Connect2Gether_API.Models.Dtos.UserDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Connect2Gether_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserPostController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var result = await context.UserPosts.Include(f => f.Comments).ToListAsync();
                    context.SaveChanges();
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserPostDto userPostDto)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    UserPost post = new UserPost();

                    post.UserId = userPostDto.UserId;
                    post.Title = userPostDto.Title;
                    post.Description = userPostDto.Description;
                    post.Like = 0;
                    context.UserPosts.Add(post);
                    context.SaveChanges();
                    return Ok("Post added!");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPost("Lke")]
        public async Task<IActionResult> Like(int postId,int userId)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    LikedPost like = new LikedPost();
                    like.PostId = postId;
                    like.UserId = userId;
                    context.LikedPosts.Add(like);
                    context.SaveChanges();
                    return Ok("Post liked!");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}
