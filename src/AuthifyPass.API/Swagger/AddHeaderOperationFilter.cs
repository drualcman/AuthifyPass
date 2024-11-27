using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AuthifyPass.API.Swagger;

internal class AddHeaderOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Agregar un encabezado específico a todas las operaciones
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "x-authify-key", // El nombre del encabezado
            In = ParameterLocation.Header,
            Description = "Header for authentication",
            Required = false, // Puedes configurarlo como obligatorio según tu lógica
            Schema = new OpenApiSchema { Type = "string" }
        });
    }
}
