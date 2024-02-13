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

//כבר קיים
[Serializable]
public class BlAlreadyExistsException : Exception
{
    public BlAlreadyExistsException(string? message) : base(message) { }

    public BlAlreadyExistsException(string message, Exception innerException)
                : base(message, innerException) { }
}

//המחיקה לא מתאפשרת
[Serializable]
public class BlDeletionImpossibleException : Exception
{
    public BlDeletionImpossibleException(string? message) : base(message) { }

    public BlDeletionImpossibleException(string message, Exception innerException)
                : base(message, innerException) { }

}


//הקריאה לא מתאפשרת
[Serializable]
public class BlReadImpossibleException : Exception
{
    public BlReadImpossibleException(string? message) : base(message) { }

    public BlReadImpossibleException(string message, Exception innerException)
                : base(message, innerException) { }

}


//נתונים לא חוקיים
[Serializable]
public class BlInvalidDataException : Exception
{
    public BlInvalidDataException(string? message) : base(message) { }

    public BlInvalidDataException(string message, Exception innerException)
                : base(message, innerException) { }
}

//לא בוצע עידכון
[Serializable]
public class BlNoUpdateWasMadeException: Exception
{
    public BlNoUpdateWasMadeException(string? message) : base(message) { }

    public BlNoUpdateWasMadeException(string message, Exception innerException)
                : base(message, innerException) { }
}

//לא הצליח לעדכן
[Serializable]
public class BlUnableToUpdateException : Exception
{
    public BlUnableToUpdateException(string? message) : base(message) { }

    public BlUnableToUpdateException(string message, Exception innerException)
                : base(message, innerException) { }
}


//לא הצליח ליצור אוביקט
[Serializable]
public class BlCreateImpossibleException : Exception
{
    public BlCreateImpossibleException(string? message) : base(message) { }

    public BlCreateImpossibleException(string message, Exception innerException)
                : base(message, innerException) { }

}
