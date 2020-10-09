using System;
using System.Threading.Tasks;
using AutoMapper;
using Binding.Contexts;
using Binding.Mapping;
using Binding.Test.Setups;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace Binding.Test.Services.UserService
{
    [TestFixture]
    public class GetAsync
    {
        private BindingContext _context;
        private IMapper _mapper;
        private IOptions<AppSettings> _appSettings;

        [SetUp]
        public async Task Setup()
        {
            var myProfile = new AutoMapping();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            _mapper = new Mapper(configuration);
            
            _context = await new BasicSetup().Setup();
            
            _appSettings = Options.Create(new AppSettings()
            {
                Secret = "this is a secret"
            });
        }

        [TearDown]
        public async Task TearDown()
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.DisposeAsync();
        }

        [Test]
        public async Task ShouldGetUserWithOnlyParentPages()
        {
            var userService = new Binding.Services.UserService(_context,_mapper,_appSettings);
            var user = await userService.GetAsync(new Guid("c385730a-91a5-429f-9c0c-b3e9bd4e5788"));

            user.Should().NotBeNull();
        }
    }
}