using HorsesForCourses.Core.Domain.Courses.InvalidationReasons;
using HorsesForCourses.Core.Domain.Courses.TimeSlots;

namespace HorsesForCourses.Tests.Core;

public class OfficeHourTests
{
    [Theory]
    [MemberData(nameof(ValidHours))]
    public void Create_OfficeHour_Valid_ShouldSucceed(int value)
    {
        var officeHour = OfficeHour.From(value);
        Assert.Equal(value, officeHour.Value);
    }

    public static IEnumerable<object[]> ValidHours
        => new[] { 9, 10, 11, 12, 13, 14, 15, 16, 17 }.Select(x => new object[] { x });

    [Theory]
    [MemberData(nameof(InvalidHours))]
    public void Create_OfficeHour_Invalid_ShouldThrow(int value)
    {
        Assert.Throws<InvalidOfficeHour>(() => OfficeHour.From(value));
    }

    public static IEnumerable<object[]> InvalidHours
    => new[] { -42, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 18, 19, 20, 21, 22, 23, 24, 42 }.Select(x => new object[] { x });

    [Theory]
    [InlineData(15, 16, true)]
    [InlineData(16, 16, false)]
    [InlineData(17, 16, false)]
    public void OfficeHour_Can_Compare(int first, int second, bool result)
    {
        Assert.Equal(result, OfficeHour.From(first) < OfficeHour.From(second));
    }

    [Theory]
    [InlineData(15, 16, -1)]
    [InlineData(16, 16, 0)]
    [InlineData(17, 16, 1)]
    public void OfficeHour_Can_Subtract(int first, int second, int result)
    {
        Assert.Equal(result, OfficeHour.From(first) - OfficeHour.From(second));
    }
}

