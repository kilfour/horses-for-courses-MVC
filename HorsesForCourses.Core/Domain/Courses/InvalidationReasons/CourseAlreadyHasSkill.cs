namespace HorsesForCourses.Core.Domain.Courses.InvalidationReasons;

public class CourseAlreadyHasSkill(string skill) : DomainException(skill) { }
