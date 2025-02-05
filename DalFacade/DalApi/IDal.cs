﻿namespace DalApi;

public interface IDal
{
    IDependency Dependency { get; }
    IEngineer Engineer { get; }
    ITask Task { get; }
    //Reset all data 
    void Reset();
    //Action to update the project completion date
    public void UpdateEndProject(DateTime value);
    //Action to update the project start date
    public void UpdateStartProject(DateTime? value);
    //Action to update the project start date 
    public DateTime? ReturnStartProject();
    //Return action to the project end date
    public DateTime? ReturnEndProject();

}

