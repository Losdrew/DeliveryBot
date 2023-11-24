using AutoMapper;
using DeliveryBot.Db.DbContexts;
using DeliveryBot.Server.Features.Base;
using DeliveryBot.Shared.Errors.ServiceErrors;
using DeliveryBot.Shared.ServiceResponseHandling;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DeliveryBot.Server.Features.CompanyEmployee;

public class DeleteCompanyEmployeeCommand : IRequest<ServiceResponse>
{
    public Guid CompanyEmployeeId { get; set; }

    public class DeleteCompanyEmployeeCommandHandler :
        ExtendedBaseHandler<DeleteCompanyEmployeeCommand, ServiceResponse>
    {
        private readonly UserManager<IdentityUser> _userManager;

        public DeleteCompanyEmployeeCommandHandler(ApplicationDbContext context, IHttpContextAccessor contextAccessor,
            IMapper mapper, ILogger<DeleteCompanyEmployeeCommandHandler> logger, UserManager<IdentityUser> userManager)
            : base(context, contextAccessor, mapper, logger)
        {
            _userManager = userManager;
        }

        public override async Task<ServiceResponse> Handle(DeleteCompanyEmployeeCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                return await UnsafeHandleAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "Delete company employee error");
                return ServiceResponseBuilder.Failure(CompanyEmployeeError.CompanyEmployeeDeleteError);
            }
        }

        protected override async Task<ServiceResponse> UnsafeHandleAsync(DeleteCompanyEmployeeCommand request,
            CancellationToken cancellationToken)
        {
            var companyEmployeeToDelete = Context.CompanyEmployees.Where(p => p.Id == request.CompanyEmployeeId);
            var userToDelete = await _userManager.FindByIdAsync(request.CompanyEmployeeId.ToString());

            var isCompanyEmployeeDeleted = await companyEmployeeToDelete.ExecuteDeleteAsync(cancellationToken);
            var userDeleteResult = await _userManager.DeleteAsync(userToDelete);

            if (isCompanyEmployeeDeleted == 0 || !userDeleteResult.Succeeded)
            {
                return ServiceResponseBuilder.Failure(CompanyEmployeeError.CompanyEmployeeNotFound);
            }

            return ServiceResponseBuilder.Success();
        }
    }
}