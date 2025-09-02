using HorsesForCourses.Core.Abstractions;
using HorsesForCourses.Core.Domain.Coaches;
using HorsesForCourses.Core.Domain.Courses;
using HorsesForCourses.Core.Domain.Courses.TimeSlots;
using HorsesForCourses.Service.Coaches;
using HorsesForCourses.Service.Coaches.GetCoachById;
using HorsesForCourses.Service.Coaches.GetCoachDetail;
using HorsesForCourses.Service.Coaches.GetCoaches;
using HorsesForCourses.Service.Coaches.Repository;
using HorsesForCourses.Service.Courses;
using HorsesForCourses.Service.Warehouse;
using HorsesForCourses.Tests.Tools;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

using QuickAcid;
using QuickFuzzr;
using QuickFuzzr.Data;
using QuickFuzzr.UnderTheHood;
using QuickPulse.Show;
using WibblyWobbly;

namespace HorsesForCourses.Tests.Integration;

public record CoachesIn(Dictionary<string, int> Db)
{
    public int Add(string key, int id)
    {
        Db[key] = id;
        return id;
    }
}

public class AcidTest
{
    [Fact]
    public void TheTest()
    {
        var script =
            from options in Script.Stashed(GetDbContextOptions)
            from coachesInDb in Script.Stashed(() => new CoachesIn([]))
            from coachService in Script.Execute(() => GetCoachesService(options))
            from _ in Script.Choose(
                RegisterCoach(options, coachService, coachesInDb),
                UpdateSkills(options, coachService, coachesInDb))
            select Acid.Test;

        QState.Run(script, 103232334)
            .Options(a => a with { FileAs = "Acid", Verbose = true })
            .With(5.Runs())
            .And(10.ExecutionsPerRun());
    }

    public record CoachName : Input;
    public record CoachEmail : Input;
    public record RegisterTheCoach : Act;
    public record CoachIsRegistered : Spec;
    public record CoachNameCheck : Spec;
    public record CoachEmailCheck : Spec;

