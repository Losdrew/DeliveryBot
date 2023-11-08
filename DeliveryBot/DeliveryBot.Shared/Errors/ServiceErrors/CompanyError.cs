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
}