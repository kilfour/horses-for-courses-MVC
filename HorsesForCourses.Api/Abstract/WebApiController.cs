using Microsoft.AspNetCore.Mvc;

namespace HorsesForCourses.Api.Abstract;

public abstract class WebApiController : ControllerBase
{
    protected IActionResult OkNotFoundIfNull<T>(T value)
    {
        if (value == null) return NotFound();
        return Ok(value);
    }
    protected IActionResult NoContentNotFoundIfFalse(bool value)
    {
        if (!value) return NotFound();
        return NoContent();
    }
}


