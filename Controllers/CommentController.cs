using Connect2Gether_API.Models;
using Connect2Gether_API.Models.Dtos.UserDtos;
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
                    if (comment.Text == "" || comment.Text == null)
                    {
                        return BadRequest("The text cannot be empty!");
                    }

                    context.Comments.Add(comment);
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
