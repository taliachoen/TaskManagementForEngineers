
using BlApi;


namespace BlImplementation;

internal class MilestoneImplementation : IMilestone
{
    private DalApi.IDal milestone = DalApi.Factory.Get;

    public void CreateProjectTimeline()
    {
        throw new NotImplementedException();
    }

    public BO.Milestone GetMilestoneDetails(int milestoneId)
    {
        throw new NotImplementedException();
    }

    public BO.Milestone UpdateMilestone(int milestoneId, BO.Milestone updatedMilestone)
    {
        throw new NotImplementedException();
    }
}
