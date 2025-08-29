
using HorsesForCourses.MVC.Controllers.Abstract;
using HorsesForCourses.Service.Courses;

namespace HorsesForCourses.MVC.Controllers;

public class CoursesController(ICoursesService Service) : MvcController
{
}


