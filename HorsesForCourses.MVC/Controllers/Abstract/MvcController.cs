using HorsesForCourses.Core.Domain;
using Microsoft.AspNetCore.Mvc;

namespace HorsesForCourses.MVC.Controllers.Abstract;

public abstract class MvcController : Controller
{
    protected ControllerMethodBuilder This(Func<Task> action)
    {
        return new ControllerMethodBuilder(this, action);
    }

    protected class ControllerMethodBuilder(Controller Controller, Func<Task> Action)
    {
        public ControllerMethodBuilderEnd OnSuccess(Func<IActionResult> onSucces)
        {
            return new ControllerMethodBuilderEnd(Controller, Action, onSucces);
        }
    }

    protected class ControllerMethodBuilderEnd(Controller Controller, Func<Task> Action, Func<IActionResult> OnSuccess)
    {
        public async Task<IActionResult> OnException(Func<IActionResult> onException)
        {
            try
            {
                await Action();
                return OnSuccess();
            }
            catch (DomainException ex)
            {
                Controller.ModelState.AddModelError(string.Empty, ex.MessageFromType());
                return onException();
            }
        }

        public async Task<IActionResult> OnException(Func<Task<IActionResult>> onException)
        {
            try
            {
                await Action();
                return OnSuccess();
            }
            catch (DomainException ex)
            {
                Controller.ModelState.AddModelError(string.Empty, ex.MessageFromType());
                return await onException();
            }
        }
    }

    protected IActionResult ViewOrNotFoundIfNull<T, TViewModel>(T value, Func<T, TViewModel> func)
    {
        if (value == null) return NotFound();
        return View(func(value));
    }
}


