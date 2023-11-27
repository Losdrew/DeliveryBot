﻿using AutoMapper;
using DeliveryBot.Db.DbContexts;
using DeliveryBot.Db.Models;
using DeliveryBot.Server.Extensions;
using DeliveryBot.Server.Features.Base;
using DeliveryBot.Shared.Dto.Robot;
using DeliveryBot.Shared.Errors.ServiceErrors;
using DeliveryBot.Shared.ServiceResponseHandling;
using MediatR;

namespace DeliveryBot.Server.Features.Robot;

public class SetRobotStatusCommand : SetRobotStatusCommandDto, IRequest<ServiceResponse<RobotInfoDto>>
{
    public class SetRobotStatusCommandHandler :
        ExtendedBaseHandler<SetRobotStatusCommand, ServiceResponse<RobotInfoDto>>
    {
        public SetRobotStatusCommandHandler(ApplicationDbContext context, IHttpContextAccessor contextAccessor,
            IMapper mapper, ILogger<SetRobotStatusCommandHandler> logger)
            : base(context, contextAccessor, mapper, logger)
        {
        }

        public override async Task<ServiceResponse<RobotInfoDto>> Handle(SetRobotStatusCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                return await UnsafeHandleAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "Set robot status error");
                return ServiceResponseBuilder.Failure<RobotInfoDto>(RobotError.RobotEditError);
            }
        }

        protected override async Task<ServiceResponse<RobotInfoDto>> UnsafeHandleAsync(SetRobotStatusCommand request,
            CancellationToken cancellationToken)
        {
            var isUserIdValid = ContextAccessor.TryGetUserId(out var userId);
            var administrator = await Context.CompanyEmployees.FindAsync(userId);

            if (!isUserIdValid || administrator == null)
            {
                return ServiceResponseBuilder.Failure<RobotInfoDto>(UserError.InvalidAuthorization);
            }

            var robotToEdit = await Context.Robots.FindAsync(request.Id);

            if (robotToEdit == null)
            {
                return ServiceResponseBuilder.Failure<RobotInfoDto>(RobotError.RobotNotFound);
            }

            if (robotToEdit.CompanyId != administrator.CompanyId)
            {
                return ServiceResponseBuilder.Failure<RobotInfoDto>(UserError.ForbiddenAccess);
            }

            robotToEdit.Status = (RobotStatus)request.NewStatus;
            await Context.SaveChangesAsync(cancellationToken);

            var result = Mapper.Map<RobotInfoDto>(robotToEdit);
            return ServiceResponseBuilder.Success(result);
        }
    }
}