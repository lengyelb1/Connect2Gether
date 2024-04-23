using Connect2Gether_API.Models;
using Connect2Gether_API.Models.Dtos;
using Connect2Gether_API.Models.Dtos.CommentDtos;
using Connect2Gether_API.Models.Dtos.UserDtos;
using Connect2Gether_API.Models.Dtos.UserPostDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Prng;
using System.Security.Claims;

namespace Connect2Gether_API.Controllers.UserControllers
{

    /* Oldal funkciók */
    /* Jelszó / azonosítóval való lekérés*/
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet("SearchWithNameOrTitle")]
        [Authorize(Roles = "Default, Admin, Moderator")]
        public IActionResult SearchWithNameOrTitle(string keresettErtek, int userId)
        {
            // Error 4001 nincs @ de van @
            using (var context = new Connect2getherContext())
            {
                try
                {
                    string[] strings = keresettErtek.Split(' ');
                    bool vankuk = false;
                    bool nincskuk = false;
                    string usernev = "";
                    string cim = "";
                    foreach (string s in strings)
                    {
                        if (s.StartsWith("@"))
                        {
                            vankuk = true;
                            usernev = s.Trim('@');
                        }
                        else
                        {
                            nincskuk = true;
                            cim += " " + s;

                        }
                    }
                    List<UserPostResponseDto> userPostDtoToLikes = new List<UserPostResponseDto>();
                    if (vankuk == true && nincskuk == false)
                    {
                        var result = context.Users.Where(x => x.Username.Contains(keresettErtek.TrimStart('@'))).ToList();
                        var simplifiedResult = result.Select(user => new
                        {
                            user.Id,
                            user.Username
                        }).ToList();
                        return Ok(simplifiedResult);
                    }
                    else if (vankuk == false && nincskuk == true)
                    {
                        var result = context.UserPosts.Include(x => x.User).Include(x => x.User!.Permission).Include(x => x.Comments).Where(x => x.Title.Contains(keresettErtek)).ToList();
                        foreach (var item in result)
                        {
                            UserPostResponseDto userPost = new UserPostResponseDto();
                            userPost.Id = item.Id;
                            userPost.Image = item.Image;
                            userPost.Description = item.Description;
                            userPost.Title = item.Title;
                            userPost.Like = item.Like;
                            userPost.UserId = item.UserId;
                            userPost.UserName = item.User!.Username;
                            userPost.User = item.User;
                            ICollection<Comment> comments = item.Comments;
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
                            userPost.UploadDate = item.UploadDate;
                            userPost.Liked = (context.LikedPosts.FirstOrDefault(x => x.UserId == userId && x.PostId == userPost.Id) != null);
                            userPostDtoToLikes.Add(userPost);
                        }

                        return Ok(userPostDtoToLikes);
                    }
                    else if (vankuk == true && nincskuk == true)
                    {
                        var result = context.UserPosts.Include(x => x.User).Include(x => x.User!.Permission).Include(x => x.Comments).Where(x => x.Title.ToLower().Contains(cim.ToLower().TrimStart())).ToList();
                        foreach (var item in result)
                        {
                            UserPostResponseDto userPost = new UserPostResponseDto();
                            userPost.Id = item.Id;
                            userPost.Image = item.Image;
                            userPost.Description = item.Description;
                            userPost.Title = item.Title;
                            userPost.Like = item.Like;
                            userPost.UserId = item.UserId;
                            userPost.UserName = item.User!.Username;
                            userPost.User = item.User;
                            ICollection<Comment> comments = item.Comments;
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
                            userPost.UploadDate = item.UploadDate;
                            userPost.Liked = (context.LikedPosts.FirstOrDefault(x => x.UserId == userId && x.PostId == userPost.Id) != null);
                            userPostDtoToLikes.Add(userPost);
                        }
                        return Ok(userPostDtoToLikes);
                    }
                    else
                    {
                        return BadRequest("Something went wrong! #4001");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        /*[HttpGet("SearchUser")]
        [Authorize(Roles = "Default, Admin")]
        public IActionResult SearchUser(string nev)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    return Ok(context.Users.Where(x => x.Username.Contains(nev)).Include(x => x.Permission).ToList());
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }*/

        /*[HttpGet("SearchPost")]
        public IActionResult SearchPost(string nev)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    return Ok(context.UserPosts.Where(x => x.Title.Contains(nev)).Include(x => x.User).Include(x => x.User!.Permission).ToList());
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }*/

        [HttpGet("UserGetPost")]
        public IActionResult UserGetPost(int id)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var request = context.UserPosts.Include(x => x.Comments).ThenInclude(x => x.User).Include(x => x.User).Where(x => x.Id == id).ToList();
                    var simplifiedRequest = request.Select(post => new
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
                    return Ok(simplifiedRequest);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpGet("UserById")]
        [Authorize]
        public IActionResult UserById(int id)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    List<User> usersList = new List<User>();
                    var userById = context.Users.Include(x => x.UserPosts)!.ThenInclude(x => x.Comments).ThenInclude(x => x.User).Include(x => x.Rank).Include(x => x.Permission).Include(x => x.LikedPosts).FirstOrDefault(x => x.Id == id);
                    usersList.Add(userById!);
                    List<UserByIdDto> userByIdDtoList = new List<UserByIdDto>();
                    if (userById == null) 
                    {
                        return BadRequest("This user does not exist!");
                    }

                    foreach (var item in usersList)
                    {
                        UserByIdDto userByIdDto = new UserByIdDto();
                        userByIdDto.Id = item.Id;
                        userByIdDto.Username = item.Username;
                        userByIdDto.Rank = item.Rank;
                        userByIdDto.Points = item.Point;
                        userByIdDto.RegistrationDate = item.RegistrationDate;
                        userByIdDto.LastLogin = item.LastLogin;
                        ICollection<UserPost> posts = item.UserPosts!;
                        foreach (var pts in posts)
                        {
                            pts.User = context.Users.FirstOrDefault(x => x.Id == pts.UserId);

                            userByIdDto.UserPosts!.Add(new UserPostResponseDto
                            {
                                Id = pts.Id,
                                Image = pts.Image,
                                Description = pts.Description,
                                Title = pts.Title,
                                Like = pts.Like,
                                UserId = pts.UserId,
                                UserName = pts.User!.Username,
                                UploadDate = pts.UploadDate,
                                Comments = convertComments(pts.Comments.ToList()),
                                Liked = (context.LikedPosts.FirstOrDefault(x => x.PostId == pts.Id) != null)
                            });
                        }
                        userByIdDtoList.Add(userByIdDto);
                    }

                    return Ok(userByIdDtoList);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        private ICollection<CommentResponseDto> convertComments(List<Comment> comments)
        {
            int x = 0;

            List<CommentResponseDto> response = new List<CommentResponseDto>();

            while (comments.Count != x)
            {
                response.Add(new CommentResponseDto
                {
                    Id = comments[x].Id,
                    Text = comments[x].Text,
                    PostId = comments[x].PostId,
                    UserId = comments[x].UserId,
                    UploadDate = comments[x].UploadDate,
                    UserName = comments[x].User.Username
                });
                x++;
            }
            return response;
        }

        [HttpGet("UserAllAlertMessage")]
        [Authorize(Roles = "Default, Admin, Moderator")]
        public IActionResult UserAllAlertMessage(int userId)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var userAllAlertMessage = context.Alertmessages.Include(x => x.User).Where(x => x.UserId == userId).ToList();
                    var simplifiedUserAlertMessage = userAllAlertMessage.Select(item => new
                    {
                        item.Id,
                        item.Title,
                        item.UserId,
                        item.Description,
                        User = item.User != null ? new { item.User.Username } : null,

                    }).ToList();
                    return Ok(simplifiedUserAlertMessage);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}
