using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TestMVC.Filters;

public class ModelFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext actionContext)
    {
        if (actionContext.ModelState.IsValid) return;
        actionContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        var errorMessage = actionContext.ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .FirstOrDefault(s => !string.IsNullOrEmpty(s));
        
        if (actionContext.Controller is Controller controller)
        {
            controller.TempData["ErrorMessage"] = errorMessage;
        }

        var returnUrl = actionContext.HttpContext.Request.Headers.Referer.ToString();
        actionContext.Result = new RedirectResult(returnUrl);
    }
}