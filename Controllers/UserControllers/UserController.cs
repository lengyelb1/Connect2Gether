using Connect2Gether_API.Models;
using Connect2Gether_API.Models.Dtos;
using Connect2Gether_API.Models.Dtos.UserDtos;
using Connect2Gether_API.Models.Dtos.UserPostDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Connect2Gether_API.Controllers.UserControllers
{

    /* Oldal funkciók */
    /* Jelszó / azonosítóval való lekérés*/
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "Default")]
    public class UserController : ControllerBase
    {
        [HttpGet("SearchWithNameOrTitle")]
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
                    List<UserPostDtoToLike> userPostDtoToLikes = new List<UserPostDtoToLike>();
                    if (vankuk == true && nincskuk == false)
                    {
                        return Ok(context.Users.Where(x => x.Username.Contains(keresettErtek.TrimStart('@'))).ToList());
                    }
                    else if (vankuk == false && nincskuk == true)
                    {
                        var result = context.UserPosts.Include(x => x.User).Include(x => x.User!.Permission).Where(x => x.Title.Contains(keresettErtek)).ToList();
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
                            userPost.UserId = item.UserId;
                            userPost.Liked = (context.LikedPosts.FirstOrDefault(x => x.UserId == userId && x.PostId == userPost.Id) != null);
                            userPostDtoToLikes.Add(userPost);
                        }

                        return Ok(userPostDtoToLikes);
                    }
                    else if (vankuk == true && nincskuk == true)
                    {
                        var result = context.UserPosts.Include(x => x.User).Include(x => x.User!.Permission).Where(x => x.Title.ToLower().Contains(cim.ToLower().TrimStart())).ToList();
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
                            userPost.UserId = item.UserId;
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

        [HttpGet("SearchUser")]
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
        }

        [HttpGet("SearchPost")]
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

        [HttpGet("UserById")]
        public IActionResult UserById(int id)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var userById = context.Users.Include(x => x.Permission).Include(x => x.LikedPosts).FirstOrDefault(x => x.Id == id);
                    return Ok(userById);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpGet("UserAllAlertMessage")]
        public IActionResult UserAllAlertMessage(int userId)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var userAllAlertMessage = context.Alertmessages.Where(x => x.UserId == userId).ToList();
                    return Ok(userAllAlertMessage);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPut("ChangeUser")]
        [Authorize(Roles = "Default")]
        public IActionResult ChangeUser(UserPutDto userPutDto,int userId)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var user = context.Users.FirstOrDefault(x => x.Id == userId);
                    user!.Id = userId;
                    user.Username = userPutDto.UserName;
                    user.Email = userPutDto.Email;
                    context.Users.Update(user);
                    context.SaveChanges();
                    return Ok("Sikeres módosítás!");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}
