using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Binding.Contexts;
using Binding.Models;
using Microsoft.EntityFrameworkCore;

namespace Binding.Services
{
    public interface IBlockService
    {
        Task<Block> CreateAsync(Block block, Guid pageId);
        Task<Block> GetAsync(Guid id);
        Task<Block> UpdateAsync(Block block);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ReOrderAsync(Guid swapThis, Guid forThat);
    }

    public class BlockService : IBlockService
    {
        private readonly BindingContext _bindingContext;

        public BlockService(BindingContext bindingContext)
        {
            _bindingContext = bindingContext;
        }

        public async Task<Block> CreateAsync(Block block, Guid pageId)
        {
            var page = await _bindingContext.Pages.Include(x => x.Blocks).FirstOrDefaultAsync(x => x.Id == pageId);

            if (page == null)
            {
                Console.WriteLine("Cant find the fucking page");
                return null;
            }

            try
            {
                block.Created = DateTime.Now;
                block.Updated = DateTime.Now;
                block.Page = null;

                if (page.Blocks == null)
                {
                    page.Blocks = new List<Block>();
                }

                block.Order = page.Blocks.Count;

                await _bindingContext.Blocks.AddAsync(block);
                
                page.Blocks.Add(block);
                page.Updated = DateTime.Now;

                await _bindingContext.SaveChangesAsync();
                return block;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Block> GetAsync(Guid id)
        {
            return await _bindingContext.Blocks.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Block> UpdateAsync(Block block)
        {
            
            // var realBlock = await _bindingContext.Blocks.FirstOrDefaultAsync(x => x.Id == block.Id);
            //
            // if (realBlock == null)
            // {
            //     Console.WriteLine("Block not found");
            //     return null;
            // }

            _bindingContext.Entry(block).State = EntityState.Modified;
            
            try
            {
                block.Updated = DateTime.Now;
                await _bindingContext.SaveChangesAsync();
                return block;
            }
            catch (DbUpdateConcurrencyException)
            {
                if ((await GetAsync(block.Id) == null))
                {
                    throw new Exception("Block not found");
                }
                throw new Exception("Database error");
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var block = await _bindingContext.Blocks.Include(x => x.Page).FirstOrDefaultAsync(x => x.Id == id);

            if (block == null)
            {
                Console.WriteLine("Cant find the fucking block");
                return false;
            }

            var page = await _bindingContext.Pages.FirstOrDefaultAsync(x => x.Id == block.Page.Id);

            // if (page == null)
            // {
            //     Console.WriteLine("Cant find the fucking page");
            //     return false;
            // }
            
            try
            {
                page.Blocks.Remove(block);
                _bindingContext.Blocks.Remove(block);
                await _bindingContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> ReOrderAsync(Guid swapThis, Guid forThat)
        {
            // TODO: change to single db call?
            var swap = await _bindingContext.Blocks.FirstOrDefaultAsync(x => x.Id == swapThis);
            var that = await _bindingContext.Blocks.FirstOrDefaultAsync(x => x.Id == forThat);

            if (swap == null || that == null)
            {
                throw new Exception("All block not found");
            }

            try
            {
                var swapOrder = swap.Order;
                swap.Order = that.Order;
                that.Order = swapOrder;

                await _bindingContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}