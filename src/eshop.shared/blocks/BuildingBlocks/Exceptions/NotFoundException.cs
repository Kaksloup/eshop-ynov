namespace BuildingBlocks.Exceptions;

/// <summary>
/// Represents an exception that is thrown when an entity cannot be found.
/// This exception is typically used to indicate that the requested resource
/// or data does not exist in the system or database.
/// </summary>
public class NotFoundException : BusinessException
{
    /// <summary>
    /// Represents an exception that is thrown when an entity cannot be found.
    /// This exception is typically used to indicate that the requested resource
    /// or data does not exist in the system or database.
    /// </summary>
    public NotFoundException(string message) : base(message)
    {
    }

    /// <summary>
    /// Represents an exception that is thrown when an entity cannot be found.
    /// This exception is typically used to indicate that the requested resource
    /// or data does not exist in the system or database.
    /// </summary>
    public NotFoundException(string entity, object key) : base($"Aucun {entity} ne correspond à cet identifiant : '{key}'")
    {
    }
}