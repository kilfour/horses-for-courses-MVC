using HorsesForCourses.Core.Domain;
using Microsoft.AspNetCore.Mvc;

namespace HorsesForCourses.MVC.Controllers.Abstract;

public abstract class MvcController : Controller
{
    protected Actor This(Func<Task> action) => new(this, action);

    protected class Actor(Controller Controller, Func<Task> Action)
    {
        public Critic OnSuccess(Func<IActionResult> onSucces) => new(Controller, Action, onSucces);
    }

    protected class Critic(Controller Controller, Func<Task> Action, Func<IActionResult> OnSuccess)
    {
        public async Task<IActionResult> OnException(Func<IActionResult> onException)
            => await SafeTry(Task.FromResult(onException()));

        public async Task<IActionResult> OnException(Func<Task<IActionResult>> onException)
            => await SafeTry(onException());

        private async Task<IActionResult> SafeTry(Task<IActionResult> onException)
        {
            try
            {
                await Action();
                return OnSuccess();
            }
            catch (DomainException ex)
            {
                Controller.ModelState.AddModelError(string.Empty, ex.MessageFromType);
                return await onException;
            }
        }
    }

    protected IActionResult ViewOrNotFoundIfNull<T, TViewModel>(T value, Func<T, TViewModel> func)
    {
        if (value == null) return NotFound();
        return View(func(value));
    }
}


