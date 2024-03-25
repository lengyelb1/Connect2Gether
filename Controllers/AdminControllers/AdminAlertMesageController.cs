using Connect2Gether_API.Models.Dtos;
using Connect2Gether_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Connect2Gether_API.Controllers.AdminControllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminAlertMesageController : ControllerBase
    {
        [HttpGet("AllAlertMessages")]
        public IActionResult GetAllAlertMessage()
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var messages = context.Alertmessages.ToList();
                    return Ok(messages);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpGet("AlertMessageById")]
        public IActionResult GetAlertMessageById(int id)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var messageById = context.Alertmessages.FirstOrDefault(x => x.Id == id);
                    return Ok(messageById);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPost("AddAlertMessage")]
        public IActionResult AddAlertMessage(AlertMessageDto alertMessageDto)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    Alertmessage alertmessage = new Alertmessage();
                    alertmessage.Title = alertMessageDto.title;
                    alertmessage.Description = alertMessageDto.description;
                    alertmessage.UserId = alertMessageDto.userId;
                    context.Alertmessages.Add(alertmessage);
                    context.SaveChanges();
                    return Ok("AlertMessage megadása!");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPut("ChangeAlertMessage")]
        public IActionResult ChangeAlertMessage(AlertMessageDto alertMessageDto, int id)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var changedMessage = context.Alertmessages.FirstOrDefault(x => x.Id == id);
                    changedMessage!.Title = alertMessageDto.title;
                    changedMessage.Description = alertMessageDto.description;
                    changedMessage.UserId = alertMessageDto.userId;
                    context.Alertmessages.Update(changedMessage);
                    context.SaveChanges();
                    return Ok("AlertMessage módosítása megtörtént!");
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
