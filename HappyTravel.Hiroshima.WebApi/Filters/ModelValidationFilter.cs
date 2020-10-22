using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HappyTravel.Hiroshima.WebApi.Filters
{
    public class ModelValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.ModelState.IsValid)
                filterContext.Result = new BadRequestObjectResult(filterContext.ModelState);
        }


        public void OnActionExecuted(ActionExecutedContext context)
        { }
    }
}