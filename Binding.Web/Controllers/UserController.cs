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
        private readonly IUserService<User> _userService;
        
        public UserController(IUserService<User> userService)
        {
            _userService = userService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IList<User>>> GetAll()
        {
            return await _userService.GetAllAsync();
        }
        
    }
}