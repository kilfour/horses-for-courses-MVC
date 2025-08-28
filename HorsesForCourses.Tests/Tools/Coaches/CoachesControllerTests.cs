using HorsesForCourses.Service.Coaches;
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
