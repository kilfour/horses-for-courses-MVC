using HorsesForCourses.Service.Coaches;
using HorsesForCourses.Service.Coaches.GetCoachDetail;
using HorsesForCourses.Service.Coaches.GetCoaches;
using HorsesForCourses.Service.Warehouse;
using Moq;

namespace HorsesForCourses.Tests.Tools.Coaches;

public abstract class CoachesServiceTests
{
    protected readonly ICoachesService service;
    protected readonly CoachesRepository repository;
    protected readonly Mock<IAmASuperVisor> supervisor;
    protected readonly Mock<IGetCoachById> getCoachById;
    protected readonly Mock<IGetTheCoachSummaries> getCoachSummaries;
    protected readonly Mock<IGetTheCoachDetail> getCoachDetail;
    protected readonly CoachSpy spy;

    public CoachesServiceTests()
    {
        getCoachDetail = new Mock<IGetTheCoachDetail>();
        getCoachSummaries = new Mock<IGetTheCoachSummaries>();
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
