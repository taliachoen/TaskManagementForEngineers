
using BlApi;


namespace BlImplementation;

internal class MilestoneImplementation : IMilestone
{
    private DalApi.IDal milestone = DalApi.Factory.Get;

    public void Create()
    {
        throw new NotImplementedException();
    }

    public BO.Milestone ReadAll(int milestoneId)
    {
        throw new NotImplementedException();
    }

    public BO.Milestone Update(int milestoneId, BO.Milestone updatedMilestone)
    {
        throw new NotImplementedException();
    }
}
