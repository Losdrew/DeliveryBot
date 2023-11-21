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

public class CreateRobotCommand : CreateRobotCommandDto, IRequest<ServiceResponse<RobotDto>>
{
    public class CreateRobotCommandHandler :
        ExtendedBaseHandler<CreateRobotCommand, ServiceResponse<RobotDto>>
    {
        public CreateRobotCommandHandler(ApplicationDbContext context, IHttpContextAccessor contextAccessor,
            IMapper mapper, ILogger<CreateRobotCommandHandler> logger, IMediator mediator)
            : base(context, contextAccessor, mapper, logger)
        {
        }

        public override async Task<ServiceResponse<RobotDto>> Handle(CreateRobotCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                return await UnsafeHandleAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "Robot creation error");
                return ServiceResponseBuilder.Failure<RobotDto>(RobotError.RobotCreateError);
            }
        }

        protected override async Task<ServiceResponse<RobotDto>> UnsafeHandleAsync(CreateRobotCommand request,
            CancellationToken cancellationToken)
        {
            var isUserIdValid = ContextAccessor.TryGetUserId(out var userId);
            var administrator = await Context.FindAsync<CompanyEmployee>(userId);

            if (!isUserIdValid || administrator == null)
            {
                return ServiceResponseBuilder.Failure<RobotDto>(UserError.InvalidAuthorization);
            }

            var robot = Mapper.Map<Db.Models.Robot>(request);
            robot.CompanyId = administrator.CompanyId;

            Context.Add(robot);
            await Context.SaveChangesAsync(cancellationToken);

            var result = Mapper.Map<RobotDto>(robot);
            return ServiceResponseBuilder.Success(result);
        }
    }
}