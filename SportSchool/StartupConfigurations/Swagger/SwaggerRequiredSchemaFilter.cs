using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SportSchool.StartupConfigurations.Swagger;

public class SwaggerRequiredSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema.Properties != null)
        {
            foreach (var schemaProperty in schema.Properties)
            {
                if (!schemaProperty.Value.Nullable)
                {
                    schema.Required.Add(schemaProperty.Key);
                }
            }
        }
    }
}