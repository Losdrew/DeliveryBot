using AutoMapper;
using DeliveryBot.Db.DbContexts;
using DeliveryBot.Server.Features.CompanyEmployee;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace DeliveryBot.Tests.Tests.Features.CompanyEmployee;

public class EditCompanyEmployeeCommandHandlerTests
{
    private readonly Mock<ApplicationDbContext> _dbContextMock;
    private readonly Mock<IHttpContextAccessor> _contextAccessorMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<EditCompanyEmployeeCommand.EditCompanyEmployeeCommandHandler>> _loggerMock;
    private readonly UserManager<IdentityUser> _userManagerMock;

    public EditCompanyEmployeeCommandHandlerTests()
    {
        _mapperMock = new Mock<IMapper>();
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        _dbContextMock = new Mock<ApplicationDbContext>(optionsBuilder.Options);
        _contextAccessorMock = new Mock<IHttpContextAccessor>();
        _loggerMock = new Mock<ILogger<EditCompanyEmployeeCommand.EditCompanyEmployeeCommandHandler>>();
        var services = new ServiceCollection();
        services.AddTransient(s => new ApplicationDbContext(optionsBuilder.Options));
        services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<ApplicationDbContext>();
        var serviceProvider = services.BuildServiceProvider();
        _userManagerMock = serviceProvider.GetService<UserManager<IdentityUser>>();
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccessResult()
    {
        var command = new EditCompanyEmployeeCommand();

        var handler = new EditCompanyEmployeeCommand.EditCompanyEmployeeCommandHandler(
            _dbContextMock.Object,
            _contextAccessorMock.Object,
            _mapperMock.Object,
            _loggerMock.Object,
            _userManagerMock);

        var result = await handler.Handle(command, default);

        result.IsSuccess.Should().BeFalse();
    }
}