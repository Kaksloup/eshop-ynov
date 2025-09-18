namespace BuildingBlocks.Exceptions;

/// <summary>
/// Represents a custom exception type used to handle business logic-related errors
/// specific to the application's domain. This exception is typically thrown when
/// a business rule is violated or an operation cannot be completed due to logical constraints.
/// </summary>
public class BusinessException : Exception
{
    /// <summary>
    /// Represents a custom exception type used to handle business logic-related errors
    /// specific to the application's domain. This exception is typically thrown when
    /// a business rule is violated or an operation cannot be completed due to logical constraints.
    /// </summary>
    protected BusinessException(string message) : base(message)
    {
    }
}