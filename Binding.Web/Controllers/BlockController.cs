using System;
using System.Threading.Tasks;
using Binding.Models;
using Binding.Services;
using Microsoft.AspNetCore.Mvc;

namespace Binding.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlockController : ControllerBase
    {
        private readonly IBlockService _blockService;

        public BlockController(IBlockService blockService)
        {
            _blockService = blockService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Block>> Get(Guid id)
        {
            var block = await _blockService.GetAsync(id);
            if (block == null)
            {
                return BadRequest("Couldn't find block");
            }

            return Ok(block);
        }

        [HttpPost]
        public async Task<ActionResult<Block>> Create([FromBody] CreateBlock createBlock)
        {
            var block = await _blockService.CreateAsync(createBlock.Block, createBlock.PageId);
            if (block == null)
            {
                return BadRequest("Block is fucked");
            }

            return Ok(block);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(Guid id)
        {
            var hasDeleted = await _blockService.DeleteAsync(id);
            if (hasDeleted)
            {
                return Ok(true);
            }
            else
            {
                return BadRequest(false);
            }
        }
    }

    public class DeleteBlock
    {
        public Guid BlockId { get; set; }
        public Guid PageId { get; set; }
    }

    public class CreateBlock
    {
        public Block Block { get; set; }
        public Guid PageId { get; set; }
    }
}