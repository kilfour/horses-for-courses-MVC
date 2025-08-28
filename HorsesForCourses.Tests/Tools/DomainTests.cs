namespace HorsesForCourses.Tests.Tools;

public abstract class DomainTests<TEntity>
{
    public DomainTests()
    {
        Entity = ManipulateEntity(CreateCannonicalEntity());
    }

    protected TEntity Entity { get; }
    protected abstract TEntity CreateCannonicalEntity();
    protected virtual TEntity ManipulateEntity(TEntity entity) => entity;
}