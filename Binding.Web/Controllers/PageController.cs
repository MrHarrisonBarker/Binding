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
        
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(Guid id)
        {
            var hasDeleted = await _pageService.DeleteAsync(id);
            if (hasDeleted)
            {
                return Ok(true);
            }
            else
            {
                return BadRequest(false);
            }
        }
        
        [HttpPut]
        public async Task<ActionResult<Page>> Update([FromBody] Page page)
        {
            var updatedPage = await _pageService.UpdateAsync(page);
            if (updatedPage != null)
            {
                return Ok(updatedPage);
            }

            return BadRequest();
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