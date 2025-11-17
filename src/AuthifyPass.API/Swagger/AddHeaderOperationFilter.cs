using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AuthifyPass.API.Swagger;

internal class AddHeaderOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "x-authify-key",
            In = ParameterLocation.Header,
            Description = "Shared secret for authentication",
            Required = false,
            Schema = new OpenApiSchema { Type = JsonSchemaType.String }
        });
    }
}
