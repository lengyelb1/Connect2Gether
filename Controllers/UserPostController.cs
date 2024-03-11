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

        [HttpGet("id")]
        public async Task<IActionResult> GetById(int id)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    UserPost result = await context.UserPosts.FirstOrDefaultAsync(x => x.Id == id);
                    result.User = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPost]
        public IActionResult Post(UserPostDto userPostDto)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {

                    UserPost userPost = new UserPost
                    {

                        UserId = userPostDto.UserId,
                        Description = userPostDto.Description,
                        Title = userPostDto.Title,
                    };


                    /*UserPost userPost = new UserPost();
                    userPost.UserId = userPostDto.UserId;
                    userPost.Description = userPostDto.Description;
                    userPost.Title = userPostDto.Title;
                    userPost.Like = 0;
                    userPost.User = context.Users.FirstOrDefault(x => x.Id == userPostDto.UserId);*/

                    context.UserPosts.Add(userPost);
                    context.SaveChanges();
                    return Ok(userPost);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }


        /*Test*/
        [HttpPost("Like")]
        public IActionResult Like(int postId, int userId)
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


        [HttpPut("id")]
        public IActionResult Put(UserPostPutDto userPostPutDto, int id)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    UserPost changedPost = new UserPost { Id = id };
                    changedPost.Description = userPostPutDto.Description;
                    changedPost.Title = userPostPutDto.Title;
                    context.UserPosts.Update(changedPost);
                    context.SaveChanges();
                    return Ok("Sikeres módosítás!");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpDelete("id")]
        public IActionResult Delete(int id)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    UserPost deletePost = new UserPost { Id = id };
                    context.UserPosts.Remove(deletePost);
                    context.SaveChanges();
                    return Ok("Sikeres törlés!");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}
