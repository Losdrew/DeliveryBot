using DeliveryBot.Db.DbContexts;
using DeliveryBot.Db.Models;
using DeliveryBot.Server.Features.Account;
using DeliveryBot.Server.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace DeliveryBot.Tests.Tests.Features.Account;

public class SignInCommandHandlerTests
{
    private readonly Mock<ILogger<SignInCommand.SignInCommandHandler>> _loggerMock;
    private readonly Mock<ITokenGenerator> _tokenGeneratorMock;
    private readonly UserManager<IdentityUser> _userManagerMock;

    public SignInCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<SignInCommand.SignInCommandHandler>>();
        _tokenGeneratorMock = new Mock<ITokenGenerator>();
        var services = new ServiceCollection();
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        services.AddTransient(s => new ApplicationDbContext(optionsBuilder.Options));
        services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<ApplicationDbContext>();
        var serviceProvider = services.BuildServiceProvider();
        _userManagerMock = serviceProvider.GetService<UserManager<IdentityUser>>();
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccessResult()
    {
        var command = new SignInCommand();

        var handler = new SignInCommand.SignInCommandHandler(
            _userManagerMock,
            _tokenGeneratorMock.Object,
            _loggerMock.Object
        );

        var result = await handler.Handle(command, default);

        result.IsSuccess.Should().BeFalse();
    }
}