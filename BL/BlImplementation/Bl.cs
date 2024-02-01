

using BlApi;
namespace BlImplementation;

internal class Bl : IBl
{
    public IEngineer Engineer => new EngineerImplementation();

    public IMilestone Milestone => new MilestoneImplementation();

    public ITask Task => new TaskImplementation();
}