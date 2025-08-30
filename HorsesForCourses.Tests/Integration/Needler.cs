using System.Collections.Concurrent;

namespace HorsesForCourses.Tests.Integration;

public class Needler<TIn, TOut>
{
    private readonly ConcurrentDictionary<string, (TIn Input, TOut Output)> data = new();

    public void Register(string key, TIn input, TOut output) =>
        data.TryAdd(key, (input, output));

    public TIn GetInput(string key) => data[key].Input;
    public TOut GetOutput(string key) => data[key].Output;

    public bool Check<TValue>(Func<TIn, TValue> expected, Func<TOut, TValue> actual) =>
        data.Keys.All(key =>
            EqualityComparer<TValue>.Default.Equals(
                expected(data[key].Input),
                actual(data[key].Output)));

    public bool Check(Func<TIn, TOut, bool> condition) =>
        data.Keys.All(key => condition(data[key].Input, data[key].Output));

    public bool HasDataWaiting => !data.IsEmpty;

    public IEnumerable<string> Keys => data.Keys;
}

