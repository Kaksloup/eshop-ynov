namespace BuildingBlocks.Exceptions;

/// <summary>
/// Represents an exception that occurs during internal server errors.
/// This exception is used to indicate that an unexpected error has occurred
/// on the server-side, preventing the successful completion of a request or action.
/// </summary>
public class InternalServerException : Exception
{
    /// <summary>
    /// Gets or sets additional details about the internal server error.
    /// This property provides descriptive information that can assist in diagnosing
    /// and troubleshooting the issue encountered on the server-side.
    /// </summary>
    public string Details { get; set; } = string.Empty;

    /// <summary>
    /// Represents an exception that occurs during internal server errors.
    /// This exception is used to indicate that an unexpected error has occurred
    /// on the server-side, preventing the successful completion of a request or action.
    /// </summary>
    public InternalServerException(string message) : base(message)
    {
    }

    /// <summary>
    /// Represents an exception that occurs during internal server errors.
    /// This exception is used to indicate that an unexpected error has occurred
    /// on the server-side, preventing the successful completion of a request or action.
    /// </summary>
    public InternalServerException(string message, string details) : base(message)
    {
        Details = details;       
    }
}