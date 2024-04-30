using Connect2Gether_API.Models;
using Connect2Gether_API.Models.Dtos;
using Connect2Gether_API.Models.Dtos.CommentDtos;
using Connect2Gether_API.Models.Dtos.UserDtos;
using Connect2Gether_API.Models.Dtos.UserPostDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Validations;
using MySqlX.XDevAPI.Common;
using System.Net.Mail;

namespace Connect2Gether_API.Controllers.AdminControllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminUsersController : ControllerBase
    {
        public static User user = new User();

        [HttpGet("UserCount")]
        public IActionResult UserCount()
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var db = context.Users.ToList().Count();
                    return Ok(db);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpGet("AllUser")]
        public async Task<IActionResult> AllUser()
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var result = await context.Users.Include(f => f.UserPosts).Include(f => f.Permission).Include(f => f.Comments).Include(f => f.Alertmessages).ToListAsync();
                    context.SaveChanges();
                    return Ok(result);
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
                    List<User> usersList = new List<User>();
                    var userById = context.Users.Include(x => x.UserPosts)!.ThenInclude(x => x.Comments)!.ThenInclude(x => x.User).Include(x => x.Rank).Include(x => x.Permission).Include(x => x.LikedPosts).FirstOrDefault(x => x.Id == id);
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
                        userByIdDto.Email = item.Email;
                        userByIdDto.Points = item.Point;
                        userByIdDto.PermissionId = item.PermissionId;
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
                                Dislike = pts.Dislike,
                                UserId = pts.UserId,
                                UserName = pts.User!.Username,
                                UploadDate = pts.UploadDate,
                                Comments = convertComments(pts.Comments!.ToList()),
                                Liked = (context.LikedPosts.FirstOrDefault(x => x.PostId == pts.Id) != null),
                                Disliked = (context.DislikedPosts.FirstOrDefault(x => x.Postid == pts.Id) != null)
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

        [HttpPost("SearchWithNameOrTitle")]
        public IActionResult SearchWithNameOrTitle([FromBody] SearchDto keresettErtek, int userId)
        {
            // Error 4001 nincs @ de van @
            using (var context = new Connect2getherContext())
            {
                try
                {
                    string[] strings = keresettErtek.searchValue!.Split(' ');
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
                        var outp = context.Users.Where(x => x.Username.Contains(keresettErtek.searchValue.TrimStart('@'))).ToList();
                        var simplifiedOutp = outp.Select(user => new
                        {
                            user.Id,
                            user.Username
                        }).ToList();

                        return Ok(simplifiedOutp.Count != 0 ? simplifiedOutp : null);
                    }
                    else if (vankuk == false && nincskuk == true)
                    {
                        var outp2 = context.UserPosts.Include(x => x.Comments).Include(x => x.User).Include(x => x.User!.Permission).Where(x => x.Title.Contains(keresettErtek.searchValue)).ToList();
                        foreach (var item in outp2)
                        {
                            UserPostResponseDto userPost = new UserPostResponseDto();
                            userPost.Id = item.Id;
                            userPost.Image = item.Image;
                            userPost.Description = item.Description;
                            userPost.Title = item.Title;
                            userPost.Like = item.Like;
                            userPost.Dislike = item.Dislike;
                            userPost.UserId = item.UserId;
                            userPost.UserName = item.User!.Username;
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
                            userPost.UploadDate = item.UploadDate;
                            userPost.Liked = (context.LikedPosts.FirstOrDefault(x => x.UserId == userId && x.PostId == userPost.Id) != null);
                            userPost.Disliked = (context.DislikedPosts.FirstOrDefault(x => x.Userid == userId && x.Postid == userPost.Id) != null);
                            userPostDtoToLikes.Add(userPost);
                        }

                        return Ok(userPostDtoToLikes.Count != 0 ? userPostDtoToLikes : null);
                    }
                    else if (vankuk == true && nincskuk == true)
                    {
                        var outp3 = context.UserPosts.Include(x => x.Comments).Include(x => x.User).Include(x => x.User!.Permission).Where(x => x.Title.ToLower().Contains(cim.ToLower().TrimStart())).ToList();
                        foreach (var item in outp3)
                        {
                            UserPostResponseDto userPost = new UserPostResponseDto();
                            userPost.Id = item.Id;
                            userPost.Image = item.Image;
                            userPost.Description = item.Description;
                            userPost.Title = item.Title;
                            userPost.Like = item.Like;
                            userPost.Dislike = item.Dislike;
                            userPost.UserId = item.UserId;
                            userPost.UserName = item.User!.Username;
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
                            userPost.UploadDate = item.UploadDate;
                            userPost.Liked = (context.LikedPosts.FirstOrDefault(x => x.UserId == userId && x.PostId == userPost.Id) != null);
                            userPost.Disliked = (context.DislikedPosts.FirstOrDefault(x => x.Userid == userId && x.Postid == userPost.Id) != null);
                            userPostDtoToLikes.Add(userPost);
                        }

                        return Ok(userPostDtoToLikes.Count != 0 ? userPostDtoToLikes : null);
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

        [HttpPost("EmailSender")]
        public IActionResult EmailSender(int id, string sender, EmailSenderDto emailSenderDto)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var user = context.Users.FirstOrDefault(x => x.Id == id);

                    MailMessage mail = new MailMessage();
                    SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
                    mail.From = new MailAddress("connectgether@gmail.com");
                    mail.To.Add(user!.Email!);
                    mail.Subject = $"{emailSenderDto.Subject}";
                    mail.Body = $"Kedves {user.Username}\n\n {emailSenderDto.Body} \n\nTisztelettel: {sender}";
                    smtpServer.Credentials = new System.Net.NetworkCredential("connectgether@gmail.com", "sdph etlk bmbw vopl");
                    smtpServer.Port = 587;
                    smtpServer.EnableSsl = true;
                    smtpServer.Send(mail);

                    return Ok("Email sent successfully!");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPost("RegisterWithOutCriterion")]
        public ActionResult<User> RegisterWithOutCriterion(RegistrationRequestDto registrationRequestDto)
        {
            using (var context = new Connect2getherContext())
            {
                Permission defaultPermission = new Permission();
                defaultPermission.Id = 1;
                defaultPermission.Name = "Default";
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(registrationRequestDto.Password, 4);
                User user = new User();
                user.Username = registrationRequestDto.UserName!;
                user.Hash = passwordHash;
                user.Email = registrationRequestDto.Email!;
                user.RegistrationDate = DateTime.Today;
                user.PermissionId = defaultPermission.Id;
                user.Permission = context.Permissions.FirstOrDefault((x) => x.Id == defaultPermission.Id && x.Name == defaultPermission.Name)!;

                if (context.Users.FirstOrDefault((x) => x.Username == user.Username) != null)
                {
                    return BadRequest("User existing!");
                }

                context.Users.Add(user);
                context.SaveChanges();
                return Ok("User added!");
            }
        }

        [HttpPut("ChangeRegisterById")]
        public ActionResult<User> ChangeRegisterById(AdminUserPutDto adminUserPutDto, int id)
        {
            using (var context = new Connect2getherContext())
            {
                var user = context.Users.FirstOrDefault((x) => x.Id == id);
                user!.Username = adminUserPutDto.UserName;
                user.Email = adminUserPutDto.Email;
                user.PermissionId = adminUserPutDto.permissionId;

                context.Users.Update(user);
                context.SaveChanges();
                return Ok("User changes!");
            }
        }

        [HttpDelete("DeleteUserById")]
        public ActionResult DeleteUserById(int id) 
        {
            using (var context = new Connect2getherContext())
            {
                var deleteUser = context.Users.FirstOrDefault((x) => x.Id == id);
                if (deleteUser == null)
                {
                    return BadRequest("This user does not exist!");
                }
                context.Users.Remove(deleteUser);
                context.SaveChanges();

                MailMessage mail = new MailMessage();
                SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("connectgether@gmail.com");
                mail.To.Add(deleteUser.Email);
                mail.Subject = "Figyelmeztetés!";
                mail.Body = $"Kedves Felhasználó!\n\nTájékoztatjuk, hogy fiókja törlésre került!";
                smtpServer.Credentials = new System.Net.NetworkCredential("connectgether@gmail.com", "sdph etlk bmbw vopl");
                smtpServer.Port = 587;
                smtpServer.EnableSsl = true;
                smtpServer.Send(mail);

                return Ok($"User deleted!");
            }
        }
    }
}
