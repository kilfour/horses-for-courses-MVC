using HorsesForCourses.Service.Coaches;
using HorsesForCourses.Service.Coaches.GetCoachById;
using HorsesForCourses.Service.Coaches.GetCoachDetail;
using HorsesForCourses.Service.Coaches.GetCoaches;
using HorsesForCourses.Service.Coaches.Repository;
using HorsesForCourses.Service.Warehouse;
using Moq;

namespace HorsesForCourses.Tests.Tools.Coaches;

public abstract class CoachesServiceTests
{
    protected readonly ICoachesService service;
    protected readonly CoachesRepository repository;
    protected readonly Mock<IAmASuperVisor> supervisor;
    protected readonly Mock<IGetCoachById> getCoachById;
    protected readonly Mock<IGetCoachSummaries> getCoachSummaries;
    protected readonly Mock<IGetCoachDetail> getCoachDetail;
    protected readonly CoachSpy spy;

    public CoachesServiceTests()
    {
        getCoachDetail = new Mock<IGetCoachDetail>();
        getCoachSummaries = new Mock<IGetCoachSummaries>();
        spy = new();
        getCoachById = new Mock<IGetCoachById>();

        supervisor = new Mock<IAmASuperVisor>();
        repository = new CoachesRepository(
           supervisor.Object,
            getCoachById.Object,
            getCoachSummaries.Object,
            getCoachDetail.Object);

        service = new CoachesService(repository);
    }
}
