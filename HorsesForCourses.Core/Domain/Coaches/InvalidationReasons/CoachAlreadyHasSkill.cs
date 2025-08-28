namespace HorsesForCourses.Core.Domain.Coaches.InvalidationReasons;

public class CoachAlreadyHasSkill(string skill) : DomainException(skill) { }