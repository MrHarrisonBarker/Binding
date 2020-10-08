using System;
using System.Threading.Tasks;
using Binding.Models;
using Binding.Services;
using Microsoft.AspNetCore.Mvc;

namespace Binding.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PageController : ControllerBase
    {
        private readonly IPageService _pageService;

        public PageController(IPageService pageService)
        {
            _pageService = pageService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Page>> Get(Guid id)
        {
            var page = await _pageService.GetAsync(id);
            if (page == null)
            {
                return BadRequest("Couldn't find page");
            }

            return Ok(page);
        }

        [HttpPost]
        public async Task<ActionResult<Page>> Create([FromBody] CreatePage createPage)
        {
            var page = await _pageService.CreateAsync(createPage.Page, createPage.UserId);
            if (page == null)
            {
                return BadRequest("Fucked up, no user found?");
            }

            return Ok(page);
        }
    }

    public class CreatePage
    {
        public Page Page { get; set; }
        public Guid UserId { get; set; }
    }
}