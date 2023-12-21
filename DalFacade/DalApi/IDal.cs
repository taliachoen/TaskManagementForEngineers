

namespace DalApi;

public interface IDal
{
    IDependensy Dependensy { get; }
    IEngineer Engineer { get; }
    ITask Task { get; }

    //Reset operation setting line
    void Reset();

    //Action to update the project completion date
    public void UpdateEndProject(DateTime? value);

    //Action to update the project start date
    public void UpdateStartProject(DateTime? value);

    //Action to update the project start date
    public DateTime? ReturnStartProject();

    //Return action to the project end date
    public DateTime? ReturnEndProject();






}

