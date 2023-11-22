using DeliveryBot.Shared.Errors.Base;

namespace DeliveryBot.Shared.Errors.ServiceErrors;

public class CompanyEmployeeError : ServiceError
{
    public static CompanyEmployeeError CompanyEmployeeEditError = new()
    {
        Header = "Edit company employee error",
        ErrorMessage = "Error when editing company",
        Code = 1
    };

    public static CompanyEmployeeError CompanyEmployeeNotFound = new()
    {
        Header = "Company employee not found",
        ErrorMessage = "Company employee not found",
        Code = 2
    };
}