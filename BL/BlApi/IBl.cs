

namespace BlApi;

public interface IBl
{
    public IEngineer Engineer { get; }
    
    public ITask Task { get; }

    //Action to update the project start date 
    public DateTime? StartProject { get; set; }
    //Return action to the project end date
    public DateTime? EndProject { get; set; }
    
    void Reset();
}
