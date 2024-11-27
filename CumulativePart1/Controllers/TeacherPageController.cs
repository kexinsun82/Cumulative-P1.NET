using CumulativePart1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CumulativePart1.Controllers
{
    public class TeacherPageController : Controller
    {
        // GET: TeacherPageController
        private readonly TeacherAPIController _api;
        // Ideally would have an article service where both the MVC and API can call the service
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

        // GET : TeacherPage/New -> A webpage that prompts the user to enter a new teacher information
        [HttpGet]
        public IActionResult New()
        {
            // direct to /Views/TeacherPage/New.cshtml
            return View();
        }

        // POST: TeacherPage/Create -> List Teachers Page with the new Teacher added
        // Request Header: Content-Type: application/x-www-url-formencoded
        // Request Body:
        // TeacherFName={TeacherFName}&TeacherLName={TeacherLName}&EmployeeNumber={EmployeeNumber}&HireDate={HireDate}&Salary={Salary}
        [HttpPost]
        public IActionResult Create(string TeacherFName, string TeacherLName, string EmployeeNumber, DateTime HireDate, Decimal Salary)
        {
            Debug.WriteLine($"First Name: {TeacherFName}");
            Debug.WriteLine($"Last Name: {TeacherLName}");
            Debug.WriteLine($"Employee Number: {EmployeeNumber}");
            Debug.WriteLine($"Hire Date: {HireDate}");
            Debug.WriteLine($"Salary: {Salary}");

            //a new teacher object
            Teacher NewTeacher = new Teacher();
            NewTeacher.TeacherFName = TeacherFName;
            NewTeacher.TeacherLName = TeacherLName;
            NewTeacher.EmployeeNumber = EmployeeNumber;
            NewTeacher.HireDate = HireDate;
            NewTeacher.Salary = Salary;

            //add the new teacher to the database
            int TeacherId = _api.AddTeacher(NewTeacher);


            //redirect to /TeacherPage/List/{teacherId}
            return RedirectToAction("List", new { id=TeacherId });
        }
    }
}
