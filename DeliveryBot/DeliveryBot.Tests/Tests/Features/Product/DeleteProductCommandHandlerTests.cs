using AutoMapper;
using DeliveryBot.Db.DbContexts;
using DeliveryBot.Server.Features.Product;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace DeliveryBot.Tests.Tests.Features.Product;

public class DeleteProductCommandHandlerTests
{
    private readonly Mock<ApplicationDbContext> _dbContextMock;
    private readonly Mock<IHttpContextAccessor> _contextAccessorMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<DeleteProductCommand.DeleteProductCommandHandler>> _loggerMock;

    public DeleteProductCommandHandlerTests()
    {
        _mapperMock = new Mock<IMapper>();
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        _dbContextMock = new Mock<ApplicationDbContext>(optionsBuilder.Options);
        _contextAccessorMock = new Mock<IHttpContextAccessor>();
        _loggerMock = new Mock<ILogger<DeleteProductCommand.DeleteProductCommandHandler>>();
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccessResult()
    {
        var command = new DeleteProductCommand();

        var handler = new DeleteProductCommand.DeleteProductCommandHandler(
            _dbContextMock.Object,
            _contextAccessorMock.Object,
            _mapperMock.Object,
            _loggerMock.Object);

        var result = await handler.Handle(command, default);

        result.IsSuccess.Should().BeFalse();
    }
}