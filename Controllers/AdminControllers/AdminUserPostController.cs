using Connect2Gether_API.Models;
using Connect2Gether_API.Models.Dtos;
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
        [HttpGet("UserPostDb")]
        public IActionResult UserPostDB()
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

        [HttpGet("SearchPost")]
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
        }

        [HttpGet("UserGetPosts")]
        public IActionResult UserGetPosts(int id)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var request = context.UserPosts.Include(x => x.Comments).Where(x => x.User!.Id == id).ToList();
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
                    var deletePost = context.UserPosts.FirstOrDefault(x => x.Id == id)!;
                    var userId = (int)deletePost!.UserId!;
                    if (deletePost == null) 
                    {
                        return NotFound("Az adott post nem létezik!");
                    }
                    var deleteLike = context.LikedPosts.FirstOrDefault(x => x.UserId == userId && x.PostId == deletePost.Id);
                    if (deleteLike != null)
                    {
                        context.LikedPosts.Remove(deleteLike!);
                    }
                    context.UserPosts.Remove(deletePost!);
                    Alertmessage alertmessage = new Alertmessage();
                    alertmessage.Title = alertMessageDto.title;
                    alertmessage.Description = alertMessageDto.description;
                    alertmessage.UserId = userId;
                    context.Alertmessages.Add(alertmessage);
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
