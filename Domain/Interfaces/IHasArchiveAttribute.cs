namespace Domain.Interfaces;

public interface IHasArchiveAttribute
{
    /// <summary>
    /// Статус архивности
    /// </summary>
    public bool IsArchive { get; set; }
}