using Connect2Gether_API.Models;
using Connect2Gether_API.Models.Dtos.UserDtos;
using Microsoft.AspNetCore.Authorization;
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
                    var result = await context.UserPosts.Include(f => f.Comments).Include(f => f.User).Include(f => f.User!.Permission).ToListAsync();
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

                    UserPost result = await context.UserPosts.Include(x => x.Comments).Include(f => f.User).Include(f => f.User!.Permission).FirstOrDefaultAsync(x => x.Id == id);
                    result!.User = await context.Users.FirstOrDefaultAsync(x => x.Id == id);

                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Default, Moderator")]
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
                        UploadDate = DateTime.Now
                    };

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

        [HttpPost("Like")]
        [Authorize(Roles = "Default")]
        public IActionResult Like(LikedPostDto likedPostDto)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var existingLike = context.LikedPosts.FirstOrDefault(x => x.UserId == likedPostDto.userId && x.PostId == likedPostDto.postId);
                    if (existingLike != null && likedPostDto.isLiked == false)
                    {
                        context.LikedPosts.Remove(existingLike); 
                        var post = context.UserPosts.FirstOrDefault(x => x.Id == likedPostDto.postId);
                        if (post != null && post.Like > 0)
                        {
                            post.Like--;
                            context.SaveChanges();
                        }
                        else
                        {
                            return BadRequest("A post nem található vagy a like számláló már nulla!");
                        }
                        return Ok("A like eltávolítása sikeres!");
                    }
                    else if (existingLike == null && likedPostDto.isLiked == true)
                    {
                        LikedPost like = new LikedPost();
                        like.PostId = likedPostDto.postId;
                        like.UserId = likedPostDto.userId;
                        context.LikedPosts.Add(like);

                        var post = context.UserPosts.FirstOrDefault(x => x.Id == likedPostDto.postId);
                        if (post != null)
                        {
                            post.Like++;
                            context.SaveChanges();
                        }
                        else
                        {
                            return BadRequest("A post nem található!");
                        }
                        return Ok("A post likolása sikeres!");
                    }
                    else
                    {
                        return BadRequest("Érvénytelen kérés!");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPut("id")]
        [Authorize(Roles = "Admin, Default, Moderator")]
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
        [Authorize(Roles = "Admin, Default, Moderator")]
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
