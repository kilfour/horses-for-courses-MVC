using HorsesForCourses.Core.Domain.Courses.InvalidationReasons;
using HorsesForCourses.Core.Domain.Courses.TimeSlots;
using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Courses;


namespace HorsesForCourses.Tests.Courses.C_UpdateTimeSlots;

public class E_UpdateTimeSlotsDomain : CourseDomainTests
{
    [Theory]
    [InlineData(9)]
    [InlineData(10)]
    [InlineData(11)]
    [InlineData(12)]
    [InlineData(13)]
    [InlineData(14)]
    [InlineData(15)]
    [InlineData(16)]
    [InlineData(17)]
    public void CreateOfficeHour_Valid_ShouldSucceed(int hour)
        => Assert.Equal(hour, OfficeHour.From(hour).Value);

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(18)]
    [InlineData(19)]
    [InlineData(20)]
    [InlineData(21)]
    [InlineData(22)]
    [InlineData(23)]
    [InlineData(24)]
    [InlineData(42)]
    [InlineData(666)]
    public void CreateOfficeHour_Invalid_ShouldThrow(int hour)
        => Assert.Throws<InvalidOfficeHour>(() => OfficeHour.From(hour));

    [Fact]
    public void CreateTimeSlot_Valid_ShouldSucceed()
    {
        var slot = TimeSlot.From(CourseDay.Monday, 9, 17);
        Assert.Equal(CourseDay.Monday, slot.Day);
        Assert.Equal(9, slot.Start.Value);
        Assert.Equal(17, slot.End.Value);
    }

    [Fact]
    public void CreateTimeslot_Invalid_ShouldThrow()
        => Assert.Throws<TimeSlotMustBeAtleastOneHourLong>(() =>
            TimeSlot.From(CourseDay.Monday, 9, 9));

    [Fact]
    public void CreateTimeSlot_EndBeforeStart_ShouldThrow()
        => Assert.Throws<TimeSlotMustBeAtleastOneHourLong>(() =>
            TimeSlot.From(CourseDay.Monday, 15, 9));
    [Fact]
    public void UpdateTimeSlots_valid_ShouldSucceed()
    {
        Entity.UpdateTimeSlots(TheCanonical.TimeSlotsFullDayMonday(), a => a);
        var timeSlot = Entity.TimeSlots.Single();
        Assert.Equal(CourseDay.Monday, timeSlot.Day);
        Assert.Equal(9, timeSlot.Start.Value);
        Assert.Equal(17, timeSlot.End.Value);
    }

    [Fact]
    public void UpdateTimeSlots_overlapping_timeslots_Full_Day_ShouldThrow()
    {
        Assert.Throws<OverlappingTimeSlots>(() =>
            Entity.UpdateTimeSlots(
                TheCanonical.TimeSlotsFullDayMonday()
                .Concat(TheCanonical.TimeSlotsFullDayMonday()), a => a));
    }

    [Fact]
    public void UpdateTimeSlots_overlapping_timeslots_overlap_ShouldThrow()
    {
        Assert.Throws<OverlappingTimeSlots>(() =>
            Entity.UpdateTimeSlots(
                [ (CourseDay.Monday, 14, 17)
                , (CourseDay.Monday, 9, 15)], a => a));
    }

    [Fact]
    public void UpdateTimeSlots_overlapping_timeslots_contained_ShouldThrow()
    {
        Assert.Throws<OverlappingTimeSlots>(() =>
            Entity.UpdateTimeSlots(
                [ (CourseDay.Monday, 9, 17)
                , (CourseDay.Monday, 14, 15)], a => a));
    }

    [Fact]
    public void UpdateTimeSlots_overlapping_timeslots_overlap_triple_ShouldThrow()
    {
        Assert.Throws<OverlappingTimeSlots>(() =>
            Entity.UpdateTimeSlots(
                [ (CourseDay.Monday, 9, 12)
                , (CourseDay.Monday, 14, 17)
                , (CourseDay.Monday, 11, 15)], a => a));
    }

    [Fact]
    public void UpdateTimeSlots_AlreadyConfirmed_ShouldThrow()
    {
        Entity.UpdateTimeSlots(TheCanonical.TimeSlotsFullDayMonday(), a => a);
        Entity.Confirm();
        Assert.Throws<CourseAlreadyConfirmed>(() =>
            Entity.UpdateTimeSlots(TheCanonical.TimeSlotsFullDayMonday(), a => a));
    }
}
