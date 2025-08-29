using HorsesForCourses.Core.Domain.Coaches;
using HorsesForCourses.Core.Domain.Courses;
using HorsesForCourses.Core.Domain.Courses.TimeSlots;

namespace HorsesForCourses.Tests.Tools.Courses;

public class CourseSpy : Course
{
    public CourseSpy() : base(TheCanonical.CourseName, TheCanonical.CourseStart, TheCanonical.CourseEnd) { }
    public bool RequiredSkillsCalled;
    public IEnumerable<string>? RequiredSkillsSeen;
    public override Course UpdateRequiredSkills(IEnumerable<string> skills)
    {
        RequiredSkillsCalled = true; RequiredSkillsSeen = skills;
        base.UpdateRequiredSkills(skills);
        return this;
    }

    public bool TimeSlotsCalled;
    public IEnumerable<TimeSlot>? TimeSlotsSeen;

    public override Course UpdateTimeSlots<T>(IEnumerable<T> timeSlotInfo, Func<T, (CourseDay Day, int Start, int End)> getTimeSlot)
    {
        TimeSlotsCalled = true;
        base.UpdateTimeSlots(timeSlotInfo, getTimeSlot);
        TimeSlotsSeen = TimeSlots;
        return this;
    }

    public bool AssignCoachCalled;
    public Coach? AssignCoachSeen;
    public override Course AssignCoach(Coach coach)
    {
        AssignCoachCalled = true; AssignCoachSeen = coach;
        base.AssignCoach(coach);
        return this;
    }
}