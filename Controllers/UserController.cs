﻿using Connect2Gether_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Connect2Gether_API.Controllers
{

    /* Oldal funkciók */
    /* Jelszó / azonosítóval való lekérés*/
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using (var context = new Connect2getherContext())
            {
                try
                {
                    var result = await context.Users.Include(f => f.UserPosts).Include(f => f.Comments).Include(f => f.Alertmessages).ToListAsync();
                    context.SaveChanges();
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}
