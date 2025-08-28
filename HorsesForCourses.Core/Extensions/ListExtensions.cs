namespace HorsesForCourses.Core.Extensions;

public static class ListExtensions
{
    private static List<T> GetDuplicates<T>(this IEnumerable<T> collection)
        => collection
            .GroupBy(x => x)
            .Where(g => g.Count() > 1)
            .SelectMany(g => g)
            .Distinct()
            .ToList();

    public static bool NoDuplicatesAllowed<T, TException>(this IEnumerable<T> collection) where TException : Exception, new()
    {
        var duplicates = collection.GetDuplicates();
        if (duplicates.Count != 0)
            throw new TException();
        return true;
    }

    public static bool NoDuplicatesAllowed<T, TException>(
        this IEnumerable<T> collection,
        Func<IEnumerable<T>, TException> factory)
        where TException : Exception
    {
        var duplicates = collection.GetDuplicates();
        if (duplicates.Count != 0)
            throw factory(duplicates);
        return true;
    }
}