namespace HorsesForCourses.Core.ValidationHelpers;

public static class DefaultString
{
    public const int MaxLength = 100;
    public static string IsValidDefaultString<TNotEmpty, TTooLong>(this string input)
        where TNotEmpty : Exception, new()
        where TTooLong : Exception, new()
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new TNotEmpty();
        if (input.Length > MaxLength)
            throw new TTooLong();
        return input;
    }
}