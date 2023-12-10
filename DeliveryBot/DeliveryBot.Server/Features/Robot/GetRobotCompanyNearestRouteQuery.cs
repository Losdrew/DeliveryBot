using AutoMapper;
using DeliveryBot.Db.DbContexts;
using DeliveryBot.Server.Features.Base;
using DeliveryBot.Server.Services;
using DeliveryBot.Shared.Dto.Address;
using DeliveryBot.Shared.Dto.Geolocation;
using DeliveryBot.Shared.Dto.Robot;
using DeliveryBot.Shared.Errors.ServiceErrors;
using DeliveryBot.Shared.ServiceResponseHandling;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeliveryBot.Server.Features.Robot;

public class GetRobotCompanyNearestRouteQuery : IRequest<ServiceResponse<RoutesDto>>
{
    public string? DeviceId { get; set; }

    public class GetRobotCompanyNearestRouteQueryHandler : 
        ExtendedBaseHandler<GetRobotCompanyNearestRouteQuery, ServiceResponse<RoutesDto>>
    {
        private readonly IGeolocationService _geolocationService;

        public GetRobotCompanyNearestRouteQueryHandler(ApplicationDbContext context, IHttpContextAccessor contextAccessor,
            IMapper mapper, ILogger<GetRobotCompanyNearestRouteQueryHandler> logger, IGeolocationService geolocationService)
            : base(context, contextAccessor, mapper, logger)
        {
            _geolocationService = geolocationService;
        }

        public override async Task<ServiceResponse<RoutesDto>> Handle(GetRobotCompanyNearestRouteQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                return await UnsafeHandleAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Get robot company nearest route error");
                return ServiceResponseBuilder.Failure<RoutesDto>(RobotError.GetCompanyNearestRouteError);
            }
        }

        protected override async Task<ServiceResponse<RoutesDto>> UnsafeHandleAsync(GetRobotCompanyNearestRouteQuery request,
            CancellationToken cancellationToken)
        {
            var robot = Context.Robots.FirstOrDefault(r => r.DeviceId != null && r.DeviceId.Equals(request.DeviceId));

            if (robot == null)
            {
                return ServiceResponseBuilder.Failure<RoutesDto>(RobotError.RobotNotFound);
            }

            var company = Context.Companies
                .Include(c => c.CompanyAddresses)
                .FirstOrDefault(c => c.Id == robot.CompanyId);

            if (company == null)
            {
                return ServiceResponseBuilder.Failure<RoutesDto>(CompanyError.CompanyNotFound);
            }

            var tasks = company.CompanyAddresses.Select(async address =>
            {
                var addressDto = Mapper.Map<AddressDto>(address);
                return _geolocationService.GetAddressLocationAsync(addressDto).Result.Result;
            });
               
            var locations = await Task.WhenAll(tasks);

            var robotLocation = Mapper.Map<RobotInfoDto>(robot).Location;

            var distances = locations.Select(location => new
            {
                Location = location,
                Distance = _geolocationService.CalculateDistance(robotLocation, location)
            });

            // Find nearest location
            var nearestLocation = distances.OrderBy(d => d.Distance).First().Location;

            // Find route to nearest location
            var route = await _geolocationService.GetRoutesAsync(robotLocation, nearestLocation);
            return ServiceResponseBuilder.Success(route.Result);
        }
    }
}