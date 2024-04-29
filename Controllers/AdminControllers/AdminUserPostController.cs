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

        [HttpGet("UserGetPostById")]
        public IActionResult UserGetPostById(int id)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var request = context.UserPosts.Include(x => x.Comments).ThenInclude(x => x.User).Include(x => x.User).Where(x => x.Id == id).ToList();
                    var simplifiedRequest = request.Select(item => new
                    {
                        item.Id,
                        item.Description,
                        item.Title,
                        item.Image,
                        item.UploadDate,
                        item.Like,
                        item.Dislike,
                        User = item.User != null ? new { item.User.Id, item.User.Username } : null,
                        Comments = item.Comments!.Select(comment => new
                        {
                            comment.Id,
                            comment.Text,
                            comment.PostId,
                            comment.UserId,
                            User = comment.User != null ? new { comment.User.Username } : null,
                            comment.UploadDate
                        }).ToList()

                    }).ToList();
                    return Ok(simplifiedRequest);
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
                        return BadRequest("This post does not exist!");
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
                    return Ok("Deleted successfully!");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}
