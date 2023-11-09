using AutoMapper;
using DeliveryBot.Db.DbContexts;
using DeliveryBot.Db.Models;
using DeliveryBot.Server.Extensions;
using DeliveryBot.Server.Features.Account;
using DeliveryBot.Server.Features.Base;
using DeliveryBot.Shared.Dto.Company;
using DeliveryBot.Shared.Errors.ServiceErrors;
using DeliveryBot.Shared.ServiceResponseHandling;
using MediatR;

namespace DeliveryBot.Server.Features.Company;

public class CreateCompanyCommand : CompanyDto, IRequest<ServiceResponse<OwnCompanyInfoDto>>
{
    public class CreateCompanyCommandHandler :
        ExtendedBaseHandler<CreateCompanyCommand, ServiceResponse<OwnCompanyInfoDto>>
    {
        private readonly IMediator _mediator;

        public CreateCompanyCommandHandler(ApplicationDbContext context, IHttpContextAccessor contextAccessor,
            IMapper mapper, ILogger<CreateCompanyCommandHandler> logger, IMediator mediator)
            : base(context, contextAccessor, mapper, logger)
        {
            _mediator = mediator;
        }

        public override async Task<ServiceResponse<OwnCompanyInfoDto>> Handle(CreateCompanyCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                return await UnsafeHandleAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "Company creation error");
                return ServiceResponseBuilder.Failure<OwnCompanyInfoDto>(CompanyError.CompanyCreateError);
            }
        }

        protected override async Task<ServiceResponse<OwnCompanyInfoDto>> UnsafeHandleAsync(CreateCompanyCommand request,
            CancellationToken cancellationToken)
        {
            var isUserIdValid = ContextAccessor.TryGetUserId(out var userId);
            var manager = await Context.FindAsync<CompanyEmployee>(userId);

            if (!isUserIdValid || manager == null)
            {
                return ServiceResponseBuilder.Failure<OwnCompanyInfoDto>(UserError.InvalidAuthorization);
            }

            var newCompany = Mapper.Map<Db.Models.Company>(request);
            Context.Add(newCompany);
            await Context.SaveChangesAsync(cancellationToken);

            if (request.CompanyEmployees != null)
            {
                // Create company employees as identity users
                foreach (var companyEmployee in request.CompanyEmployees)
                {
                    var createCompanyEmployeeCommand = new CreateCompanyEmployeeCommand
                    {
                        Email = companyEmployee.Email,
                        Password = companyEmployee.Password,
                        FirstName = companyEmployee.FirstName,
                        LastName = companyEmployee.LastName,
                        CompanyId = newCompany.Id
                    };

                    var createCompanyEmployeeResponse = await _mediator.Send(createCompanyEmployeeCommand, cancellationToken);

                    if (!createCompanyEmployeeResponse.IsSuccess)
                    {
                        return createCompanyEmployeeResponse.MapErrorResult<OwnCompanyInfoDto>();
                    }
                }
            }

            manager.CompanyId = newCompany.Id;
            Context.Update(manager);
            await Context.SaveChangesAsync(cancellationToken);

            var result = Mapper.Map<OwnCompanyInfoDto>(newCompany);
            return ServiceResponseBuilder.Success(result);
        }
    }
}