using HorsesForCourses.Core.Abstractions;
using HorsesForCourses.Core.Domain.Coaches;
using HorsesForCourses.Core.Domain.Courses.InvalidationReasons;
using HorsesForCourses.Core.Domain.Courses.TimeSlots;
using HorsesForCourses.Core.Domain.Skills;
using HorsesForCourses.Core.ValidationHelpers;

namespace HorsesForCourses.Core.Domain.Courses;

public class Course : DomainEntity<Course>
{
    public CourseName Name { get; init; } = CourseName.Empty;
    public DateOnly Start { get; init; }
    public DateOnly End { get; init; }
    public List<TimeSlot> TimeSlots { get; private set; } = [];
    public List<Skill> RequiredSkills { get; init; } = [];
    public bool IsConfirmed { get; private set; }
    public Coach? AssignedCoach { get; private set; }

    private Course() { /*** EFC Was Here ****/ }
    public Course(string name, DateOnly start, DateOnly end)
    {
        if (start > end)
            throw new CourseEndDateCanNotBeBeforeStartDate();
        Name = new CourseName(name);
        Start = start;
        End = end;
    }

    bool NotAllowedIfAlreadyConfirmed()
        => IsConfirmed ? throw new CourseAlreadyConfirmed() : true;

    public virtual Course UpdateRequiredSkills(IEnumerable<string> skills)
    {
        NotAllowedIfAlreadyConfirmed();
        NotAllowedWhenThereAreDuplicateSkills(skills);
        return OverWriteRequiredSkills(skills);
        // ------------------------------------------------------------------------------------------------
        // --
        static bool NotAllowedWhenThereAreDuplicateSkills(IEnumerable<string> skills)
            => skills.NoDuplicatesAllowed(a => new CourseAlreadyHasSkill(string.Join(",", a)));
        Course OverWriteRequiredSkills(IEnumerable<string> skills)
        {
            var newSkills = skills.Select(Skill.From).ToList();
            RequiredSkills.Clear();
            RequiredSkills.AddRange(newSkills);
            return this;
        }
        // ------------------------------------------------------------------------------------------------
    }

    public virtual Course UpdateTimeSlots<T>(IEnumerable<T> timeSlotInfo, Func<T, (CourseDay Day, int Start, int End)> getTimeSlot)
    {
        var timeSlots = TimeSlot.EnumerableFrom(timeSlotInfo, getTimeSlot);
        NotAllowedIfAlreadyConfirmed();
        NotAllowedWhenTimeSlotsOverlap(timeSlots);
        return OverWriteTimeSlots(timeSlots);
        // ------------------------------------------------------------------------------------------------
        // --
        bool NotAllowedWhenTimeSlotsOverlap(IEnumerable<TimeSlot> timeSlots)
            => TimeSlot.HasOverlap(timeSlots) ? throw new OverlappingTimeSlots() : true;
        Course OverWriteTimeSlots(IEnumerable<TimeSlot> timeSlots) { TimeSlots = [.. timeSlots]; return this; }
        // ------------------------------------------------------------------------------------------------
    }

    public Course Confirm()
    {
        NotAllowedIfAlreadyConfirmed();
        NotAllowedWhenThereAreNoTimeSlots();
        return ConfirmIt();
        // ------------------------------------------------------------------------------------------------
        // --
        bool NotAllowedWhenThereAreNoTimeSlots()
            => TimeSlots.Count == 0 ? throw new AtLeastOneTimeSlotRequired() : true;
        Course ConfirmIt() { IsConfirmed = true; return this; }
        // ------------------------------------------------------------------------------------------------
    }

    public virtual Course AssignCoach(Coach coach)
    {
        NotAllowedIfNotYetConfirmed();
        NotAllowedIfCourseAlreadyHasCoach();
        NotAllowedIfCoachIsInsuitable(coach);
        NotAllowedIfCoachIsUnavailable(coach);
        return AssignTheCoachAlready(coach);

        // ------------------------------------------------------------------------------------------------
        // --
        bool NotAllowedIfNotYetConfirmed()
            => !IsConfirmed ? throw new CourseNotYetConfirmed() : true;
        bool NotAllowedIfCourseAlreadyHasCoach()
            => AssignedCoach != null ? throw new CourseAlreadyHasCoach() : true;
        bool NotAllowedIfCoachIsInsuitable(Coach coach)
            => !coach.IsSuitableFor(this) ? throw new CoachNotSuitableForCourse() : true;
        bool NotAllowedIfCoachIsUnavailable(Coach coach)
            => !coach.IsAvailableFor(this) ? throw new CoachNotAvailableForCourse() : true;
        Course AssignTheCoachAlready(Coach coach)
        {
            AssignedCoach = coach;
            coach.AssignCourse(this);
            return this;
        }
        // ------------------------------------------------------------------------------------------------
    }
}