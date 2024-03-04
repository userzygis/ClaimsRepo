using Claims.Domain.ActionModels.Responses;
using Claims.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Text.Json;

namespace Claims.Api.Middlewares
{
    public class CustomClaimsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public CustomClaimsMiddleware(RequestDelegate next, ILogger<CustomClaimsMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                LogAction(httpContext);
                await _next(httpContext).ConfigureAwait(false);
            }
            catch (ClaimsValidationException ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandleValidationExceptionAsync(httpContext, ex).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(httpContext, ex).ConfigureAwait(false);
            }
        }

        private void LogAction(HttpContext httpContext)
        {
            var endPoint = httpContext.GetEndpoint();
            if (endPoint != null)
            {
                var controllerActionDescriptor = endPoint.Metadata.GetMetadata<ControllerActionDescriptor>();
                if(controllerActionDescriptor != null)
                {
                    var controllerName = controllerActionDescriptor.ControllerName;
                    var actionName = controllerActionDescriptor.ActionName;
                    _logger.LogInformation($"Executing action: {controllerName}->{actionName}");
                }
            }
        }

        private Task HandleValidationExceptionAsync(HttpContext context, ClaimsValidationException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            var result = new ValidationErrorResponse
            {
                Error = exception.Message
            };

            return context.Response.WriteAsync(ConvertToJson(result));
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var resp = new InternalServerErrorResponse()
            {
                Error = "Terrible exception occurred"
            };
            return context.Response.WriteAsync(ConvertToJson(resp));
        }

        private string ConvertToJson<T>(T result)
        {
            var jsonResp = JsonSerializer.Serialize(result, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            });
            return jsonResp;
        }
    }
}
