using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace TestMVC.Filters;


public class ExceptionFilter : IExceptionFilter
{
    private readonly ITempDataDictionaryFactory _tempData;
    
    
    // ReSharper disable once ConvertToPrimaryConstructor
    public ExceptionFilter(ITempDataDictionaryFactory tempData)
    {
        _tempData = tempData;
    }

    public void OnException(ExceptionContext context)
    {
        var exception = context.Exception;
        var exceptionMessage = exception.Message;

        // Access TempData from the controller context
        var tempData = _tempData.GetTempData(context.HttpContext);
        tempData["ErrorMessage"] = exceptionMessage;

        var returnUrl = context.HttpContext.Request.Headers.Referer.ToString();
        context.Result = new RedirectResult(returnUrl);
        context.ExceptionHandled = true;
    }
}