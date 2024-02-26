using Connect2Gether_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Connect2Gether_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post(Comment comment)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    Comment comment1 = new Comment();

                    comment1.Id = comment.Id;
                    comment1.Post = context.UserPosts.FirstOrDefault(p => p.Id == comment.PostId);
                    comment1.User = context.Users.FirstOrDefault(p => p.Id == comment.UserId);
                    comment1.Text = comment.Text;
                    comment1.CommentId = comment.CommentId;

                    context.Comments.Add(comment1);
                    context.SaveChanges();
                    return Ok("Sikeres feltöltés!");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}
