using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WMS_API.Access_Layers.Attributes
{
    public class ApiKeyAuthAttribute : Attribute, IAsyncActionFilter
    {
        private const string ApiKeyHeaderName = "Api-Key";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var apiKey = configuration.GetValue<string>("ApiKey");

            if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedKey)) 
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "API Key was not provided."
                };
                return;
            }

            if (!apiKey.Equals(extractedKey)) 
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 403,
                    Content = "API Key was not valid"
                };
                return;
            }

            await next();
        }
    }
}
