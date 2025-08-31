namespace HorsesForCourses.Core.Abstractions;

public record Id<T>
{
    private readonly IdPrimitive value;
    public IdPrimitive Value => value;

    public static Id<T> Empty { get; } = new Id<T>(default(IdPrimitive));

    private Id() { }

    private Id(IdPrimitive value) => this.value = value;

    public static Id<T> From(IdPrimitive value) => new(value);
}

