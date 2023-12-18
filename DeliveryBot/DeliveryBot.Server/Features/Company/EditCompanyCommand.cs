using AutoMapper;
using DeliveryBot.Db.DbContexts;
using DeliveryBot.Db.Models;
using DeliveryBot.Server.Extensions;
using DeliveryBot.Server.Features.Account;
using DeliveryBot.Server.Features.Base;
using DeliveryBot.Server.Features.CompanyEmployee;
using DeliveryBot.Shared.Dto.Company;
using DeliveryBot.Shared.Errors.ServiceErrors;
using DeliveryBot.Shared.ServiceResponseHandling;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeliveryBot.Server.Features.Company;

public class EditCompanyCommand : EditCompanyCommandDto, IRequest<ServiceResponse<OwnCompanyInfoDto>>
{
    public class EditCompanyCommandHandler :
        ExtendedBaseHandler<EditCompanyCommand, ServiceResponse<OwnCompanyInfoDto>>
    {
        private readonly IMediator _mediator;

        public EditCompanyCommandHandler(ApplicationDbContext context, IHttpContextAccessor contextAccessor,
            IMapper mapper, ILogger<EditCompanyCommandHandler> logger, IMediator mediator)
            : base(context, contextAccessor, mapper, logger)
        {
            _mediator = mediator;
        }

        public override async Task<ServiceResponse<OwnCompanyInfoDto>> Handle(EditCompanyCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                return await UnsafeHandleAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "Edit company error");
                return ServiceResponseBuilder.Failure<OwnCompanyInfoDto>(CompanyError.CompanyEditError);
            }
        }

        protected override async Task<ServiceResponse<OwnCompanyInfoDto>> UnsafeHandleAsync(EditCompanyCommand request,
            CancellationToken cancellationToken)
        {
            var isUserIdValid = ContextAccessor.TryGetUserId(out var userId);
            var manager = await Context.CompanyEmployees.FindAsync(userId);

            if (!isUserIdValid || manager == null)
            {
                return ServiceResponseBuilder.Failure<OwnCompanyInfoDto>(UserError.InvalidAuthorization);
            }

            var companyToEdit = await Context.Companies
                .Include(c => c.CompanyAddresses)
                .Include(c => c.CompanyEmployees)
                .FirstOrDefaultAsync(c => c.Id == manager.CompanyId, cancellationToken);

            if (companyToEdit == null)
            {
                return ServiceResponseBuilder.Failure<OwnCompanyInfoDto>(CompanyError.CompanyNotFound);
            }

            if (request.CompanyEmployees != null)
            {
                var editCompanyEmployeesResponse =
                    await EditCompanyEmployees(request, companyToEdit, cancellationToken);

                if (!editCompanyEmployeesResponse.IsSuccess)
                {
                    return editCompanyEmployeesResponse.MapErrorResult<OwnCompanyInfoDto>();
                }
            }

            if (request.CompanyAddresses != null)
            {
                await EditCompanyAddresses(request, companyToEdit);
            }

            Mapper.Map(request, companyToEdit);

            await Context.SaveChangesAsync(cancellationToken);

            var result = Mapper.Map<OwnCompanyInfoDto>(companyToEdit);
            return ServiceResponseBuilder.Success(result);
        }

        private async Task<ServiceResponse> EditCompanyEmployees(EditCompanyCommand request, Db.Models.Company company,
            CancellationToken cancellationToken)
        {
            foreach (var companyEmployee in request.CompanyEmployees)
            {
                var isExistingEmployee = company.CompanyEmployees.Any(c => c.Id == companyEmployee.Id);

                if (isExistingEmployee)
                {
                    var editCompanyEmployeeCommand = new EditCompanyEmployeeCommand
                    {
                        Id = companyEmployee.Id,
                        Email = companyEmployee.Email,
                        FirstName = companyEmployee.FirstName,
                        LastName = companyEmployee.LastName
                    };

                    var editCompanyEmployeeResponse =
                        await _mediator.Send(editCompanyEmployeeCommand, cancellationToken);

                    if (!editCompanyEmployeeResponse.IsSuccess)
                    {
                        return editCompanyEmployeeResponse.MapErrorResult();
                    }
                }
                else
                {
                    var createCompanyEmployeeCommand = new CreateCompanyEmployeeCommand
                    {
                        Email = companyEmployee.Email,
                        FirstName = companyEmployee.FirstName,
                        LastName = companyEmployee.LastName,
                        CompanyId = company.Id
                    };

                    var createCompanyEmployeeResponse =
                        await _mediator.Send(createCompanyEmployeeCommand, cancellationToken);

                    if (!createCompanyEmployeeResponse.IsSuccess)
                    {
                        return createCompanyEmployeeResponse.MapErrorResult();
                    }
                }
            }

            return ServiceResponseBuilder.Success();
        }

        private async Task EditCompanyAddresses(EditCompanyCommand request, Db.Models.Company company)
        {
            foreach (var companyAddress in request.CompanyAddresses)
            {
                var existingAddress = company.CompanyAddresses.FirstOrDefault(a => a.Id == companyAddress.Id);

                if (existingAddress != null)
                {
                    Mapper.Map(companyAddress, existingAddress);
                }
                else
                {
                    var newAddress = Mapper.Map<Address>(companyAddress);
                    company.CompanyAddresses.Add(newAddress);
                }
            }
        }
    }
}