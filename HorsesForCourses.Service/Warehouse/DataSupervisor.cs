using HorsesForCourses.Core.Abstractions;

namespace HorsesForCourses.Service.Warehouse;

public interface IAmASuperVisor
{
    Task Enlist(IDomainEntity entity);
    Task Ship();
}

public class DataSupervisor : IAmASuperVisor
{
    private readonly AppDbContext dbContext;

    public DataSupervisor(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task Enlist(IDomainEntity entity)
    {
        await dbContext.AddAsync(entity);
    }

    public async Task Ship()
    {
        await dbContext.SaveChangesAsync();
    }
}
