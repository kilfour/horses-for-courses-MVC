using HorsesForCourses.Core.Abstractions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HorsesForCourses.Service.Warehouse;

public class IdValueComparer<T> : ValueComparer<Id<T>>
{
    public IdValueComparer() : base(
        (a, b) => a!.Value == b!.Value,
        id => id.Value.GetHashCode(),
        id => Id<T>.From(id.Value))
    { }
}