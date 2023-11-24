using Microsoft.AspNetCore.Mvc.Filters;

namespace DeliveryBot.Server.Extensions;

public class FileDownloadAttribute : ActionFilterAttribute
{
    public string FileName { get; set; }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        context.HttpContext.Response.Headers.Add("Content-Disposition", $"attachment; filename={FileName}");
    }
}