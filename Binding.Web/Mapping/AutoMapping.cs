using AutoMapper;
using Binding.Models;

namespace Binding.Mapping
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Page, PageViewModel>();
        }
    }
}