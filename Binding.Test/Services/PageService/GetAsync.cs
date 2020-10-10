using System;
using System.Threading.Tasks;
using AutoMapper;
using Binding.Contexts;
using Binding.Mapping;
using Binding.Test.Seeds;
using Binding.Test.Setups;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Binding.Test.Services.PageService
{
    [TestFixture]
    public class GetAsync
    {
        private BindingContext _context;
        private IMapper _mapper;

        [SetUp]
        public async Task Setup()
        {
            _context = await new BasicSetup().Setup();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapping()));
            _mapper = new Mapper(configuration);
        }


        [TearDown]
        public async Task TearDown()
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.DisposeAsync();
        }

        [Test]
        public async Task ReturnsCorrectPage()
        {
            var pageService = new Binding.Services.PageService(_context,_mapper);

            var pageId = Guid.Parse("b1af5bdb-3771-4c17-baa4-346814bb6ecc");

            var page = await pageService.GetAsync(pageId);

            page.Id.Should().Be(pageId);
        }
        
        [Test]
        public async Task ShouldNotFindPage()
        {
            var pageService = new Binding.Services.PageService(_context,_mapper);

            var pageId = Guid.Empty;

            var page = await pageService.GetAsync(pageId);

            page.Should().BeNull();
        }
    }
}