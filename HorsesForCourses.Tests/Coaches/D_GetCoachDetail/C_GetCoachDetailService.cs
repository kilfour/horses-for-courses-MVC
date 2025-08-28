using HorsesForCourses.Tests.Tools;
using HorsesForCourses.Tests.Tools.Coaches;
using Moq;

namespace HorsesForCourses.Tests.Coaches.D_GetCoachDetail;

public class C_GetCoachDetailService : CoachesServiceTests
{
    [Fact]
    public async Task GetCoachDetail_uses_the_query_object()
    {
        await service.GetCoachDetail(TheCanonical.CoachId);
        getCoachDetail.Verify(a => a.One(TheCanonical.CoachId));
    }

    [Fact]
    public async Task GetCoachDetail_success_returns_coach_detail()
    {
        var expected = TheCanonical.CoachDetail();
        getCoachDetail.Setup(a => a.One(TheCanonical.CoachId)).ReturnsAsync(expected);
        var result = await service.GetCoachDetail(TheCanonical.CoachId);
        Assert.Equal(expected, result);
    }
}