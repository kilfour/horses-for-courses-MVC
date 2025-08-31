using HorsesForCourses.Core.Abstractions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HorsesForCourses.Service.Warehouse;

public class IdValueConverter<T> : ValueConverter<Id<T>, IdPrimitive>
{
    public IdValueConverter()
        : base(
            id => id.Value,
            value => Id<T>.From(value))
    { }
}
