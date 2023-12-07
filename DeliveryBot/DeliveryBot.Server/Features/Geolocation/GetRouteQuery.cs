using AutoMapper;
using DeliveryBot.Db.DbContexts;
using DeliveryBot.Server.Features.Base;
using DeliveryBot.Server.Services;
using DeliveryBot.Shared.Dto.Geolocation;
using DeliveryBot.Shared.Errors.ServiceErrors;
using DeliveryBot.Shared.ServiceResponseHandling;
using MediatR;

namespace DeliveryBot.Server.Features.Geolocation;

public class GetRouteQuery : GetRouteQueryDto, IRequest<ServiceResponse<RoutesDto>>
{
    public class GetRouteQueryHandler : ExtendedBaseHandler<GetRouteQuery, ServiceResponse<RoutesDto>>
    {
        private readonly IGeolocationService _geolocationService;

        public GetRouteQueryHandler(ApplicationDbContext context, IHttpContextAccessor contextAccessor,
            IMapper mapper, ILogger<GetRouteQueryHandler> logger, IGeolocationService geolocationService)
            : base(context, contextAccessor, mapper, logger)
        {
            _geolocationService = geolocationService;
        }

        public override async Task<ServiceResponse<RoutesDto>> Handle(GetRouteQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                return await UnsafeHandleAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Get route error");
                return ServiceResponseBuilder.Failure<RoutesDto>(GeolocationError.GetRouteError);
            }
        }

        protected override async Task<ServiceResponse<RoutesDto>> UnsafeHandleAsync(GetRouteQuery request,
            CancellationToken cancellationToken)
        {
            var route = await _geolocationService.GetRoutesAsync(request.FirstPoint, request.SecondPoint);

            if (!route.IsSuccess)
            {
                return ServiceResponseBuilder.Failure<RoutesDto>(route.Error);
            }

            return ServiceResponseBuilder.Success(route.Result);
        }
    }
}