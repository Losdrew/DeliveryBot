using AutoMapper;
using DeliveryBot.Db.DbContexts;
using DeliveryBot.Server.Extensions;
using DeliveryBot.Server.Features.Base;
using DeliveryBot.Shared.Dto.CompanyEmployee;
using DeliveryBot.Shared.Errors.ServiceErrors;
using DeliveryBot.Shared.ServiceResponseHandling;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace DeliveryBot.Server.Features.CompanyEmployee;

public class EditCompanyEmployeeCommand : EditCompanyEmployeeCommandDto,
    IRequest<ServiceResponse<CompanyEmployeeInfoDto>>
{
    public class EditCompanyEmployeeCommandHandler :
        ExtendedBaseHandler<EditCompanyEmployeeCommand, ServiceResponse<CompanyEmployeeInfoDto>>
    {
        private readonly UserManager<IdentityUser> _userManager;

        public EditCompanyEmployeeCommandHandler(ApplicationDbContext context, IHttpContextAccessor contextAccessor,
            IMapper mapper, ILogger<EditCompanyEmployeeCommandHandler> logger, UserManager<IdentityUser> userManager)
            : base(context, contextAccessor, mapper, logger)
        {
            _userManager = userManager;
        }

        public override async Task<ServiceResponse<CompanyEmployeeInfoDto>> Handle(
            EditCompanyEmployeeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await UnsafeHandleAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "Edit company employee error");
                var error = CompanyEmployeeError.CompanyEmployeeEditError;
                return ServiceResponseBuilder.Failure<CompanyEmployeeInfoDto>(error);
            }
        }

        protected override async Task<ServiceResponse<CompanyEmployeeInfoDto>> UnsafeHandleAsync(
            EditCompanyEmployeeCommand request, CancellationToken cancellationToken)
        {
            var isUserIdValid = ContextAccessor.TryGetUserId(out var userId);
            var manager = await Context.CompanyEmployees.FindAsync(userId);

            if (!isUserIdValid || manager == null)
            {
                return ServiceResponseBuilder.Failure<CompanyEmployeeInfoDto>(UserError.InvalidAuthorization);
            }

            var companyEmployeeToEdit = await Context.CompanyEmployees.FindAsync(request.Id);

            if (companyEmployeeToEdit == null)
            {
                var error = CompanyEmployeeError.CompanyEmployeeNotFound;
                return ServiceResponseBuilder.Failure<CompanyEmployeeInfoDto>(error);
            }

            if (companyEmployeeToEdit.CompanyId != manager.CompanyId)
            {
                return ServiceResponseBuilder.Failure<CompanyEmployeeInfoDto>(UserError.ForbiddenAccess);
            }

            await UpdateUserCredentialsAsync(request);

            Mapper.Map(request, companyEmployeeToEdit);
            await Context.SaveChangesAsync(cancellationToken);

            var result = Mapper.Map<CompanyEmployeeInfoDto>(companyEmployeeToEdit);
            return ServiceResponseBuilder.Success(result);
        }

        private async Task UpdateUserCredentialsAsync(EditCompanyEmployeeCommand request)
        {
            var userToEdit = await _userManager.FindByIdAsync(request.Id.ToString());

            if (request.Password != null)
            {
                var passwordToken = await _userManager.GeneratePasswordResetTokenAsync(userToEdit);
                await _userManager.ResetPasswordAsync(userToEdit, passwordToken, request.Password);
            }

            if (request.Email != null)
            {
                var emailToken = await _userManager.GenerateChangeEmailTokenAsync(userToEdit, request.Email);
                await _userManager.ChangeEmailAsync(userToEdit, request.Email, emailToken);
            }
        }
    }
}