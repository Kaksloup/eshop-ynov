namespace BuildingBlocks.Exceptions;

/// <summary>
/// Represents an exception that is thrown when attempting to create or add
/// an entity that already exists in the datastore or system. This is typically
/// used to enforce uniqueness constraints or prevent duplication of data.
/// </summary>
public class AlreadyExistsException : BusinessException
{
    /// <summary>
    /// Represents an exception that is thrown when attempting to create or add
    /// an entity that already exists in the datastore or system. This is typically
    /// used to enforce uniqueness constraints or prevent duplication of data.
    /// </summary>
    public AlreadyExistsException(string message) : base(message)
    {
    }

    /// <summary>
    /// Represents an exception that is thrown when attempting to create or add
    /// an entity that already exists in the datastore or system. This exception
    /// is typically used to prevent duplication and to enforce uniqueness constraints
    /// within the application.
    /// </summary>
    public AlreadyExistsException(string entityType, string champ, object keyName) : base(
        $"{entityType} avec le {champ} '{keyName}' existe déjà")
    {
    }
}