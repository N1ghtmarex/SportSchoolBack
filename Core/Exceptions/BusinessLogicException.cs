using System.Globalization;

namespace Core.Exceptions;

/// <summary>
/// Exception thrown when a business logic error occurs. 4xx http status code.
/// </summary>
[Serializable]
public class BusinessLogicException : Exception
{
    public BusinessLogicException() : base() { }

    public BusinessLogicException(string message) : base(message) { }

    public BusinessLogicException(string message, Exception innerException) : base(message, innerException) { }

    public BusinessLogicException(string message, params object[] args)
        : base(string.Format(CultureInfo.CurrentCulture, message, args)) { }
}

