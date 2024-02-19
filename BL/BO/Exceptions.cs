namespace BO;

/// <summary>
/// Exception thrown when attempting to access a non-existent entity.
/// </summary>
[Serializable]
public class BlDoesNotExistException : Exception
{
    public BlDoesNotExistException(string? message) : base(message) { }

    public BlDoesNotExistException(string message, Exception innerException)
                : base(message, innerException) { }
}

/// <summary>
/// Exception thrown when a property has a null value.
/// </summary>
[Serializable]
public class BlNullPropertyException : Exception
{
    public BlNullPropertyException(string? message) : base(message) { }

    public BlNullPropertyException(string message, Exception innerException)
                : base(message, innerException) { }
}

/// <summary>
/// Exception thrown when attempting to create an entity that already exists.
/// </summary>
[Serializable]
public class BlAlreadyExistsException : Exception
{
    public BlAlreadyExistsException(string? message) : base(message) { }

    public BlAlreadyExistsException(string message, Exception innerException)
                : base(message, innerException) { }
}

/// <summary>
/// Exception thrown when deletion of an entity is not possible.
/// </summary>
[Serializable]
public class BlDeletionImpossibleException : Exception
{
    public BlDeletionImpossibleException(string? message) : base(message) { }

    public BlDeletionImpossibleException(string message, Exception innerException)
                : base(message, innerException) { }
}

/// <summary>
/// Exception thrown when reading from an entity is not possible.
/// </summary>
[Serializable]
public class BlReadImpossibleException : Exception
{
    public BlReadImpossibleException(string? message) : base(message) { }

    public BlReadImpossibleException(string message, Exception innerException)
                : base(message, innerException) { }
}

/// <summary>
/// Exception thrown when invalid data is encountered.
/// </summary>
[Serializable]
public class BlInvalidDataException : Exception
{
    public BlInvalidDataException(string? message) : base(message) { }

    public BlInvalidDataException(string message, Exception innerException)
                : base(message, innerException) { }
}

/// <summary>
/// Exception thrown when no update is made to an entity.
/// </summary>
[Serializable]
public class BlNoUpdateWasMadeException : Exception
{
    public BlNoUpdateWasMadeException(string? message) : base(message) { }

    public BlNoUpdateWasMadeException(string message, Exception innerException)
                : base(message, innerException) { }
}

/// <summary>
/// Exception thrown when an update to an entity is not successful.
/// </summary>
[Serializable]
public class BlUnableToUpdateException : Exception
{
    public BlUnableToUpdateException(string? message) : base(message) { }

    public BlUnableToUpdateException(string message, Exception innerException)
                : base(message, innerException) { }
}

/// <summary>
/// Exception thrown when creating an object is not possible.
/// </summary>
[Serializable]
public class BlCreateImpossibleException : Exception
{
    public BlCreateImpossibleException(string? message) : base(message) { }

    public BlCreateImpossibleException(string message, Exception innerException)
                : base(message, innerException) { }
}
