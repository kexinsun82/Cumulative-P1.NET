using CumulativePart1.Models;
using Microsoft.AspNetCore.Mvc;

namespace CumulativePart1.Controllers
{
    public class TeacherPageController : Controller
    {
        // GET: TeacherPageController
        private readonly TeacherAPIController _api;

        public TeacherPageController(TeacherAPIController api)
        {
            _api = api;
        }

        // GET: api/TeacherPage/List
        public IActionResult List()
        {
            List<Teacher> Teachers = _api.ListTeachersInfo();
            return View(Teachers);
        }

        // GET: api/Teacher/Show/{id}
        public IActionResult Show(int id)
        {
            Teacher SelectedTeacher = _api.FindTeacher(id);

            if (SelectedTeacher == null || SelectedTeacher.TeacherId == 0)
            {
            TempData["ErrorMessage"] = "Teacher not found, please enter a valid ID.";
            return RedirectToAction("List");
            }

            return View(SelectedTeacher);
        }
    }
}
