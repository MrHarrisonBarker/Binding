using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Binding.Contexts;
using Binding.Models;
using Microsoft.EntityFrameworkCore;

namespace Binding.Services
{
    public interface IPageService
    {
        Task<Page> CreateAsync(Page page, Guid userId);
        Task<PageWithBlocksViewModel> GetAsync(Guid id);
        Task<Page> UpdateAsync(Page page);
        Task<bool> DeleteAsync(Guid id);
    }

    public class PageService : IPageService
    {
        private readonly BindingContext _bindingContext;
        private readonly IMapper _mapper;

        public PageService(BindingContext bindingContext, IMapper mapper)
        {
            _bindingContext = bindingContext;
            _mapper = mapper;
        }

        public async Task<Page> CreateAsync(Page page, Guid userId)
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
                    var parent = await _bindingContext.Pages.Include(x => x.Children).FirstOrDefaultAsync(x => x.Id == page.Parent.Id);
                    if (parent == null)
                    {
                        Console.WriteLine("Parent not found");
                        return null;
                    }

                    if (parent.Children == null)
                    {
                        parent.Children = new List<Page>();
                    }

                    page.Order = parent.Children.Count;
                    parent.Children.Add(page);
                    parent.Updated = DateTime.Now;
                    page.Parent = null;
                }
                else
                {
                    var pagesWithNoParents = _bindingContext.Pages.Where(x => x.Parent == null);
                    page.Order = pagesWithNoParents.Count();
                }

                page.Updated = DateTime.Now;
                page.Created = DateTime.Now;
                await _bindingContext.Pages.AddAsync(page);

                if (user.Pages == null)
                {
                    user.Pages = new List<Page>();
                }

                user.Pages.Add(page);
                if (page.Children != null)
                {
                    foreach (var child in page.Children) user.Pages.Add(child);
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

        public async Task<PageWithBlocksViewModel> GetAsync(Guid id)
        {
            // when getting page, get page with blocks and minimal children and minimal parent just for reference

            var page = await _bindingContext.Pages.Include(x => x.Children).Include(x => x.Blocks).FirstOrDefaultAsync(x => x.Id == id);

            if (page == null)
            {
                throw new Exception("Page not found");
            }
            
            page = page.RecC(page, _bindingContext);
            
            var newPage = new PageWithBlocksViewModel()
            {
                Id = page.Id,
                Name = page.Name,
                Created = page.Created,
                Updated = page.Updated,
                Order = page.Order,
                Blocks = page.Blocks.Select(block => _mapper.Map<BlockViewModel>(block)).ToList(),
                Children = _mapper.Map<IList<PageWithNoBlocksViewModel>>(page.Children),
                Parent = _mapper.Map<PageWithNoBlocksViewModel>(page.Parent)
            };

            // var page = await _bindingContext.Pages.Select(x => new PageWithBlocksViewModel()
            // {
            //     Id = x.Id,
            //     Name = x.Name,
            //     Created = x.Created,
            //     Updated = x.Updated,
            //     Order = x.Order,
            //     Children = _mapper.Map<IList<PageWithNoBlocksViewModel>>(x.Children),
            //     Blocks = x.Blocks.Select(block => _mapper.Map<BlockViewModel>(block)).ToList(),
            //     Parent = _mapper.Map<PageWithNoBlocksViewModel>(x.Parent)
            // }).FirstOrDefaultAsync(x => x.Id == id);

            // x.Childern.Select(child => _mapper.Map<PageWithNoBlocksViewModel>(child)).ToList()

            return newPage;

            // return await _bindingContext.Pages.FirstOrDefaultAsync(x => x.Id == id);
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
                if (await GetAsync(page.Id) == null)
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