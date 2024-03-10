using Connect2Gether_API.Models;
using Connect2Gether_API.Models.Dtos.UserDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IO;




namespace Connect2Gether_API.Controllers
{
   
    [Route("[controller]")]
    [ApiController]
    
    public class CommentController : ControllerBase
    {
        public static JwtSecurityToken JWTokenDecoder(string token)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            return jwtHandler.ReadJwtToken(token);
        }
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
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                using (var context = new Connect2getherContext())
                {
                    return Ok(context.Comments.ToList());

                }


            }
            catch (Exception e)
            {
                return BadRequest(e.Message);

            }

        }
        [HttpGet("{id}")]
        public IActionResult GetByPost(int id)
        {
            try
            {
                using (var context = new Connect2getherContext())
                {

                    var post = context.UserPosts.Include(p => p.Comments).FirstOrDefault(p => p.Id == id);

                    if (post == null)
                    {
                        return NotFound("Nincs ilyen Post");
                    }
                    else
                    {

                        return Ok(post.Comments.ToList());
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



        [Authorize(Roles = "Admin")]
        [HttpPut("AdminOperation/{id}")]
        public IActionResult Put(int id, CommentDto updatedCommentDto)
        {

            using (var context = new Connect2getherContext())
            {
                try
                {
                    var existingComment = context.Comments.FirstOrDefault(c => c.Id == id);

                    if (existingComment == null)
                    {
                        return NotFound("A megadott komment nem található.");
                    }

                    existingComment.Text = updatedCommentDto.Text;

                    context.SaveChanges();

                    return Ok("Sikeres frissítés!");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        [Authorize(Roles = "Default,Admin")]
        [HttpPut]
       public IActionResult OwnCommentPut(CommentDto commentDto)
        {
            try
            {
              
                

                using (var context = new Connect2getherContext())
                {
                    var existingComment = context.Comments.FirstOrDefault(c => c.Id == commentDto.Id);

                    if (existingComment == null)
                    {
                        return NotFound("A megadott komment nem található.");
                    }

                   
                    

                    existingComment.Text = commentDto.Text;

                    context.SaveChanges();

                    return Ok("Sikeres frissítés!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("AdminOperation/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteByAdmin(int id)
        {
            try
            {
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                using (var context = new Connect2getherContext())
                {
                    var existingComment = context.Comments.FirstOrDefault(c => c.Id == id);

                    if (existingComment == null)
                    {
                        return NotFound("A megadott komment nem található.");
                    }

                    context.Comments.Remove(existingComment);
                    context.SaveChanges();

                    return Ok("A komment sikeresen törölve lett.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteByUser(int id)
        {
            try
            {

                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                using (var context = new Connect2getherContext())
                {
                    var existingComment = context.Comments.FirstOrDefault(c => c.Id == id);

                    if (existingComment == null)
                    {
                        return NotFound("A megadott komment nem található.");
                    }


                    if (existingComment.UserId != userId)
                    {
                        return Forbid();
                    }

                    context.Comments.Remove(existingComment);
                    context.SaveChanges();

                    return Ok("A komment sikeresen törölve lett.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }









    }
}
