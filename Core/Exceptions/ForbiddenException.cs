using System.Globalization;

namespace Core.Exceptions;

/// <summary>
/// Exception thrown when a forbidden operation is attempted. 401 http status code.
/// </summary>
[Serializable]
public class ForbiddenException : Exception
{
    public ForbiddenException() : base() { }

    public ForbiddenException(string message) : base(message) { }

    public ForbiddenException(string message, Exception innerException) : base(message, innerException) { }

    public ForbiddenException(string message, params object[] args)
        : base(string.Format(CultureInfo.CurrentCulture, message, args)) { }
}

