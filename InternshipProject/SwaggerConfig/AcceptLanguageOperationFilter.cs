using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace InternshipProject.SwaggerConfig {
    public class AcceptLanguageOperationFilter : IOperationFilter {
        public void Apply(OpenApiOperation operation, OperationFilterContext context) {

            operation.Parameters ??= new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter {
                Name = "Accept-Language",
                Description = "Language preference for the response.",
                In = ParameterLocation.Header,
                Required = true,
                Schema = new OpenApiSchema {
                    Type = "string",
                    Default = new OpenApiString("en-US")
                },
            });
        }
    }
}
