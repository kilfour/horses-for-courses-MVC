using HorsesForCourses.Core.Domain.Courses;
using HorsesForCourses.Core.Domain.Courses.InvalidationReasons;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Courses;


namespace HorsesForCourses.Tests.Courses.A_CreateCourse;

public class E_CreateCourseDomain : CourseDomainTests
{
    [Fact]
    public void CreateCourse_WithValidData_ShouldSucceed()
    {
        Assert.Equal(TheCanonical.CourseName, Entity.Name.Value);
        Assert.Equal(TheCanonical.CourseStart, Entity.Period.Start);
        Assert.Equal(TheCanonical.CourseEnd, Entity.Period.End);
    }

    [Fact]
    public void CreateCourse_WithValidData_does_not_assign_id()
        => Assert.Equal(default, Entity.Id.Value);

    [Fact]
    public void CreateCourse_WithEmptyName_ShouldThrow()
        => Assert.Throws<CourseNameCanNotBeEmpty>(
            () => new Course(string.Empty, TheCanonical.CourseStart, TheCanonical.CourseEnd));

    [Fact]
    public void CreateCourse_WithLongName_ShouldThrow()
        => Assert.Throws<CourseNameCanNotBeTooLong>(
            () => new Course(new string('-', 101), TheCanonical.CourseStart, TheCanonical.CourseEnd));

    [Fact]
    public void CreateCourse_WithEndDateBeforeStartDate_ShouldThrow()
        => Assert.Throws<CourseEndDateCanNotBeBeforeStartDate>(
            () => new Course(TheCanonical.CourseName, new DateOnly(2025, 7, 31), new DateOnly(2025, 7, 1)));


    [Fact]
    public void CreateCourse_WithDefaultStartDate_ShouldThrow()
        => Assert.Throws<CourseStartDateCanNotBeEmpty>(
            () => new Course(TheCanonical.CourseName, default, new DateOnly(2025, 7, 1)));

    [Fact]
    public void CreateCourse_WithDefaultEndDate_ShouldThrow()
        => Assert.Throws<CourseEndDateCanNotBeEmpty>(
            () => new Course(TheCanonical.CourseName, new DateOnly(2025, 7, 31), default));
}
