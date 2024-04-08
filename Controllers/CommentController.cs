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
using MySqlX.XDevAPI.Common;
using Connect2Gether_API.Models.Dtos.CommentDtos;
using System.Collections.Generic;




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

        public static int JWTokenDecodeID(string jwt)
        {
            JwtSecurityToken token = JWTokenDecoder(jwt);
            return int.Parse(token.Claims.First(claim => claim.Type == "id").Value);
        }

        [HttpPost("AddComment")]
        [Authorize(Roles = "Default")]
        public IActionResult AddComment(CommentDto commentDto)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    Comment comment1 = new Comment();

                    comment1.Id = commentDto.Id;
                    comment1.Post = context.UserPosts.FirstOrDefault(p => p.Id == commentDto.PostId);
                    comment1.User = context.Users.FirstOrDefault(p => p.Id == commentDto.UserId);
                    comment1.Text = commentDto.Text;
                    comment1.CommentId = commentDto.CommentId;
                    comment1.UploadDate = DateTime.Now;

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

        [HttpGet("AllComment")]
        public IActionResult AllComment()
        {
            try
            {
                using (var context = new Connect2getherContext())
                {
                    var result = context.Comments.Include(x => x.User).Include(x => x.User!.Permission).ToList();
                    var simplifiedResult = result.Select(comment => new
                    {
                        comment.Id,
                        comment.Text,
                        comment.PostId,
                        comment.UserId,
                        comment.UploadDate,
                        User = new { comment.User!.Username, comment.User.Permission },
                    }).ToList();
                    return Ok(simplifiedResult);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("AllCommentByOwner")]
        [Authorize(Roles = "Default")]
        public async Task<IActionResult> AllCommentByOwner(int userId)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    List<AllCommentByOwnerDto> listAllCommentByOwners = new List<AllCommentByOwnerDto>();
                    var result = await context.Comments.Include(x => x.User).Include(x => x.User!.Permission).ToListAsync();
                    foreach (var item in result)
                    {
                        AllCommentByOwnerDto allCommentByOwner = new AllCommentByOwnerDto();
                        allCommentByOwner.Id = item.Id;
                        allCommentByOwner.Text = item.Text;
                        allCommentByOwner.PostId = item.PostId;
                        allCommentByOwner.UserId = item.UserId;
                        allCommentByOwner.CommentId = item.CommentId;
                        allCommentByOwner.Post = item.Post;
                        allCommentByOwner.User = item.User;
                        allCommentByOwner.OwnComment = item.UserId == userId;

                        listAllCommentByOwners.Add(allCommentByOwner);
                    }

                    context.SaveChanges();
                    return Ok(listAllCommentByOwners);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpGet("CommentByPostId")]
        [Authorize(Roles = "Default")]
        public IActionResult CommentByPostId(int id)
        {
            try
            {
                using (var context = new Connect2getherContext())
                {

                    var post = context.UserPosts.Include(p => p.Comments).Include(p => p.User).FirstOrDefault(p => p.Id == id);

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

        [HttpPut("AdminOperation/ChangeCommentByAdmin")]
        [Authorize(Roles = "Admin")]
        public IActionResult ChangeCommentByAdmin(int id, CommentDto updatedCommentDto, int userId)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var changedComment = context.Comments.FirstOrDefault(x => x.Id == id);
                    if (changedComment == null)
                    {
                        return BadRequest("Nics ilyen comment!");
                    }
                    if (changedComment!.UserId == userId)
                    {
                        changedComment.Text = updatedCommentDto.Text!;
                        context.Comments.Update(changedComment);
                        context.SaveChanges();
                        return Ok("Sikeres módosítás!");
                    }
                    else
                    {
                        return BadRequest("Ez a user nem változtathat!");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPut("ChangeOwnComment")]
        [Authorize(Roles = "Default,Admin")]
        public IActionResult ChangeOwnComment(CommentDto commentDto, int id, int userId)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var changedComment = context.Comments.FirstOrDefault(x => x.Id == id);
                    if (changedComment == null)
                    {
                        return BadRequest("Nics ilyen comment!");
                    }
                    if (changedComment!.UserId == userId)
                    {
                        changedComment.Text = commentDto.Text!;
                        context.Comments.Update(changedComment);
                        context.SaveChanges();
                        return Ok("Sikeres módosítás!");
                    }
                    else
                    {
                        return BadRequest("Ez a user nem változtathat!");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpDelete("AdminOperation/DeleteCommentByAdmin")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteByAdmin(int id, int userId)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var deleteComment = context.Comments.FirstOrDefault(x => x.Id == id);
                    if (deleteComment == null)
                    {
                        return BadRequest("Nincs ilyen comment!");
                    }
                    if (deleteComment!.UserId == userId)
                    {
                        context.Comments.Remove(deleteComment);
                        context.SaveChanges();
                        return Ok("Sikeres törlés!");
                    }
                    else
                    {
                        return BadRequest("Ez a user nem törölhet!");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpDelete("DeleteCommentByUserId")]
        [Authorize(Roles = "Default, Admin")]
        public IActionResult DeleteByUser(int id, int userId)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var deleteComment = context.Comments.FirstOrDefault(x => x.Id == id);
                    if (deleteComment == null)
                    {
                        return BadRequest("Nincs ilyen comment!");
                    }
                    if (deleteComment!.UserId == userId)
                    {
                        context.Comments.Remove(deleteComment);
                        context.SaveChanges();
                        return Ok("Sikeres törlés!");
                    }
                    else
                    {
                        return BadRequest("Ez a user nem törölhet!");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpDelete("DeleteCommentByPostUserId")]
        [Authorize(Roles = "Default, Admin")]
        public IActionResult DeleteByPostUserId(int id, int userId)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var deleteComment = context.Comments.FirstOrDefault(x => x.Id == id);
                    if (deleteComment == null)
                    {
                        return BadRequest("Nincs ilyen comment!");
                    }
                    var deletedCommentPost = context.UserPosts.FirstOrDefault(x => x.Id == deleteComment.PostId);
                    if (deletedCommentPost!.UserId == userId)
                    {
                        context.Comments.Remove(deleteComment);
                        context.SaveChanges();
                        return Ok("Sikeres törlés!");
                    }
                    else
                    {
                        return BadRequest("Ez a user nem törölhet!");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}