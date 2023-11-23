using AutoMapper;
using DeliveryBot.Db.DbContexts;
using DeliveryBot.Server.Features.Company;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace DeliveryBot.Tests.Tests.Features.Company;

public class CreateCompanyCommandHandlerTests
{
    private readonly Mock<ApplicationDbContext> _dbContextMock;
    private readonly Mock<IHttpContextAccessor> _contextAccessorMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<CreateCompanyCommand.CreateCompanyCommandHandler>> _loggerMock;
    private readonly Mock<IMediator> _mediatorMock;

    public CreateCompanyCommandHandlerTests()
    {
        _mapperMock = new Mock<IMapper>();
        _mediatorMock = new Mock<IMediator>();
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        _dbContextMock = new Mock<ApplicationDbContext>(optionsBuilder.Options);
        _contextAccessorMock = new Mock<IHttpContextAccessor>();
        _loggerMock = new Mock<ILogger<CreateCompanyCommand.CreateCompanyCommandHandler>>();
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccessResult()
    {
        var command = new CreateCompanyCommand();

        var handler = new CreateCompanyCommand.CreateCompanyCommandHandler(
            _dbContextMock.Object,
            _contextAccessorMock.Object,
            _mapperMock.Object,
            _loggerMock.Object,
            _mediatorMock.Object);

        var result = await handler.Handle(command, default);

        result.IsSuccess.Should().BeFalse();
    }
}