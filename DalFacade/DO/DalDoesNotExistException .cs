
namespace DO;
[Serializable]

public class DalDoesNotExistException : Exception
{
    public DalDoesNotExistException(string? message) : base(message) { }

}
