using HorsesForCourses.Service.Coaches.GetCoachById;
using HorsesForCourses.Service.Coaches.GetCoachDetail;
using HorsesForCourses.Service.Coaches.GetCoaches;
using HorsesForCourses.Service.Warehouse;

namespace HorsesForCourses.Service.Coaches.Repository;

public record CoachesRepository(
    IAmASuperVisor Supervisor,
    IGetCoachById GetCoachById,
    IGetCoachSummaries GetCoachSummaries,
    IGetCoachDetail GetCoachDetail);