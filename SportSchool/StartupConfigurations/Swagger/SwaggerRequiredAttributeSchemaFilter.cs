using System.Reflection;
using Domain.CustomAttributes;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SportSchool.StartupConfigurations.Swagger;

public class SwaggerRequiredAttributeSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema.Properties == null) return;
        var properties = context.Type.GetProperties();
        foreach (var property in properties)
        {
            var attribute = property.GetCustomAttribute(typeof(SwaggerRequiredAttribute));

            if (attribute == null) continue;
            var propertyNameInCamelCasing =
                char.ToLowerInvariant(property.Name[0]) + property.Name.Substring(1);

            if (schema.Required == null)
            {
                schema.Required = (ISet<string>?)new List<string>()
                {
                    propertyNameInCamelCasing
                }.AsEnumerable();
            }
            else
            {
                schema.Required.Add(propertyNameInCamelCasing);
            }
        }
    }
}