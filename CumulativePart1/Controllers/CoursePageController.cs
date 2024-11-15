using CumulativePart1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CumulativePart1.Controllers
{
    public class CoursePageController : Controller
    {
        // GET: CoursePageController
        private readonly CourseAPIController _api;

        public CoursePageController(CourseAPIController api)
        {
            _api = api;
        }

        // GET: api/CoursePage/List
        public IActionResult List()
        {
            List<Course> Courses = _api.ListCoursesInfo();
            return View(Courses);
        }

        // GET: api/CoursePage/Show/{id}
        public IActionResult Show(int id)
        {
            Course SelectedCourse = _api.FindCourse(id);

            if (SelectedCourse == null || SelectedCourse.CourseId == 0)
            {
            TempData["ErrorMessage"] = "Course not found, please enter a valid ID.";
            return RedirectToAction("List");
            }

            return View(SelectedCourse);
        }
    }
}
