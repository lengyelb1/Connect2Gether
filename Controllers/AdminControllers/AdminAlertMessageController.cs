﻿using Connect2Gether_API.Models.Dtos;
using Connect2Gether_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Connect2Gether_API.Controllers.AdminControllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminAlertMessageController : ControllerBase
    {
        [HttpGet("AllAlertMessage")]
        public IActionResult AllAlertMessage()
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var messages = context.Alertmessages.Include(x => x.User).ToList();
                    return Ok(messages);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpDelete("DeleteAlertMessage")]
        public IActionResult DeleteAlertMessage(int id)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var deletedMessage = context.Alertmessages.FirstOrDefault(x => x.Id == id);
                    context.Alertmessages.Remove(deletedMessage!);
                    context.SaveChanges();
                    return Ok("AlertMessage törlése sikeres!");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}
