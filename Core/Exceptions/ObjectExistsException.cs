using System.Globalization;

namespace Core.Exceptions;

/// <summary>
/// Thrown when an object already exists. 403 http status code.
/// </summary>
[Serializable]
public class ObjectExistsException : Exception
{
    public ObjectExistsException() : base() { }

    public ObjectExistsException(string message) : base(message) { }

    public ObjectExistsException(string message, Exception innerException) : base(message, innerException) { }

    public ObjectExistsException(string message, params object[] args)
        : base(string.Format(CultureInfo.CurrentCulture, message, args)) { }
}

