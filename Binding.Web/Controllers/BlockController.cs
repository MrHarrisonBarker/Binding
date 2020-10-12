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

        [HttpPut]
        public async Task<ActionResult<Block>> Update([FromBody] Block block)
        {
            var updatedBlock = await _blockService.UpdateAsync(block);
            if (updatedBlock != null)
            {
                return Ok(updatedBlock);
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(Guid id)
        {
            var hasDeleted = await _blockService.DeleteAsync(id);
            if (hasDeleted)
            {
                return Ok(true);
            }

            return BadRequest(false);
        }

        [HttpPut("reorder")]
        public async Task<ActionResult<bool>> ReOrder([FromBody] SwapBlock swapBlock)
        {
            var hasSwapped = await _blockService.ReOrderAsync(swapBlock.SwapThis, swapBlock.ForThat);
            if (hasSwapped)
            {
                return Ok(true);
            }

            return BadRequest(false);
        }
    }

    public class SwapBlock
    {
        public Guid SwapThis { get; set; }
        public Guid ForThat { get; set; }
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