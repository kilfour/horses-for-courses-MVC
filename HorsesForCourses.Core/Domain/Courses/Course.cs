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

    public Period Period { get; init; } = Period.Empty;

    public IReadOnlyCollection<TimeSlot> TimeSlots => timeSlots.AsReadOnly();
    private List<TimeSlot> timeSlots = [];

    public IReadOnlyCollection<Skill> RequiredSkills => requiredSkills.AsReadOnly();
    private readonly List<Skill> requiredSkills = [];

    public bool IsConfirmed { get; private set; }
    public Coach? AssignedCoach { get; private set; }

    private Course() { /*** EFC Was Here ****/ }
    public Course(string name, DateOnly start, DateOnly end)
    {
        Name = new CourseName(name);
        Period = Period.From(start, end);
    }

    bool NotAllowedIfAlreadyConfirmed()
        => IsConfirmed ? throw new CourseAlreadyConfirmed() : true;

    public virtual Course UpdateRequiredSkills(IEnumerable<string> newSkills)
    {
        NotAllowedIfAlreadyConfirmed();
        NotAllowedWhenThereAreDuplicateSkills();
        return OverWriteRequiredSkills();
        // ------------------------------------------------------------------------------------------------
        // --
        bool NotAllowedWhenThereAreDuplicateSkills()
            => newSkills.NoDuplicatesAllowed(a => new CourseAlreadyHasSkill(string.Join(",", a)));
        Course OverWriteRequiredSkills()
        {
            requiredSkills.Clear();
            requiredSkills.AddRange(newSkills.Select(Skill.From));
            return this;
        }
        // ------------------------------------------------------------------------------------------------
    }

    public virtual Course UpdateTimeSlots<T>(
        IEnumerable<T> timeSlotInfo,
        Func<T, (CourseDay Day, int Start, int End)> getTimeSlot)
    {
        var newTimeSlots = TimeSlot.EnumerableFrom(timeSlotInfo, getTimeSlot);
        NotAllowedIfAlreadyConfirmed();
        NotAllowedWhenTimeSlotsOverlap();
        return OverWriteTimeSlots();
        // ------------------------------------------------------------------------------------------------
        // --
        bool NotAllowedWhenTimeSlotsOverlap()
            => TimeSlot.HasOverlap(newTimeSlots) ? throw new OverlappingTimeSlots() : true;
        Course OverWriteTimeSlots() { this.timeSlots = [.. newTimeSlots]; return this; }
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