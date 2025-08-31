using HorsesForCourses.Core.Abstractions;
using HorsesForCourses.Core.Domain.Coaches;
using HorsesForCourses.Service.Coaches;
using HorsesForCourses.Service.Coaches.GetCoachById;
using HorsesForCourses.Service.Coaches.GetCoachDetail;
using HorsesForCourses.Service.Coaches.GetCoaches;
using HorsesForCourses.Service.Coaches.Repository;
using HorsesForCourses.Service.Warehouse;
using HorsesForCourses.Tests.Tools;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

using QuickAcid;
using QuickFuzzr;
using QuickFuzzr.Data;

namespace HorsesForCourses.Tests.Integration;

public record AcidTestContext(List<Id<Coach>> CoachIds);

public class AcidTest
{
    [Fact(Skip = "integration")]
    public void RegisterCoach()
    {
        var script =
            from options in "Session Factory".Stashed(GetDbContextOptions)
            from coachService in Script.Execute(() => GetCoachesService(options))
            from _ in Script.Choose(
                from coachId in "Register Coach".Act(() => coachService
                    .RegisterCoach(TheCanonical.CoachName, TheCanonical.CoachEmail)
                    .Await())
                from reload in Script.Execute(() => LoadCoach(options, coachId))
                from registered in "Coach Is Registered".Spec(() => reload != null)
                from _ in "Coach Name Registered".Spec(() => reload.Name.Value == TheCanonical.CoachName)
                from __ in "Coach Email Registered".Spec(() => reload.Email.Value == TheCanonical.CoachEmail)
                select Acid.Test
            )
            select Acid.Test;
        QState.Run(script).With(5.Runs()).And(5.ExecutionsPerRun());
    }

    public record RegisterCoachRequest(string Name, string Email);

    [Fact(Skip = "integration")]
    public void RegisterCoachConcurrent()
    {
        var script =

            from needler in "Needler".Stashed(() => new Needler<RegisterCoachRequest, IdPrimitive>())

            from options in "Session Factory".Stashed(GetDbContextOptions)
            from coachService in Script.Execute(() => GetCoachesService(options))

            from _ in Script.Choose(
                // Register Coach 
                from name in "Coach Name".Input(Fuzz.ChooseFrom(DataLists.FirstNames).Unique("name"))
                from email in "Coach Email".Input(Fuzz.Constant($"{name}@coaches.com"))
                from request in Script.Execute(() => new RegisterCoachRequest(name, email))
                from sleepy in "sleep time".Input(Fuzz.Int())
                from task in "Register Coach".Act(() =>
                {
                    coachService.RegisterCoach(name, email)
                        .Attach(needler, name, request, sleepy);
                })
                select Acid.Test,

                // Delayed Verify
                //   Todo: unify asserts in one Spec or something and clear needler data after check 
                from registered in "Coach Is Registered".SpecIf(
                    () => needler.HasDataWaiting,
                    () => needler.Check((a, b) => LoadCoach(options, b) != null))
                from withEmail in "Coach Email Registered".SpecIf(
                    () => needler.HasDataWaiting,
                    () => needler.Check(a => a.Email, a => LoadCoach(options, a).Email.Value))
                select Acid.Test)

            select Acid.Test;

        var needleScript =

        QState.Run(script)
            .Options(a => a with { FileAs = "RegisterCoach" })
            .With(50.Runs())
            .And(20.ExecutionsPerRun());
    }

    private static Coach LoadCoach(DbContextOptions<AppDbContext> options, IdPrimitive id)
    {
        return GetDbContext(options).Coaches.Where(a => a.Id == Id<Coach>.From(id)).Single();
    }

    private static DbContextOptions<AppDbContext> GetDbContextOptions()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connection);
        var dbContext = GetDbContext(builder.Options);
        dbContext.Database.EnsureCreated();
        return builder.Options;
    }

    private static AppDbContext GetDbContext(DbContextOptions<AppDbContext> options) => new(options);

    private static CoachesService GetCoachesService(DbContextOptions<AppDbContext> options)
    {
        var dbContext = GetDbContext(options);
        var getCoachDetail = new GetCoachDetail(dbContext);
        var getCoachSummaries = new GetCoachSummaries(dbContext);
        var getCoachById = new GetCoachById(dbContext);
        var supervisor = new DataSupervisor(dbContext);
        var repository = new CoachesRepository(
            supervisor,
            getCoachById,
            getCoachSummaries,
            getCoachDetail);
        return new CoachesService(repository);
    }
}
