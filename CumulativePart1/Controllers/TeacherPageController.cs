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

        // GET: TeacherPage/List
        public IActionResult List()
        {
            List<Teacher> Teachers = _api.ListTeachersInfo();
            return View(Teachers);
        }

        // GET: TeacherPage/Show/{id}
        public IActionResult Show(int id)
        {
            Teacher SelectedTeacher = _api.FindTeacher(id);
            return View(SelectedTeacher);
        }
    }
}
