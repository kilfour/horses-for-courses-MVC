// using HorsesForCourses.Core.Domain.Courses.InvalidationReasons;
// using HorsesForCourses.Tests.Tools;
// using Microsoft.AspNetCore.Mvc;
// using Moq;

// namespace HorsesForCourses.Tests.Courses.A_CreateCourse;

// public class B_CreateCourseMVC : CoursesMVCControllerTests
// {
//     [Fact]
//     public async Task CreateCourse_GET_Passes_The_Model_To_The_View()
//     {
//         var result = await controller.CreateCourse();
//         var view = Assert.IsType<ViewResult>(result);
//         var viewModel = Assert.IsType<CreateCourseViewModel>(view.Model);
//         Assert.Equal(string.Empty, viewModel.Name);
//         Assert.Equal(string.Empty, viewModel.Email);
//     }

//     [Fact]
//     public async Task CreateCourse_POST_Puts_The_Course_In_Storage()
//     {
//         await controller.CreateCourse(TheCanonical.CourseName, TheCanonical.CourseEmail);
//         service.Verify(a => a.CreateCourse(TheCanonical.CourseName, TheCanonical.CourseEmail));
//     }

//     [Fact]
//     public async Task CreateCourse_POST_Redirects_To_Index_On_Success()
//     {
//         var result = await controller.CreateCourse(TheCanonical.CourseName, TheCanonical.CourseEmail);
//         var redirect = Assert.IsType<RedirectToActionResult>(result);
//         Assert.Equal(nameof(controller.Index), redirect.ActionName);
//     }

//     [Fact]
//     public async Task CreateCourse_POST_Returns_View_On_Exception()
//     {
//         service
//             .Setup(a => a.CreateCourse(It.IsAny<string>(), It.IsAny<string>()))
//             .ThrowsAsync(new CourseNameCanNotBeEmpty());
//         var result = await controller.CreateCourse("", TheCanonical.CourseEmail);
//         var view = Assert.IsType<ViewResult>(result);
//         var model = Assert.IsType<CreateCourseViewModel>(view.Model);
//         Assert.Equal("", model.Name);
//         Assert.Equal(TheCanonical.CourseEmail, model.Email);
//     }

//     [Fact]
//     public async Task CreateCourse_POST_Returns_View_With_ModelError_On_Exception()
//     {
//         service
//             .Setup(a => a.CreateCourse(It.IsAny<string>(), It.IsAny<string>()))
//             .ThrowsAsync(new CourseNameCanNotBeEmpty());
//         await controller.CreateCourse("", TheCanonical.CourseEmail);
//         Assert.False(controller.ModelState.IsValid);
//         Assert.Contains(controller.ModelState, kvp => kvp.Value!.Errors.Any(e => e.ErrorMessage == "Course name can not be empty."));
//     }
// }