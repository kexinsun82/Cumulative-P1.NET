using CumulativePart1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Diagnostics;

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

        // GET : StudentPage/New -> A webpage that prompts the user to enter a new Student information
        [HttpGet]
        public IActionResult New()
        {
            // direct to /Views/StudentPage/New.cshtml
            return View();
        }

        // POST: StudentPage/Create -> List Students Page with the new Student added
        // Request Header: Content-Type: application/x-www-url-formencoded
        // Request Body:
        // StudentFName={StudentFName}&StudentLName={StudentLName}&StudentNumber={StudentNumber}&EnrolDate={EnrolDate}
        [HttpPost]
        public IActionResult Create(string StudentFName, string StudentLName, string StudentNumber, DateTime EnrolDate)
        {
            Debug.WriteLine($"First Name: {StudentFName}");
            Debug.WriteLine($"Last Name: {StudentLName}");
            Debug.WriteLine($"Student Number: {StudentNumber}");
            Debug.WriteLine($"Enrol Date: {EnrolDate}");

            //a new Student object
            Student NewStudent = new Student();
            NewStudent.StudentFName = StudentFName;
            NewStudent.StudentLName = StudentLName;
            NewStudent.StudentNumber = StudentNumber;
            NewStudent.EnrolDate = EnrolDate;

            //add the new Student to the database
            int StudentId = _api.AddStudent(NewStudent);


            //redirect to /StudentPage/List/{StudentId}
            return RedirectToAction("List", new { id=StudentId });
        }

        // GET: /StudentPage/DeleteConfirm/{id} -> A webpage asking the user if they are sure they want to delete this Student
        [HttpGet]
        public IActionResult DeleteConfirm(int id)
        {
            // problem: get the Student information
            // given: the Student id
            Student SelectedStudent = _api.FindStudent(id);

            // directs to /Views/StudentPage/DeleteConfirm.cshtml
            return View(SelectedStudent);
        }

        // POST: StudentPage/Delete/{id} -> A webpage that lists the Students
        [HttpPost]
        public IActionResult Delete(int id)
        {
            int RowsAffected = _api.DeleteStudent(id);

            //todo: log rows affected

            //direct to Views/StudentPage/List.cshtml
            return RedirectToAction("List");
        }

    }
}
