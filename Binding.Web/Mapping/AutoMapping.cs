using System.Collections.Generic;
using AutoMapper;
using Binding.Models;

namespace Binding.Mapping
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Page, PageWithNoBlocksViewModel>();
            CreateMap<Block, BlockViewModel>();
            // CreateMap<IList<Page>, IList<PageWithNoBlocksViewModel>>();
        }
    }
}