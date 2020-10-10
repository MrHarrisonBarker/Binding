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
    public class Authenticate
    {
        private BindingContext _context;
        private IMapper _mapper;
        private IOptions<AppSettings> _appSettings;

        [SetUp]
        public async Task Setup()
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapping()));
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
        public async Task ShouldAuthenticate()
        {
            var userService = new Binding.Services.UserService(_context, _mapper, _appSettings);

            var hasAuthenticated = await userService.Authenticate("harrison@thebarkers.me.uk",
                "Password");

            hasAuthenticated.Should().NotBeNull();
        }

        [Test]
        public async Task ShouldNotAuthenticate()
        {
            var userService = new Binding.Services.UserService(_context, _mapper, _appSettings);

            // FluentActions.Invoking(async () =>
            //         await userService.Authenticate("harrison@thebarkers.me.uk", "Wrong password"))
            //     .Should().Throw<Exception>()
            //     .WithMessage("Password incorrect");

            Func<Task> act = async () =>
            {
                await userService.Authenticate("harrison@thebarkers.me.uk", "Wrong password");
            };
            await act.Should().ThrowAsync<Exception>().WithMessage("Password incorrect");
        }

        [Test]
        public async Task ShouldNotFindUser()
        {
            var userService = new Binding.Services.UserService(_context, _mapper, _appSettings);
            
            // "User not found"
            Func<Task> act = async () =>
            {
                await userService.Authenticate("wrong email", "Wrong password");
            };
            await act.Should().ThrowAsync<Exception>().WithMessage("User not found");
        }
    }
}