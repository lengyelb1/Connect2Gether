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
using Connect2Gether_API.Models.Dtos;
using Connect2Gether_API.Models.Dtos.UserPostDtos;




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
        [Authorize(Roles = "Default, Admin, Moderator")]
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
                    return Ok("Upload successfully!");
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
                        User = new { comment.User!.Username },
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
        [Authorize(Roles = "Default, Admin, Moderator")]
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
                        allCommentByOwner.UploadDate = item.UploadDate;
                        allCommentByOwner.UserName = item.User!.Username;
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

        [HttpGet("CommentById")]
        public IActionResult CommentById(int id)
        {
            try
            {
                using (var context = new Connect2getherContext())
                {
                    List<Comment> comments = new List<Comment>();
                    var comment = context.Comments.Include(p => p.User).FirstOrDefault(p => p.Id == id);
                    comments.Add(comment!);

                    if (comment == null)
                    {
                        return NotFound("This comment does not exist!");
                    }
                    else
                    {
                        var simplifiedComment = comments.Select(item => new
                        {
                            item.Id,
                            item.Text,
                            item.PostId,
                            item.UserId,
                            item.CommentId,
                            item.UploadDate,
                            User = item.User != null ? new { item.User.Username } : null,

                        }).ToList();
                        return Ok(simplifiedComment);
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("CommentByPostId")]
        public IActionResult CommentByPostId(int id)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    List<Comment> comments = new List<Comment>();
                    var comment = context.Comments.Include(p => p.User).FirstOrDefault(p => p.PostId == id);
                    comments.Add(comment!);

                    if (comment == null)
                    {
                        return NotFound("This comment does not exist!");
                    }
                    else
                    {
                        var simplifiedComment = comments.Select(item => new
                        {
                            item.Id,
                            item.Text,
                            item.PostId,
                            item.UserId,
                            item.CommentId,
                            item.UploadDate,
                            User = item.User != null ? new { item.User.Username } : null,

                        }).ToList();
                        return Ok(simplifiedComment);
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        /*[HttpPut("AdminOperation/ChangeCommentByAdmin")]
        [Authorize(Roles = "Admin")]
        public IActionResult ChangeCommentByAdmin(int id, CommentPutDto commentPutDto)
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
                    changedComment.Text = commentPutDto.Text!;
                    context.Comments.Update(changedComment);
                    context.SaveChanges();
                    return Ok("Sikeres módosítás!");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }*/

        [HttpPut("ChangeOwnComment")]
        [Authorize(Roles = "Default,Admin,Moderator")]
        public IActionResult ChangeOwnComment(CommentPutDto commentPutDto, int id, int userId)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var changedComment = context.Comments.FirstOrDefault(x => x.Id == id);
                    if (changedComment == null)
                    {
                        return BadRequest("This comment does not exist!");
                    }
                    if (changedComment!.UserId == userId)
                    {
                        changedComment.Text = commentPutDto.Text!;
                        context.Comments.Update(changedComment);
                        context.SaveChanges();
                        return Ok("Changes successfully!");
                    }
                    else
                    {
                        return BadRequest("This user cannot make changes!");
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
        public IActionResult DeleteByAdmin(int id, AlertMessageDto alertMessageDto)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var deleteComment = context.Comments.FirstOrDefault(x => x.Id == id);
                    if (deleteComment == null)
                    {
                        return BadRequest("This comment does not exist!");
                    }
                    context.Comments.Remove(deleteComment);
                    context.SaveChanges();
                    Alertmessage alertmessage = new Alertmessage();
                    alertmessage.Title = alertMessageDto.title;
                    alertmessage.Description = alertMessageDto.description;
                    alertmessage.UserId = deleteComment.UserId;
                    context.Alertmessages.Add(alertmessage);
                    context.SaveChanges();
                    return Ok("Deleted successfully!");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpDelete("DeleteCommentByUserId")]
        [Authorize(Roles = "Default, Admin, Moderator")]
        public IActionResult DeleteByUser(int id, int userId)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var deleteComment = context.Comments.FirstOrDefault(x => x.Id == id);
                    var deletedCommentPost = context.UserPosts.FirstOrDefault(x => x.Id == deleteComment!.PostId);
                    if (deleteComment == null)
                    {
                        return BadRequest("This comment does not exist!");
                    }
                    if (deleteComment!.UserId == userId || deletedCommentPost!.UserId == userId)
                    {
                        context.Comments.Remove(deleteComment);
                        context.SaveChanges();
                        return Ok("Deleted successfully!");
                    }
                    else
                    {
                        return BadRequest("This user cannot make deleted!");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        /*[HttpDelete("DeleteCommentByPostUserId")]
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
        }*/
    }
}