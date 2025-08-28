using HorsesForCourses.Api.Coaches;

namespace HorsesForCourses.Tests.Tools.Coaches;

public abstract class CoachesApiControllerTests : CoachesControllerTests
{
    protected readonly CoachesController controller;

    public CoachesApiControllerTests()
    {
        controller = new CoachesController(service.Object);
    }
}
