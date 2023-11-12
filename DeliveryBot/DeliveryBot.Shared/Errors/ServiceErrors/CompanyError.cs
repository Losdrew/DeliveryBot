using DeliveryBot.Shared.Errors.Base;

namespace DeliveryBot.Shared.Errors.ServiceErrors;

public class CompanyError : ServiceError
{
    public static CompanyError CompanyCreateError = new()
    {
        Header = "Company creation error",
        ErrorMessage = "Error when creating company",
        Code = 1
    };

    public static CompanyError GetCompaniesError = new()
    {
        Header = "Get companies error",
        ErrorMessage = "Error when getting a list of companies",
        Code = 2
    };

    public static CompanyError GetOwnCompanyError = new()
    {
        Header = "Get own company error",
        ErrorMessage = "Error when getting user's company",
        Code = 3
    };

    public static CompanyError CompanyNotFound = new()
    {
        Header = "Company not found",
        ErrorMessage = "Company not found",
        Code = 4
    };
}