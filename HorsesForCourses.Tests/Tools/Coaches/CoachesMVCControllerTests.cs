
using HorsesForCourses.MVC.Controllers;

namespace HorsesForCourses.Tests.Tools.Coaches;

public abstract class CoachesMVCControllerTests : CoachesControllerTests
{
    protected readonly CoachesController controller;

    public CoachesMVCControllerTests()
    {
        controller = new CoachesController(service.Object);
    }
}
