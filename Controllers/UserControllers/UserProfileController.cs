using Connect2Gether_API.Models;
using Connect2Gether_API.Models.Dtos.UserDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Connect2Gether_API.Controllers.UserControllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        /*[HttpGet("UserProfile")]
        public IActionResult UserProfile(int id, UserProfileDto userProfileDto)
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var user = context.Users.FirstOrDefault(x => x.Id == id);
                    if (user == null) 
                    {
                        return StatusCode(404, "Nincs ilyen user");
                    }
                    else
                    {
                        user.Username = userProfileDto.UserName;
                        user.Email = userProfileDto.Email;
                        user.Point = userProfileDto.Points;
                        user.RankId = userProfileDto.RankId;
                        user.RegistrationDate = userProfileDto.RegistrationDate;
                        context.UserPosts.Where(x => x.UserId == user.Id).Count();
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
