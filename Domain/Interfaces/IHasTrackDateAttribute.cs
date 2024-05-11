namespace Domain.Interfaces;

public interface IHasTrackDateAttribute
{
    /// <summary>
    /// Дата модификации
    /// </summary>
    public DateTimeOffset DateModified { get; set; }

    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTimeOffset DateCreated { get; set; }
}