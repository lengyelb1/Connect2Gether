using Connect2Gether_API.Models;
using Connect2Gether_API.Models.Dtos;
using Connect2Gether_API.Models.Dtos.CommentDtos;
using Connect2Gether_API.Models.Dtos.UserPostDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Connect2Gether_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserPostController : ControllerBase
    {
        [HttpGet("AllUserPost")]
        public async Task<IActionResult> AllUserPost()
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var result = await context.UserPosts.Include(f => f.Comments).ThenInclude(f => f.User).Include(f => f.User).ToListAsync();
                    var simplifiedResult = result.Select(post => new
                    {
                        post.Id,
                        post.Description,
                        post.Title,
                        post.UploadDate,
                        post.Like,
                        User = post.User != null ? new { post.User.Id, post.User.Username } : null,
                        Comments = post.Comments.Select(comment => new
                        {
                            comment.Id,
                            comment.Text,
                            comment.PostId,
                            comment.UserId,
                            User = comment.User != null ? new { comment.User.Username } : null,
                            comment.UploadDate
                        }).ToList()
                    }).ToList();
                    return StatusCode(200, simplifiedResult);
                }
                catch (Exception ex)
                {
                    return StatusCode(400, ex);
                }
            } 
        }

        [HttpGet("AllUserPostByOwner")]
        [Authorize(Roles = "Default, Admin")]
        public async Task<IActionResult> AllUserPostByOwner(int userId)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    List<AllUserPostByOwnerDto> listAllUserPostByOwners = new List<AllUserPostByOwnerDto>();
                    var result = await context.UserPosts.Include(f => f.Comments).Include(f => f.User).ToListAsync();
                    foreach (var item in result) 
                    {
                        AllUserPostByOwnerDto allUserPostByOwner = new AllUserPostByOwnerDto();
                        allUserPostByOwner.Id = item.Id;
                        allUserPostByOwner.ImageId = item.ImageId;
                        allUserPostByOwner.Description = item.Description;
                        allUserPostByOwner.Title = item.Title;
                        allUserPostByOwner.Like = item.Like;
                        allUserPostByOwner.User = item.User;
                        allUserPostByOwner.UploadDate = item.UploadDate;
                        ICollection<Comment> comments = item.Comments;
                        foreach (var cmnt in comments)
                        {
                            cmnt.User = context.Users.FirstOrDefault(u => u.Id == cmnt.UserId);

                            allUserPostByOwner.Comments.Add(new CommentResponseDto
                            {
                                Id = cmnt.Id,
                                Text = cmnt.Text,
                                PostId = cmnt.PostId,
                                UserId = cmnt.UserId,
                                UserName = cmnt.User!.Username,
                                CommentId = cmnt.CommentId,
                                UploadDate = cmnt.UploadDate
                            });
                        }
                        allUserPostByOwner.UserName = item.User!.Username;
                        allUserPostByOwner.OwnPost = item.UserId == userId;

                        listAllUserPostByOwners.Add(allUserPostByOwner);
                    }

                    context.SaveChanges();
                    return Ok(listAllUserPostByOwners);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpGet("UserPostWithLike")]
        [Authorize(Roles = "Default, Admin")]
        public async Task<IActionResult> UserPostWithLiked(int userId)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    List<UserPostResponseDto> userPostDtoToLikes = new List<UserPostResponseDto>();

                    var result = await context.UserPosts.Include(f => f.Comments).Include(f => f.User).ToListAsync();
                    foreach (var item in result)
                    {
                        UserPostResponseDto userPost = new UserPostResponseDto();
                        userPost.Id = item.Id;
                        userPost.ImageId = item.ImageId;
                        userPost.Description = item.Description;
                        userPost.Title = item.Title;
                        userPost.Like = item.Like;
                        userPost.UserId = item.UserId;
                        userPost.User = item.User;
                        ICollection<Comment> comments = item.Comments;

                        foreach (var cmnt in comments)
                        {
                            cmnt.User = context.Users.FirstOrDefault(u => u.Id == cmnt.UserId);

                            userPost.Comments.Add(new CommentResponseDto
                            {
                                Id = cmnt.Id,
                                Text = cmnt.Text,
                                PostId = cmnt.PostId,
                                UserId = cmnt.UserId,
                                UserName = cmnt.User!.Username,
                                CommentId = cmnt.CommentId,
                                UploadDate = cmnt.UploadDate
                            });
                        }
                        userPost.UserName = item.User!.Username;
                        userPost.UploadDate = item.UploadDate;
                        userPost.Liked = (context.LikedPosts.FirstOrDefault(x => x.UserId == userId && x.PostId == userPost.Id) != null);

                        userPostDtoToLikes.Add(userPost);
                    }

                    context.SaveChanges();
                    return Ok(userPostDtoToLikes);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpGet("UserPostById")]
        public async Task<IActionResult> UserPostById(int id)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    List<UserPost> userPostsList = new List<UserPost>();
                    var result = await context.UserPosts.Include(x => x.Comments).ThenInclude(x => x.User).Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id);
                    userPostsList.Add(result!);
                    var simplifiedResult = userPostsList.Select(item => new
                    {
                        item.Id,
                        item.Description,
                        item.Title,
                        item.UploadDate,
                        item.Like,
                        User = item.User != null ? new { item.User.Id, item.User.Username } : null,
                        Comments = item.Comments.Select(comment => new
                        {
                            comment.Id,
                            comment.Text,
                            comment.PostId,
                            comment.UserId,
                            User = comment.User != null ? new { comment.User.Username } : null,
                            comment.UploadDate
                        }).ToList()

                    }).ToList();
                    return Ok(simplifiedResult);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpGet("UserPostByIdWithLike")]
        [Authorize(Roles = "Default, Admin")]
        public async Task<IActionResult> UserPostByIdWithLike(int userId, int postId)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var item = await context.UserPosts.Include(x => x.Comments).Include(x => x.User).FirstOrDefaultAsync(x => x.Id == postId);
                    
                    UserPostResponseDto userPost = new UserPostResponseDto();
                    userPost.Id = item!.Id;
                    userPost.ImageId = item.ImageId;
                    userPost.Description = item.Description;
                    userPost.Title = item.Title;
                    userPost.Like = item.Like;
                    userPost.UserId = item.UserId;
                    userPost.UserName = item.User!.Username;
                    userPost.UploadDate = item.UploadDate;
                    userPost.User = item.User;
                    ICollection<Comment> comments = item.Comments;

                    foreach (var cmnt in comments)
                    {
                        cmnt.User = context.Users.FirstOrDefault(u => u.Id == cmnt.UserId);

                        userPost.Comments.Add(new CommentResponseDto
                        {
                            Id = cmnt.Id,
                            Text = cmnt.Text,
                            PostId = cmnt.PostId,
                            UserId = cmnt.UserId,
                            UserName = cmnt.User!.Username,
                            CommentId = cmnt.CommentId,
                            UploadDate = cmnt.UploadDate
                        });
                    }
                    userPost.Liked = (context.LikedPosts.FirstOrDefault(x => x.UserId == userId && x.PostId == userPost.Id) != null);
                    context.SaveChanges();
                    return Ok(userPost);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPost("AddUserPost")]
        [Authorize(Roles = "Admin, Default")]
        public IActionResult AddUserPost(UserPostDto userPostDto)
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
                    /*if (userPostDto.Image == null)
                    {
                        context.UserPosts.Add(userPost);
                        context.SaveChanges();
                        return Ok(userPost);
                    }
                    else
                    {
                        context.Images.Add(userPostDto.Image);
                        context.SaveChanges();
                        userPost.ImageId = userPostDto.Image.Id;
                        context.UserPosts.Add(userPost);
                        context.SaveChanges();
                        return Ok(userPost);
                    }*/
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
        [Authorize(Roles = "Default, Admin")]
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

        [HttpPut("ChangeUserPostById")]
        [Authorize(Roles = "Default, Admin")]
        public IActionResult ChangeUserPostById(UserPostPutDto userPostPutDto, int id, int userId)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var changedPost = context.UserPosts.FirstOrDefault(x => x.Id == id);
                    if (changedPost == null)
                    {
                        return BadRequest("Nics ilyen post!");
                    }
                    if (changedPost!.UserId == userId)
                    {
                        changedPost!.Description = userPostPutDto.Description;
                        changedPost.Title = userPostPutDto.Title;
                        context.UserPosts.Update(changedPost);
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

        [HttpDelete("DeleteUserPostById")]
        [Authorize(Roles = "Default, Admin")]
        public IActionResult DeleteUserPostById(int id, int userId)
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

                    if (deletedPost.UserId == userId)
                    {
                        if (deletedPost != null && deletedLike == null)
                        {
                            context.UserPosts.Remove(deletedPost);
                            context.SaveChanges();
                        }
                        else if (deletedPost != null && deletedLike != null)
                        {
                            context.LikedPosts.Remove(deletedLike!);
                            context.SaveChanges();
                            context.UserPosts.Remove(deletedPost);
                            context.SaveChanges();
                        }
                    }
                    else
                    {
                        return BadRequest("Ez a user nem törölhet!");
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
