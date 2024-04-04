using Connect2Gether_API.Models;
using Connect2Gether_API.Models.Dtos;
using Connect2Gether_API.Models.Dtos.UserPostDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

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
                    var result = await context.UserPosts.Include(f => f.Comments).Include(f => f.User).Include(f => f.User!.Permission).ToListAsync();
                    var simplifiedResult = result.Select(post => new
                    {
                        post.Id,
                        post.Description,
                        post.Title,
                        post.UploadDate,
                        User = post.User != null ? new { post.User.Id, post.User.Username, Permission = post.User.Permission?.Name } : null,
                        Comments = post.Comments.Select(comment => new
                        {
                            comment.Id,
                            comment.Text,
                            comment.PostId,
                            comment.UserId,
                            User = comment.User != null ? new { comment.User.Username, Permission = comment.User.Permission?.Name } : null,
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
        [Authorize(Roles = "Default")]
        public async Task<IActionResult> AllUserPostByOwner(int userId)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    List<AllUserPostByOwnerDto> listAllUserPostByOwners = new List<AllUserPostByOwnerDto>();
                    var result = await context.UserPosts.Include(f => f.Comments).Include(f => f.User).Include(f => f.User!.Permission).ToListAsync();
                    foreach (var item in result) 
                    {
                        AllUserPostByOwnerDto allUserPostByOwner = new AllUserPostByOwnerDto();
                        allUserPostByOwner.Id = item.Id;
                        allUserPostByOwner.ImageId = item.ImageId;
                        allUserPostByOwner.Description = item.Description;
                        allUserPostByOwner.Title = item.Title;
                        allUserPostByOwner.Like = item.Like;
                        allUserPostByOwner.UserId = item.UserId;
                        allUserPostByOwner.Comments = item.Comments;
                        allUserPostByOwner.User = item.User;
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
        [Authorize(Roles = "Default")]
        public async Task<IActionResult> UserPostWithLiked(int userId)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    List<UserPostDtoToLike> userPostDtoToLikes = new List<UserPostDtoToLike>();

                    var result = await context.UserPosts.Include(f => f.Comments).Include(f => f.User).Include(f => f.User!.Permission).ToListAsync();
                    foreach (var item in result)
                    {
                        UserPostDtoToLike userPost = new UserPostDtoToLike();
                        userPost.Id = item.Id;
                        userPost.ImageId = item.ImageId;
                        userPost.Description = item.Description;
                        userPost.Title = item.Title;
                        userPost.Like = item.Like;
                        userPost.UserId = item.UserId;
                        userPost.Comments = item.Comments;
                        userPost.User = item.User;
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
                    var result = await context.UserPosts.Include(x => x.Comments).Include(x => x.User).Include(x => x.User!.Permission).FirstOrDefaultAsync(x => x.Id == id);
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpGet("UserPostByIdWithLike")]
        [Authorize(Roles = "Default")]
        public async Task<IActionResult> UserPostByIdWithLike(int userId, int postId)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var item = await context.UserPosts.Include(x => x.Comments).Include(x => x.User).Include(f => f.User!.Permission).FirstOrDefaultAsync(x => x.Id == postId);
                    
                    UserPostDtoToLike userPost = new UserPostDtoToLike();
                    userPost.Id = item!.Id;
                    userPost.ImageId = item.ImageId;
                    userPost.Description = item.Description;
                    userPost.Title = item.Title;
                    userPost.Like = item.Like;
                    userPost.UserId = item.UserId;
                    userPost.Comments = item.Comments;
                    userPost.User = item.User;
                    userPost.UserId = item.UserId;
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
        [Authorize(Roles = "Default")]
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
        [Authorize(Roles = "Default, Moderator")]
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
        [Authorize(Roles = "Default, Moderator")]
        public IActionResult DeleteUserPostById(int id, int userId)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var deletePost = context.UserPosts.FirstOrDefault(x => x.Id == id);
                    if (deletePost == null)
                    {
                        return BadRequest("Nincs ilyen post!");
                    }
                    if (deletePost!.UserId == userId)
                    {
                        context.UserPosts.Remove(deletePost!);
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
