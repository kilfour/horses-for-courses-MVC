namespace HorsesForCourses.Core.Domain;

public class DomainException : Exception
{
    public DomainException() : base() { }
    public DomainException(string message) : base(message) { }
}
