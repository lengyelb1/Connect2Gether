using Connect2Gether_API.Models;
using Connect2Gether_API.Models.Dtos;
using Connect2Gether_API.Models.Dtos.UserPostDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Connect2Gether_API.Controllers.AdminControllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminUserPostController : ControllerBase
    {
        [HttpGet("UserPostCount")]
        public IActionResult UserPostCount()
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var db = context.UserPosts.ToList().Count();
                    return Ok(db);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        /*[HttpGet("SearchPost")]
        public IActionResult SearchPost(string nev)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    return Ok(context.UserPosts.Include(x => x.User).Include(x => x.User!.Permission).Where(x => x.Title.Contains(nev)).ToList());
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }*/

        [HttpGet("UserGetPostById")]
        public IActionResult UserGetPostById(int id)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var request = context.UserPosts.Include(x => x.Comments).Include(x => x.User).Include(x => x.User!.Permission).Where(x => x.Id == id).ToList();
                    return Ok(request);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpDelete("DeleteUserPostById")]
        public IActionResult DeleteUserPostById(int id, AlertMessageDto alertMessageDto)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var deletedPost = context.UserPosts.FirstOrDefault(x => x.Id == id);
                    var deletedLike = context.LikedPosts.FirstOrDefault(x => x.PostId == deletedPost!.Id);
                    if (deletedPost == null)
                    {
                        return BadRequest("Ez a post nem létezik!");
                    }
                    else if (deletedPost != null && deletedLike == null)
                    {
                        context.UserPosts.Remove(deletedPost);
                        context.SaveChanges();
                        Alertmessage alertMessage = new Alertmessage();
                        alertMessage.Title = alertMessageDto.title;
                        alertMessage.Description = alertMessageDto.description;
                        alertMessage.UserId = (int)deletedPost.UserId!;
                        context.Alertmessages.Add(alertMessage);
                        context.SaveChanges();
                    }
                    else if (deletedPost != null && deletedLike != null)
                    {
                        context.LikedPosts.Remove(deletedLike!);
                        context.SaveChanges();
                        context.UserPosts.Remove(deletedPost);
                        context.SaveChanges();
                        Alertmessage alertMessage = new Alertmessage();
                        alertMessage.Title = alertMessageDto.title;
                        alertMessage.Description = alertMessageDto.description;
                        alertMessage.UserId = (int)deletedPost.UserId!;
                        context.Alertmessages.Add(alertMessage);
                        context.SaveChanges();
                    }
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
