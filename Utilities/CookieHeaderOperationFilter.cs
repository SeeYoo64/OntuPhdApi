using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class CookieHeaderOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= new List<OpenApiParameter>();

        // Добавляем параметр Cookie для эндпоинтов, зависящих от сессии
        if (context.ApiDescription.HttpMethod != "OPTIONS" &&
            context.ApiDescription.RelativePath.Contains("auth/session") ||
            context.ApiDescription.RelativePath.Contains("auth/signout"))
        {
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Cookie",
                In = ParameterLocation.Header,
                Description = "auth.session-token=<your-session-token>",
                Required = false,
                Schema = new OpenApiSchema { Type = "string" }
            });
        }
    }
}