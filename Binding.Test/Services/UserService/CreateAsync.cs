using System;
using System.Threading.Tasks;
using AutoMapper;
using Binding.Contexts;
using Binding.Mapping;
using Binding.Models;
using Binding.Test.Setups;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace Binding.Test.Services.UserService
{
    [TestFixture]
    public class CreateAsync
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
        public async Task ShouldCreateUniqueUser()
        {
            var userService = new Binding.Services.UserService(_context,_mapper,_appSettings);

            var user = new User
            {
                Email = "test@test.co.uk",
                Password = "Password",
                DisplayName = "Test User"
            };

            var hasCreated = await userService.CreateAsync(user);
            hasCreated.Should().NotBeNull();

            // Func<Task> act = async () => { await userService.CreateAsync(user); };
            // await act.Should().NotThrowAsync<Exception>();
            // act.Should().NotBeNull();
        }

        [Test]
        public async Task ShouldFailNonUniqueUser()
        {
            var userService = new Binding.Services.UserService(_context,_mapper,_appSettings);

            var user = new User
            {
                Email = "harrison@thebarkers.me.uk",
                Password = "Fail",
                DisplayName = "Fail Test User"
            };
            
            FluentActions.Invoking(async () => await userService.CreateAsync(user))
                .Should().Throw<Exception>()
                .WithMessage("User already exists");
        }
    }
}