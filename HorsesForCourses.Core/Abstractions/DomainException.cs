using System.Text.RegularExpressions;

namespace HorsesForCourses.Core.Domain;

public class DomainException : Exception
{
    public DomainException() : base() { }
    public DomainException(string message) : base(message) { }

    public string MessageFromType()
    {
        var type = GetType().Name;
        // Insert a space before each capital (except the first one)
        string withSpaces = Regex.Replace(type, "(?<!^)([A-Z])", " $1");
        return $"{new string([.. withSpaces.Take(1)])}{new string([.. withSpaces.Skip(1).Select(char.ToLower)])}.";

    }
}
