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
using QuickFuzzr.UnderTheHood;
using QuickPulse.Arteries;

namespace HorsesForCourses.Tests.Integration;

public record AcidTestContext(List<Id<Coach>> CoachIds);



public class AcidTest
{
    [Fact(Skip = "Explicit")]
    public void TheTest()
    {
        var script =
            from options in "Session Factory".Stashed(GetDbContextOptions)
            from coachesInDb in "Session Factory".Stashed(() => new Dictionary<string, int>())
            from coachService in Script.Execute(() => GetCoachesService(options))
            from _ in Script.Choose(
                RegisterCoach(options, coachService, coachesInDb),
                UpdateSkills(options, coachService, coachesInDb)
            )
            select Acid.Test;
        QState.Run(script).With(5.Runs()).And(10.ExecutionsPerRun());
    }

    private static QAcidScript<Acid> RegisterCoach(
        DbContextOptions<AppDbContext> options,
        CoachesService coachService,
        Dictionary<string, int> coachesInDb) =>
            from name in "Coach Name".Input(Fuzz.ChooseFrom(DataLists.FirstNames).Unique("name"))
            from email in "Coach Email".Input(Fuzz.Constant($"{name}@coaches.com"))
            from coachId in "Register Coach".Act(() =>
            {
                var id = coachService.RegisterCoach(name, email).Await();
                coachesInDb[name] = id;
                return id;
            })
            from reload in Script.Execute(() => LoadCoach(options, coachId))
            from registered in "Coach Is Registered".Spec(() => reload != null)
            from _ in "Coach Name Registered".Spec(() => reload.Name.Value == TheCanonical.CoachName)
            from __ in "Coach Email Registered".Spec(() => reload.Email.Value == TheCanonical.CoachEmail)
            select Acid.Test;

    private static QAcidScript<Acid> UpdateSkills(
        DbContextOptions<AppDbContext> options,
        CoachesService coachService,
        Dictionary<string, int> coachesInDb) =>
            from skills in "coach skills".Input(Fuzz.ChooseFromThese(Skills).Many(1, 5).Unique(Guid.NewGuid()))
            from coachName in "coach name".Input(Fuzz.ChooseFromWithDefaultWhenEmpty(coachesInDb.Keys))
            from success in "Update Skills".ActIf(
                () => coachesInDb.Count != 0,
                () => coachService.UpdateSkills(coachesInDb[coachName], skills).Await())
            from reload in Script.Execute(() => LoadCoach(options, coachesInDb[coachName]))
            from registered in "Coach Is Registered".Spec(() => reload != null)
            from _ in "Coach Name Registered".Spec(() => reload.Name.Value == TheCanonical.CoachName)
            from __ in "Coach Email Registered".Spec(() => reload.Email.Value == TheCanonical.CoachEmail)
            select Acid.Test;

    private readonly static string[] Skills =
        [ "C#"
        , "Cookery"
        , "Agile Dev"
        , "Kung Fu"
        , "Flower Arranging"];


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
