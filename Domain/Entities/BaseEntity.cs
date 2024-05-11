using System.ComponentModel.DataAnnotations;
using Domain.CustomAttributes;

namespace Domain.Entities;

public abstract class BaseEntity<T>
{
    /// <summary>
    /// Идентификатор сущности
    /// </summary>
    public T Id { get; set; } = default!;
}