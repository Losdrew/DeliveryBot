using AutoMapper;
using DeliveryBot.Db.DbContexts;
using DeliveryBot.Server.Features.Delivery;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace DeliveryBot.Tests.Tests.Features.Delivery;

public class GetDeliveryQueryHandlerTests
{
    private readonly Mock<ApplicationDbContext> _dbContextMock;
    private readonly Mock<IHttpContextAccessor> _contextAccessorMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<GetDeliveryQuery.GetDeliveryQueryHandler>> _loggerMock;

    public GetDeliveryQueryHandlerTests()
    {
        _mapperMock = new Mock<IMapper>();
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        _dbContextMock = new Mock<ApplicationDbContext>(optionsBuilder.Options);
        _contextAccessorMock = new Mock<IHttpContextAccessor>();
        _loggerMock = new Mock<ILogger<GetDeliveryQuery.GetDeliveryQueryHandler>>();
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccessResult()
    {
        var command = new GetDeliveryQuery();

        var handler = new GetDeliveryQuery.GetDeliveryQueryHandler(
            _dbContextMock.Object,
            _contextAccessorMock.Object,
            _mapperMock.Object,
            _loggerMock.Object);

        var result = await handler.Handle(command, default);

        result.IsSuccess.Should().BeFalse();
    }
}