using System.Security.Claims;

namespace DeliveryBot.Server.Extensions
{
    public static class HttpContextExtensions
    {
        public static bool TryGetUserId(this IHttpContextAccessor contextAccessor, out Guid userId)
        {
            return contextAccessor.HttpContext.TryGetUserId(out userId);
        }

        public static bool TryGetUserId(this HttpContext? context, out Guid userId)
        {
            var identityClaim = context?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (identityClaim != null && !string.IsNullOrEmpty(identityClaim.Value))
            {
                var isValid = Guid.TryParse(identityClaim.Value, out Guid parsedUserId);

                if (isValid && parsedUserId != Guid.Empty)
                {
                    userId = parsedUserId;
                    return true;
                }
            }

            userId = Guid.Empty;
            return false;
        }
    }
}