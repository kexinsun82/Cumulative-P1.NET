using CumulativePart1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using MySql.Data.MySqlClient;

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

        // GET : CoursePage/New -> A webpage that prompts the user to enter a new Course information
        [HttpGet]
        public IActionResult New()
        {
            // direct to /Views/CoursePage/New.cshtml
            return View();
        }

        // POST: CoursePage/Create -> List Courses Page with the new Course added
        // Request Header: Content-Type: application/x-www-url-formencoded
        // Request Body:
        // courseId={courseId}&courseCode={courseCode}&teacherId={teacherId}&startDate={startDate}&finishDate={finishDate}&courseName={courseName}
        [HttpPost]
        public IActionResult Create(Course CourseInfo)
        {
            int CourseId = _api.AddCourse(CourseInfo);


            return RedirectToAction("Show", new { id = CourseId });
        }

        // GET: /CoursePage/DeleteConfirm/{id} -> A webpage asking the user if they are sure they want to delete this course
        [HttpGet]
        public IActionResult DeleteConfirm(int id)
        {
            // problem: get the course information
            // given: the course id
            Course SelectedCourse = _api.FindCourse(id);

            // directs to /Views/CoursePage/DeleteConfirm.cshtml
            return View(SelectedCourse);
        }

        // POST: CoursePage/Delete/{id} -> A webpage that lists the Courses
        [HttpPost]
        public IActionResult Delete(int id)
        {
            int RowsAffected = _api.DeleteCourse(id);

            //todo: log rows affected

            //direct to Views/CoursePage/List.cshtml
            return RedirectToAction("List");
        }

    }
}
