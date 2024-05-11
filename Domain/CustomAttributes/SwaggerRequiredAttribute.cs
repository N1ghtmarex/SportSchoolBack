namespace Domain.CustomAttributes;

/// <summary>
/// Атрибут для пометки свойства обязательным при отрисовке сваггером
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class SwaggerRequiredAttribute : Attribute
{
}