using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Binding.Contexts;
using Binding.Models;
using Microsoft.EntityFrameworkCore;

namespace Binding.Services
{
    public interface IPageService
    {
        Task<Page> CreateAsync(Page page, Guid userId);
        Task<Page> GetAsync(Guid id);
        Task<Page> UpdateAsync(Page page);
        Task<bool> DeleteAsync(Guid id);
    }

    public class PageService : IPageService
    {
        private readonly BindingContext _bindingContext;

        public PageService(BindingContext bindingContext)
        {
            _bindingContext = bindingContext;
        }

        public async Task<Page> CreateAsync(Page page,Guid userId)
        {
            // TODO: This won't make children only parents
            
            var user = await _bindingContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                Console.WriteLine("User not found");
                return null;
            }

            try
            {
                if (page.Parent != null)
                {
                    var parent = await _bindingContext.Pages.FirstOrDefaultAsync(x => x.Id == page.Parent.Id);
                    if (parent == null)
                    {
                        Console.WriteLine("Parent not found");
                        return null;
                    }

                    if (parent.Childern == null)
                    {
                        parent.Childern = new List<Page>();
                    }
                    parent.Childern.Add(page);
                    parent.Updated = DateTime.Now;
                    page.Parent = null;
                }
                
                page.Updated = DateTime.Now;
                page.Created = DateTime.Now;
                await _bindingContext.Pages.AddAsync(page);

                if (user.Pages == null)
                {
                    user.Pages = new List<Page>();
                }
                user.Pages.Add(page);
                if (page.Childern != null)
                {
                    foreach (var child in page.Childern) user.Pages.Add(child);
                }
                user.Updated = DateTime.Now;

                await _bindingContext.SaveChangesAsync();
                return page;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
                // return false;
            }
        }

        public async Task<Page> GetAsync(Guid id)
        {
            return await _bindingContext.Pages
                .Include(x => x.Childern)
                .Include(x => x.Blocks)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Page> UpdateAsync(Page page)
        {
            
            _bindingContext.Entry(page).State = EntityState.Modified;
            
            try
            {
                page.Updated = DateTime.Now;
                await _bindingContext.SaveChangesAsync();
                return page;
            }
            catch (DbUpdateConcurrencyException)
            {
                if ((await GetAsync(page.Id) == null))
                {
                    throw new Exception("Page not found");
                }
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var page = await _bindingContext.Pages.Include(x => x.Blocks).FirstOrDefaultAsync(x => x.Id == id);

            if (page == null)
            {
                Console.WriteLine("Cant find Page");
                return false;
            }

            // var parent = await _bindingContext.Pages.FirstOrDefaultAsync(x => x.Id == page.Parent.Id);
            // var children = await _bindingContext.Pages.

            try
            {
                if (page.Blocks != null)
                {
                    _bindingContext.Blocks.RemoveRange(page.Blocks);
                }

                _bindingContext.Pages.Remove(page);
                
                
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