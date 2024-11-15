using CumulativePart1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CumulativePart1.Controllers
{
    public class StudentPageController : Controller
    {
        // GET: StudentPageController
        private readonly StudentAPIController _api;

        public StudentPageController(StudentAPIController api)
        {
            _api = api;
        }

        // GET: api/StudentPage/List
        public IActionResult List()
        {
            List<Student> Students = _api.ListStudentsInfo();
            return View(Students);
        }

        // GET: api/StudentPage/Show/{id}
        public IActionResult Show(int id)
        {
            Student SelectedStudent = _api.FindStudent(id);

            if (SelectedStudent == null || SelectedStudent.StudentId == 0)
            {
            TempData["ErrorMessage"] = "Student not found, please enter a valid ID.";
            return RedirectToAction("List");
            }

            return View(SelectedStudent);
        }
    }
}
