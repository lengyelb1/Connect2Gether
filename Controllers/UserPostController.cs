using Connect2Gether_API.Models;
using Connect2Gether_API.Models.Dtos;
using Connect2Gether_API.Models.Dtos.CommentDtos;
using Connect2Gether_API.Models.Dtos.UserPostDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Reflection.Metadata;
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
                    var result = await context.UserPosts.Include(f => f.Comments)!.ThenInclude(f => f.User).Include(f => f.User).ToListAsync();
                    var simplifiedResult = result.Select(post => new
                    {
                        post.Id,
                        post.Description,
                        post.Title,
                        post.Image,
                        post.UploadDate,
                        post.Like,
                        post.Dislike,
                        User = post.User != null ? new { post.User.Id, post.User.Username } : null,
                        Comments = post.Comments!.Select(comment => new
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
        [Authorize(Roles = "Default, Admin, Moderator")]
        public IActionResult AllUserPostByOwner(int userId)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    List<AllUserPostByOwnerDto> listAllUserPostByOwners = new List<AllUserPostByOwnerDto>();
                    var result = context.UserPosts.Include(f => f.Comments).Include(f => f.User).Where(x => x.UserId == userId).ToList();
                    foreach (var item in result) 
                    {
                        AllUserPostByOwnerDto allUserPostByOwner = new AllUserPostByOwnerDto();
                        allUserPostByOwner.Id = item.Id;
                        allUserPostByOwner.Image = item.Image;
                        allUserPostByOwner.Description = item.Description;
                        allUserPostByOwner.Title = item.Title;
                        allUserPostByOwner.Like = item.Like;
                        allUserPostByOwner.Dislike = item.Dislike;
                        allUserPostByOwner.User = item.User;
                        allUserPostByOwner.UploadDate = item.UploadDate;
                        ICollection<Comment> comments = item.Comments!;
                        foreach (var cmnt in comments)
                        {
                            cmnt.User = context.Users.FirstOrDefault(u => u.Id == cmnt.UserId)!;

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
        [Authorize(Roles = "Default, Admin, ")]
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
                        userPost.Image = item.Image;
                        userPost.Description = item.Description;
                        userPost.Title = item.Title;
                        userPost.Like = item.Like;
                        userPost.Dislike = item.Dislike;
                        userPost.UserId = item.UserId;
                        userPost.User = item.User;
                        ICollection<Comment> comments = item.Comments!;

                        foreach (var cmnt in comments)
                        {
                            cmnt.User = context.Users.FirstOrDefault(u => u.Id == cmnt.UserId)!;

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
                        userPost.Disliked = (context.DislikedPosts.FirstOrDefault(x => x.Userid == userId && x.Postid == userPost.Id) != null);

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
                    var result = await context.UserPosts.Include(x => x.Comments)!.ThenInclude(x => x.User).Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id);
                    userPostsList.Add(result!);
                    var simplifiedResult = userPostsList.Select(item => new
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
                    return Ok(simplifiedResult);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpGet("UserPostByIdWithLike")]
        [Authorize(Roles = "Default, Admin, Moderator")]
        public async Task<IActionResult> UserPostByIdWithLike(int userId, int postId)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var item = await context.UserPosts.Include(x => x.Comments).Include(x => x.User).FirstOrDefaultAsync(x => x.Id == postId);
                    
                    UserPostResponseDto userPost = new UserPostResponseDto();
                    userPost.Id = item!.Id;
                    userPost.Image = item.Image;
                    userPost.Description = item.Description;
                    userPost.Title = item.Title;
                    userPost.Like = item.Like;
                    userPost.Dislike = item.Dislike;
                    userPost.UserId = item.UserId;
                    userPost.UserName = item.User!.Username;
                    userPost.UploadDate = item.UploadDate;
                    userPost.User = item.User;
                    ICollection<Comment> comments = item.Comments!;

                    foreach (var cmnt in comments)
                    {
                        cmnt.User = context.Users.FirstOrDefault(u => u.Id == cmnt.UserId)!;

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
                    userPost.Disliked = (context.DislikedPosts.FirstOrDefault(x => x.Userid == userId && x.Postid == userPost.Id) != null);
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
        [Authorize(Roles = "Admin, Default, Moderator")]
        public IActionResult AddUserPost(UserPostDto userPostDto)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    UserPost userPost = new UserPost
                    {
                        UserId = userPostDto.UserId,
                        Description = userPostDto.Description!,
                        Title = userPostDto.Title!,
                        Image = userPostDto.Image,
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
        [Authorize(Roles = "Default, Admin, Moderator")]
        public IActionResult Like(LikedPostDto likedPostDto)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var existingLike = context.LikedPosts.FirstOrDefault(x => x.UserId == likedPostDto.userId && x.PostId == likedPostDto.postId);
                    var existingDislike = context.DislikedPosts.FirstOrDefault(x => x.Userid == likedPostDto.userId && x.Postid == likedPostDto.postId);
                    if (existingLike != null && likedPostDto.isLiked == false)
                    {
                        context.LikedPosts.Remove(existingLike!);
                        var post = context.UserPosts.FirstOrDefault(x => x.Id == likedPostDto.postId);
                        if (post != null && post.Like > 0)
                        {
                            post.Like--;
                            context.SaveChanges();
                        }
                        else
                        {
                            return BadRequest("The post cannot be found or the like counter is already zero!");
                        }
                        return Ok("Like removed successfully!");
                    }
                    else if (existingLike == null && likedPostDto.isLiked == true)
                    {
                        LikedPost like = new LikedPost();
                        like.PostId = likedPostDto.postId;
                        like.UserId = likedPostDto.userId;
                        context.LikedPosts.Add(like);

                        var post = context.UserPosts.FirstOrDefault(x => x.Id == likedPostDto.postId);
                        if (existingDislike != null)
                        {
                            context.DislikedPosts.Remove(existingDislike);
                            if (post != null && post.Dislike > 0)
                            {
                                post.Dislike--;
                            }
                        }

                        var user = context.Users.FirstOrDefault(x => x.Id == post!.UserId);
                        if (post != null)
                        {
                            post.Like++;
                            context.SaveChanges();
                            if (context.Deletedlikes.Select(x=> x.UserId == user!.Id && x.PostId == post.Id).IsNullOrEmpty())
                            {
                                user!.Point++;
                                context.SaveChanges();
                                return Ok("The post has been liked successfully! +1 point");
                            }
                            context.SaveChanges();
                            return Ok("The post has been liked successfully!");
                        }
                        else
                        {
                            return BadRequest("The post not found!");
                        }
                    }
                    else
                    {
                        return BadRequest("Invalid request!");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPost("Dislike")]
        [Authorize(Roles = "Default, Admin, Moderator")]
        public IActionResult Dislike(DislikedPostDto dislikedPostDto)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var existingDislike = context.DislikedPosts.FirstOrDefault(x => x.Userid == dislikedPostDto.userId && x.Postid == dislikedPostDto.postId);
                    var existingLike = context.LikedPosts.FirstOrDefault(x => x.UserId == dislikedPostDto.userId && x.PostId == dislikedPostDto.postId);
                    if (existingDislike != null && dislikedPostDto.isDisliked == false)
                    {
                        context.DislikedPosts.Remove(existingDislike!);
                        var post = context.UserPosts.FirstOrDefault(x => x.Id == dislikedPostDto.postId);
                        if (post != null && post.Dislike > 0)
                        {
                            post.Dislike--;
                            context.SaveChanges();
                        }
                        else
                        {
                            return BadRequest("The post cannot be found or the dislike counter is already zero!");
                        }
                        return Ok("Dislike removed successfully!");
                    }
                    else if (existingDislike == null && dislikedPostDto.isDisliked == true)
                    {
                        DislikedPost dislike = new DislikedPost();
                        dislike.Postid = dislikedPostDto.postId;
                        dislike.Userid = dislikedPostDto.userId;
                        context.DislikedPosts.Add(dislike);

                        var post = context.UserPosts.FirstOrDefault(x => x.Id == dislikedPostDto.postId);
                        if (existingLike != null)
                        {
                            context.LikedPosts.Remove(existingLike);
                            if (post != null && post.Like > 0)
                            {
                                post.Like--;
                            }
                        }

                        var user = context.Users.FirstOrDefault(x => x.Id == post!.UserId);
                        if (post != null)
                        {
                            post.Dislike++;
                            context.SaveChanges();
                            return Ok("The post has been disliked successfully!");
                        }
                        else
                        {
                            return BadRequest("The post not found!");
                        }
                    }
                    else
                    {
                        return BadRequest("Invalid request!");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPut("ChangeUserPostById")]
        [Authorize(Roles = "Default, Admin, ")]
        public IActionResult ChangeUserPostById(UserPostPutDto userPostPutDto, int id, int userId)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var changedPost = context.UserPosts.FirstOrDefault(x => x.Id == id);
                    if (changedPost == null)
                    {
                        return BadRequest("This post does not exist!");
                    }
                    if (changedPost!.UserId == userId)
                    {
                        changedPost!.Description = userPostPutDto.Description;
                        changedPost.Title = userPostPutDto.Title;
                        context.UserPosts.Update(changedPost);
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

        [HttpDelete("DeleteUserPostById")]
        [Authorize(Roles = "Default, Admin, Moderator")]
        public IActionResult DeleteUserPostById(int id, int userId)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var deletedPost = context.UserPosts.FirstOrDefault(x => x.Id == id);
                    var deletedLike = context.LikedPosts.FirstOrDefault(x => x.PostId == deletedPost!.Id);
                    var deletedDisLike = context.DislikedPosts.FirstOrDefault(x => x.Postid == deletedPost!.Id);
                    if (deletedPost == null)
                    {
                        return BadRequest("This post does not exist!");
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
                            context.DislikedPosts.Remove(deletedDisLike!);
                            context.SaveChanges();
                            context.UserPosts.Remove(deletedPost);
                            context.SaveChanges();
                        }
                    }
                    else
                    {
                        return BadRequest("This user cannot make deleted!");
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
