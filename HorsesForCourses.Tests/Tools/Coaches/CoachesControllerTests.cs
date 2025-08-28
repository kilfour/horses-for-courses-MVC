using HorsesForCourses.Service.Coaches;
using HorsesForCourses.Service.Coaches.GetCoachDetail;
using HorsesForCourses.Service.Coaches.GetCoaches;
using HorsesForCourses.Service.Warehouse;
using HorsesForCourses.Service.Warehouse.Paging;
using Moq;

namespace HorsesForCourses.Tests.Tools.Coaches;

public abstract class CoachesControllerTests
{
    protected readonly CoachesRepository repository;
    protected readonly Mock<ICoachesService> service;
    protected readonly Mock<IAmASuperVisor> supervisor;
    protected readonly Mock<IGetCoachById> getCoachById;
    protected readonly Mock<IGetTheCoachSummaries> getCoachSummaries;
    protected readonly Mock<IGetTheCoachDetail> getCoachDetail;
    protected readonly CoachSpy spy;

    public CoachesControllerTests()
    {
        getCoachDetail = new Mock<IGetTheCoachDetail>();
        getCoachDetail.Setup(a => a.One(TheCanonical.CoachId)).ReturnsAsync(new CoachDetail());

        getCoachSummaries = new Mock<IGetTheCoachSummaries>();
        getCoachSummaries.Setup(a => a.All(It.IsAny<PageRequest>()))
            .ReturnsAsync(new PagedResult<CoachSummary>([], 0, 1, 15));

        spy = new();
        getCoachById = new Mock<IGetCoachById>();
        getCoachById.Setup(a => a.Load(TheCanonical.CoachId)).ReturnsAsync(spy);

        supervisor = new Mock<IAmASuperVisor>();
        repository = new CoachesRepository(
           supervisor.Object,
            getCoachById.Object,
            getCoachSummaries.Object,
            getCoachDetail.Object);

        service = new Mock<ICoachesService>();
    }
}
