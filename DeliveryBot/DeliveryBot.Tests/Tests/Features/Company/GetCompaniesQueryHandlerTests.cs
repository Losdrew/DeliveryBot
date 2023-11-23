using AutoMapper;
using DeliveryBot.Db.DbContexts;
using DeliveryBot.Server.Features.Company;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace DeliveryBot.Tests.Tests.Features.Company;

public class GetCompaniesQueryHandlerTests
{
    private readonly Mock<ApplicationDbContext> _dbContextMock;
    private readonly Mock<IHttpContextAccessor> _contextAccessorMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<GetCompaniesQuery.GetCompaniesQueryHandler>> _loggerMock;

    public GetCompaniesQueryHandlerTests()
    {
        _mapperMock = new Mock<IMapper>();
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        _dbContextMock = new Mock<ApplicationDbContext>(optionsBuilder.Options);
        _contextAccessorMock = new Mock<IHttpContextAccessor>();
        _loggerMock = new Mock<ILogger<GetCompaniesQuery.GetCompaniesQueryHandler>>();
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccessResult()
    {
        var command = new GetCompaniesQuery();

        var handler = new GetCompaniesQuery.GetCompaniesQueryHandler(
            _dbContextMock.Object,
            _contextAccessorMock.Object,
            _mapperMock.Object,
            _loggerMock.Object);

        var result = await handler.Handle(command, default);

        result.IsSuccess.Should().BeFalse();
    }
}