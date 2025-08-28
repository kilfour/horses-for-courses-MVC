using HorsesForCourses.Api.Warehouse;
using HorsesForCourses.Core.Domain.Coaches;
using HorsesForCourses.Core.Domain.Courses;
using HorsesForCourses.Tests.Tools;
using Microsoft.EntityFrameworkCore;


namespace HorsesForCourses.Tests.Courses.E_AssignCoach;

public class C_AssignCoachData : TheDatabaseTest
{
    private readonly Course course;
    private readonly Coach coach;

    public C_AssignCoachData()
    {
        course = TheCanonical.Course();
        coach = TheCanonical.Coach();
        AddToDb(course, coach);
    }

    private void Act()
    {
        var context = GetDbContext();
        var entity = Reload(context);
        entity.UpdateTimeSlots(TheCanonical.TimeSlotsFullDayMonday(), a => a);
        entity.Confirm();
        entity.AssignCoach(coach);
        context.SaveChanges();
    }

    private Course Reload() => Reload(GetDbContext());
    private Course Reload(AppDbContext dbContext) => dbContext.Courses.Include(a => a.AssignedCoach).Single(a => a.Id == course.Id);

    [Fact]
    public void Coach_can_be_updated()
    {
        Act();
        Assert.Equal(coach.Id, Reload().AssignedCoach!.Id);
    }
}