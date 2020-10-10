using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Binding.Contexts;

namespace Binding.Models
{
    public class PageMethods
    {
        public PageWithNoBlocksViewModel FixChildren(Page page, BindingContext bindingContext, IMapper mapper)
        {
            var pageWithChildren = page.RecC(page, bindingContext);
            return new PageWithNoBlocksViewModel()
            {
                Id = pageWithChildren.Id,
                Created = pageWithChildren.Created,
                Name = pageWithChildren.Name,
                Updated = pageWithChildren.Updated,
                Order = pageWithChildren.Order,
                Children = mapper.Map<IList<PageWithNoBlocksViewModel>>(pageWithChildren.Children)
            };
        }
        
        public Page RecC(Page page, BindingContext bindingContext)
        {
            foreach (var child in page.Children)
            {
                if (child == null)
                {
                    return null;
                }
                
                bindingContext.Entry(child).Collection(x => x.Children).Load();
                RecC(child,bindingContext);
            }
            
            if (page.Children == null)
            {
                return null;
            }
        
            return page;
        }
        
        // public PageWithNoBlocksViewModel RecCView(PageWithNoBlocksViewModel page, BindingContext bindingContext)
        // {
        //     foreach (var child in page.Children)
        //     {
        //         if (child == null)
        //         {
        //             return null;
        //         }
        //         
        //         bindingContext.Entry(child).Collection(x => x.Children).Load();
        //         RecCView(child,bindingContext);
        //     }
        //     
        //     if (page.Children == null)
        //     {
        //         return null;
        //     }
        //
        //     return page;
        // }
    }
    
    public class Page : PageMethods
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        
        public int Order { get; set; }
        
        public User Owner { get; set; }
        
        // many blocks to one page
        public IList<Block> Blocks { get; set; }
        
        // many children to one parent page
        [AllowNull]
        public IList<Page> Children { get; set; }
        
        // one parent to many page
        [AllowNull]
        public Page Parent { get; set; }
    }

    public class PageWithNoBlocksViewModel : PageMethods
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        
        public int Order { get; set; }
        
        public IList<PageWithNoBlocksViewModel> Children { get; set; }
    }
    
    public class PageWithBlocksViewModel : PageMethods
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        
        public int Order { get; set; }
        
        public IList<PageWithNoBlocksViewModel> Children { get; set; }
        
        public IList<BlockViewModel> Blocks { get; set; }
        
        public PageWithNoBlocksViewModel Parent { get; set; }
    }
}