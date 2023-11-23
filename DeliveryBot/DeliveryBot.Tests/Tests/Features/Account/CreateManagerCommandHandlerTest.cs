using DeliveryBot.Db.DbContexts;
using DeliveryBot.Server.Features.Account;
using DeliveryBot.Server.Services;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace DeliveryBot.Tests.Tests.Features.Account;

public class CreateManagerCommandHandlerTests
{
    private readonly Mock<ApplicationDbContext> _dbContextMock;
    private readonly Mock<ILogger<CreateManagerCommand.CreateManagerCommandHandler>> _loggerMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<ITokenGenerator> _tokenGeneratorMock;

    public CreateManagerCommandHandlerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        _dbContextMock = new Mock<ApplicationDbContext>(optionsBuilder.Options);
        _loggerMock = new Mock<ILogger<CreateManagerCommand.CreateManagerCommandHandler>>();
        _tokenGeneratorMock = new Mock<ITokenGenerator>();
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccessResult()
    {
        var command = new CreateManagerCommand();

        var handler = new CreateManagerCommand.CreateManagerCommandHandler(
            _mediatorMock.Object,
            _dbContextMock.Object,
            _tokenGeneratorMock.Object,
            _loggerMock.Object
        );

        var result = await handler.Handle(command, default);

        result.IsSuccess.Should().BeFalse();
    }
}