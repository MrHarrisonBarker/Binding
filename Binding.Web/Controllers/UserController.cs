using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Binding.Models;
using Binding.Services;
using Microsoft.AspNetCore.Mvc;

namespace Binding.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IList<User>>> GetAll()
        {
            return await _userService.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserViewModel>> Get(Guid id)
        {    
            return await _userService.GetAsync(id);
        }

        [HttpGet("authenticate")]
        public async Task<ActionResult<UserViewModel>> Authenticate(string email, string password)
        {
            return await _userService.Authenticate(email, password);
        }
        
        [HttpPost]
        public async Task<ActionResult<Block>> Create([FromBody] User user)
        {
            var block = await _userService.CreateAsync(user);
            if (block == null)
            {
                return BadRequest("User is fucked");
            }

            return Ok(block);
        }
    }
}