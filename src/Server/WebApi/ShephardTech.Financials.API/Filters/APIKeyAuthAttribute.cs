using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using ShephardTech.Financials.Common;

namespace ShephardTech.Financials.API.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class APIKeyAuthAttribute : Attribute, IAsyncActionFilter
    {

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var _logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<APIKeyAuthAttribute>>();

            var HeaderKey = AppConstants.Settings.APIKEY_KEY_VALUE;

            

            if (!context.HttpContext.Request.Headers.TryGetValue(AppConstants.Settings.APIKEY_KEY_NAME, out var potentialApiKey))
            {
                var errorMessage = $"{AppConstants.Settings.APIKEY_KEY_NAME} is missing in the request header";

                _logger.LogError(errorMessage);

                context.Result = new UnauthorizedResult();

                return;
            }

            if (!potentialApiKey.Equals(HeaderKey))
            {
                var errorMessage = $"Wrong {AppConstants.Settings.APIKEY_KEY_NAME}";

                _logger.LogError(errorMessage);

                context.Result = new UnauthorizedResult();

                return;
            }


            await next();
        }

    }
}
