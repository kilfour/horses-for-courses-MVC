using HorsesForCourses.Api.Coaches.GetCoachDetail;
using HorsesForCourses.Api.Coaches.GetCoaches;
using HorsesForCourses.Api.Warehouse;

namespace HorsesForCourses.Api.Coaches;

public class CoachesRepository
{
    public CoachesRepository(
        IAmASuperVisor supervisor,
        IGetCoachById getCoachById,
        IGetTheCoachSummaries getTheCoachSummaries,
        IGetTheCoachDetail getTheCoachDetail)
    {
        Supervisor = supervisor;
        GetCoachById = getCoachById;
        GetTheCoachSummaries = getTheCoachSummaries;
        GetTheCoachDetail = getTheCoachDetail;
    }

    public IAmASuperVisor Supervisor { get; }
    public IGetCoachById GetCoachById { get; }
    public IGetTheCoachSummaries GetTheCoachSummaries { get; }
    public IGetTheCoachDetail GetTheCoachDetail { get; }
}