using HorsesForCourses.Core.Abstractions;
using HorsesForCourses.Core.Domain;
using HorsesForCourses.Core.Domain.Courses;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Courses;
using Moq;

namespace HorsesForCourses.Tests.Courses.A_CreateCourse;

public class C_CreateCourseService : CoursesServiceTests
{

    [Fact]
    public async Task CreateCourse_delivers_to_the_supervisor()
    {
        await service.CreateCourse(TheCanonical.CourseName, TheCanonical.CourseStart, TheCanonical.CourseEnd);
        supervisor.Verify(a => a.Enlist(It.Is<Course>(a => a.Name.Value == TheCanonical.CourseName)));
        supervisor.Verify(a => a.Ship());
    }

    [Fact]
    public async Task CreateCourse_Does_Not_Ship_On_Exception()
    {
        await Assert.ThrowsAnyAsync<DomainException>(
            async () => await service.CreateCourse(string.Empty, TheCanonical.CourseStart, TheCanonical.CourseEnd));
        supervisor.Verify(a => a.Ship(), Times.Never);
    }
}