    private static QAcidScript<Acid> RegisterCoach(
        DbContextOptions<AppDbContext> options,
        CoachesService coachService,
        CoachesIn coachesIn) =>
            from name in Script.Input<CoachName>().From(Fuzz.ChooseFrom(DataLists.FirstNames).Unique("name"))
            from email in Script.Input<CoachEmail>().From(Fuzz.Constant($"{name}@coaches.com"))
            from coachId in Script.Act<RegisterTheCoach>()
                .Do(() => coachesIn.Add(name, coachService.RegisterCoach(name, email).Await()))
            from reload in Script.Execute(() => LoadCoach(options, coachId))
            from _ in Script.Spec<CoachIsRegistered>(() => reload != null)
            from __ in Script.Spec<CoachNameCheck>(() => reload.Name.Value == name)
            from ___ in Script.Spec<CoachEmailCheck>(() => reload.Email.Value == email)
            select Acid.Test;
    private static QAcidScript<Acid> RegisterCoachOld(
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
            from _ in "Coach Name Registered".Spec(() => reload.Name.Value == name)
            from __ in "Coach Email Registered".Spec(() => reload.Email.Value == email)
            select Acid.Test;
    private static QAcidScript<Acid> UpdateSkills(
        DbContextOptions<AppDbContext> options,
        CoachesService coachService,
        CoachesIn coachesIn) =>
            from coachName in "coach name".Input(Fuzz.ChooseFromWithDefaultWhenEmpty(coachesIn.Db.Keys))
            from skills in "coach skills".Input(Fuzz.ChooseFromThese(Skills).Unique(Guid.NewGuid()).Many(1, 5))
            from success in "Update Skills".ActIf(
                () => coachesIn.Db.Count != 0,
                () => coachService.UpdateSkills(coachesIn.Db[coachName], skills).Await())
            from reload in Script.ExecuteIf(
                () => coachesIn.Db.Count != 0,
                () => LoadCoach(options, coachesIn.Db[coachName]))
            from _ in "Coach Is Registered".SpecIf(
                () => coachesIn.Db.Count != 0,
                () => reload != null)
                // from _t in "Db Skills".TraceIf(
                //     () => coachesIn.Db.Count != 0,
                //     () => Introduce.This(reload.Skills.Select(a => a.Value).Order(), false))
                // from __t in "Input Skills".TraceIf(
                //     () => coachesIn.Db.Count != 0,
                //     () => Introduce.This(skills.Order(), false))
            from __ in "Coach Skills Updated".SpecIf(
                () => coachesIn.Db.Count != 0,
                () => reload.Skills.Select(a => a.Value).Order().SequenceEqual(skills.Order()))

            select Acid.Test;

    private static QAcidScript<Acid> CreateCourse(
        DbContextOptions<AppDbContext> options,
        CoursesService coursesService,
        Dictionary<string, int> coursesInDb) =>
            from name in "Course Name".Input(
                from courseSkill in Fuzz.ChooseFromThese(Skills)
                from suffix in Fuzz.ChooseFromThese(CourseSuffixes)
                from complete in Fuzz.Constant($"{courseSkill}, {suffix}.").Unique("course-names")
                select complete)
            from startDay in "start day".Input(Fuzz.Int(1, 31))
            from start in Script.Execute(() => startDay.January(2025))
            from endDay in "start day".Input(Fuzz.Int(startDay + 1, 32))
            from end in Script.Execute(() => endDay.January(2025))
            from coachId in "Register Coach".Act(() =>
            {
                var id = coursesService.CreateCourse(name, start, end).Await();
                coursesInDb[name] = id;
                return id;
            })
            from reload in Script.Execute(() => LoadCoach(options, coachId))
            from registered in "Course Is Registered".Spec(() => reload != null)
            from _ in "Course Name Registered".Spec(() => reload.Name.Value == TheCanonical.CoachName)
            from __ in "Coach Email Registered".Spec(() => reload.Email.Value == TheCanonical.CoachEmail)
            select Acid.Test;

    private readonly static string[] Skills =
        [ "C#"
        , "Cookery"
        , "Agile Dev"
        , "Kung Fu"
        , "Flower Arranging"];

    private static Coach LoadCoach(DbContextOptions<AppDbContext> options, IdPrimitive id)
        => GetDbContext(options).Coaches.Where(a => a.Id == Id<Coach>.From(id)).Single();

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

    private static Generator<TimeSlot> TimeslotGeneratorFor(int key) =>
        from start in Fuzz.Int(9, 17)
        from end in Fuzz.Int(start + 1, 18)
        from day in Fuzz.Enum<CourseDay>()
        from timeslot in Fuzz.Constant(TimeSlot.From(day, start, end))
        select timeslot;

    private readonly static string[] CourseSuffixes = [
        "For Dummies",
        "Basic Techiques",
        "Advanced",
        "101",
        "For Professionals",
        "The Ultimate",
        "What You Always Wanted to Know"];

    private static readonly Generator<Course> CourseGenerator =
         from start in Fuzz.Int(1, 31)
         let startDate = start.January(2025)
         from end in Fuzz.Int(start + 1, 32)
         let endDate = end.January(2025)
         from skill in Fuzz.ChooseFromThese(Skills)
         from namePartTwo in Fuzz.ChooseFromThese(CourseSuffixes)
         let name = $"{skill}, {namePartTwo}."
         from key in Fuzz.Int().Unique("weekday-key")
         from timeslots in TimeslotGeneratorFor(key).Many(1, 5)
         from course in Fuzz.Constant(new Course(name, startDate, endDate))
            .Apply(a => a.UpdateTimeSlots(timeslots.ToList(), b => (b.Day, b.Start.Value, b.End.Value)))
            .Apply(a => a.Confirm())
         select course;
}
