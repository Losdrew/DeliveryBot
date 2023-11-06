using Microsoft.AspNetCore.Identity;

namespace DeliveryBot.Server.Services;

public interface ITokenGenerator
{
    public Task<string> GenerateAsync(IdentityUser user);
}