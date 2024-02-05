namespace BO;

//מספר מזהה לא קיים
[Serializable]
public class BlDoesNotExistException : Exception
{
    public BlDoesNotExistException(string? message) : base(message) { }

    public BlDoesNotExistException(string message, Exception innerException)
                : base(message, innerException) { }
}

//תכונה עם ערך נאל
[Serializable]
public class BlNullPropertyException : Exception
{
    public BlNullPropertyException(string? message) : base(message) { }

    public BlNullPropertyException(string message, Exception innerException)
                : base(message, innerException) { }
}

//מספר מזהה כבר קיים
[Serializable]
public class BlAlreadyExistsException : Exception
{
    public BlAlreadyExistsException(string? message) : base(message) { }

    public BlAlreadyExistsException(string message, Exception innerException)
                : base(message, innerException) { }
}


//המחיקה לא מתאפשרת
[Serializable]
public class BlDeletionImpossible : Exception
{
    public BlDeletionImpossible(string? message) : base(message) { }

    public BlDeletionImpossible(string message, Exception innerException)
                : base(message, innerException) { }

}

[Serializable]
public class BlInvalidDataException : Exception
{
    public BlInvalidDataException(string? message) : base(message) { }

    public BlInvalidDataException(string message, Exception innerException)
                : base(message, innerException) { }
}
