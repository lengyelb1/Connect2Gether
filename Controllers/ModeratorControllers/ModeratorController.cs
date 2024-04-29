﻿using Connect2Gether_API.Models;
using Connect2Gether_API.Models.Dtos;
using Connect2Gether_API.Models.Dtos.UserSuspiciousDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Validations;
using System.Net.Mail;

namespace Connect2Gether_API.Controllers.ModeratorControllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "Moderator, Admin")]
    public class ModeratorController : ControllerBase
    {
        [HttpGet("AllSuspiciousUser")]
        public IActionResult AllSuspiciousUser()
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var request = context.UserSuspicious.Include(x => x.User).ToList();
                    var simplifiedRequest = request.Select(userSuspicious => new
                    {
                        userSuspicious.Id,
                        userSuspicious.UserId,
                        User = userSuspicious.User != null ? new { userSuspicious.User.Username } : null,

                    }).ToList();
                    return Ok(simplifiedRequest);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpGet("SuspiciousUserById")]
        public IActionResult SuspiciousUserById(int id) 
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    List<UserSuspiciou> userSuspiciousList = new List<UserSuspiciou>();
                    var request = context.UserSuspicious.Include(x => x.User).FirstOrDefault(x => x.Id == id);
                    userSuspiciousList.Add(request!);
                    var simplifiedRequest = userSuspiciousList.Select(userSuspicious => new
                    {
                        userSuspicious.Id,
                        userSuspicious.UserId,
                        User = userSuspicious.User != null ? new { userSuspicious.User.Username } : null,

                    }).ToList();
                    return Ok(simplifiedRequest);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPost("AddSuspicious")]
        public IActionResult AddSuspicious(int id, UserSuspiciousDto userSuspiciousDto)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var user = context.Users.FirstOrDefault(x => x.Id == id);
                    if (user == null)
                    {
                        return BadRequest("This user does not exist!");
                    }
                    else
                    {
                        context.UserSuspicious.Add(new UserSuspiciou {
                            UserId = user.Id, 
                            Message = userSuspiciousDto.Message, 
                            Description = userSuspiciousDto.Descrpition 
                        });

                        MailMessage mail = new MailMessage();
                        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
                        mail.From = new MailAddress("connectgether@gmail.com");
                        mail.To.Add(user.Email!);
                        mail.Subject = $"{userSuspiciousDto.Subject}";
                        mail.Body = $"Kedves {user.Username}!\n{userSuspiciousDto.Message}\nÜdv,{userSuspiciousDto.Sender}";
                        smtpServer.Credentials = new System.Net.NetworkCredential("connectgether@gmail.com", "sdph etlk bmbw vopl");
                        smtpServer.Port = 587;
                        smtpServer.EnableSsl = true;
                        smtpServer.Send(mail);

                        context.Alertmessages.Add(new Alertmessage
                        {
                            UserId = user.Id,
                            Title = userSuspiciousDto.Subject,
                            Description = userSuspiciousDto.Descrpition
                        });

                        context.SaveChanges();

                        return Ok("Added successfully!");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpDelete("DeleteSuspiciousById")]
        public ActionResult DeleteSuspiciousById(int id)
        {
            using (var context = new Connect2getherContext())
            {
                var suspiciousUser = context.UserSuspicious.FirstOrDefault(x => x.Id == id);
                context.UserSuspicious.Remove(suspiciousUser!);
                context.SaveChanges();
                return Ok($"User removed suspicious!");
            }
        }
    }
}
