namespace BuildingBlocks.Exceptions;

/// <summary>
/// Represents an exception that is thrown when a bad request is encountered.
/// </summary>
public class BadRequestException : Exception
{
    /// <summary>
    /// Gets or sets the detailed information associated with the exception.
    /// This property provides additional context or descriptive details
    /// about the exception that occurred.
    /// </summary>
    public string Details { get; set; } = string.Empty;

    /// <summary>
    /// Represents an exception that is thrown when a bad request is encountered.
    /// </summary>
    public BadRequestException(string message) : base(message)
    {
    }

    /// <summary>
    /// Represents an exception that is thrown when a bad request is encountered.
    /// </summary>
    public BadRequestException(string message, string details) : base(message)
    {
        Details = details;
    }
}