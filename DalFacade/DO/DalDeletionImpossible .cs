

namespace DO;
[Serializable]

public class DalDeletionImpossible : Exception
{
    public DalDeletionImpossible(string? message) : base(message) { }

}
