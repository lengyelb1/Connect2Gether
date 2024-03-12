using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Connect2Gether_API.Controllers.ModeratorControllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "Moderator")]
    public class ModeratorUsersController : ControllerBase
    {

    }
}
