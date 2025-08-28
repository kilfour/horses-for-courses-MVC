using HorsesForCourses.Service.Coaches;
using HorsesForCourses.Service.Coaches.GetCoachDetail;
using HorsesForCourses.Service.Coaches.GetCoaches;
using HorsesForCourses.Service.Warehouse;
using HorsesForCourses.Service.Warehouse.Paging;
using Moq;

namespace HorsesForCourses.Tests.Tools.Coaches;

public abstract class CoachesControllerTests
{
    protected readonly Mock<ICoachesService> service;

    public CoachesControllerTests()
    {
        service = new Mock<ICoachesService>();
    }
}
