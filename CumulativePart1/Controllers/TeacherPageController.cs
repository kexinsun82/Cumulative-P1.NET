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

        // GET: /TeacherPage/DeleteConfirm/{id} -> A webpage asking the user if they are sure they want to delete this teacher
        [HttpGet]
        public IActionResult DeleteConfirm(int id)
        {
            // problem: get the teacher information
            // given: the teacher id
            Teacher SelectedTeacher = _api.FindTeacher(id);

            // directs to /Views/TeacherPage/DeleteConfirm.cshtml
            return View(SelectedTeacher);
        }

        // POST: TeacherPage/Delete/{id} -> A webpage that lists the teachers
        [HttpPost]
        public IActionResult Delete(int id)
        {
            int RowsAffected = _api.DeleteTeacher(id);

            if (RowsAffected > 0)
            {
                // Success message
                TempData["SuccessMessage"] = "Teacher deleted successfully."; 
            }
            else
            {
                // Error message
                TempData["ErrorMessage"] = "Teacher not found or could not be deleted."; 
            }

            //direct to Views/TeacherPage/List.cshtml
            return RedirectToAction("List");
        }

        // GET: TeacherPage/Edit/28 -> A webpage which asks the user to enter updated teacher information given the original teacher
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Teacher SelectedTeacher = _api.FindTeacher(id);
            // Direct to Views/Teacher/Edit.cshtml
            return View(SelectedTeacher);

        }

        // POST: /TeacherPage/Update/{id} -> Shows the teacher which updated
        [HttpPost]
        public IActionResult Update(int id, string TeacherFName, string TeacherLName,  string EmployeeNumber, DateTime HireDate, decimal Salary)
        {

            // Error Handling on Update when trying to update a teacher that does not exist
            Teacher ExistingTeacher = _api.FindTeacher(id);
            if (ExistingTeacher == null || ExistingTeacher.TeacherId == 0)
            {
                TempData["ErrorMessage"] = "Teacher not found. Please enter a valid ID.";
                return RedirectToAction("List");
            }

            // Error Handling on Update when the Teacher Name is empty
            if (string.IsNullOrWhiteSpace(TeacherFName) || string.IsNullOrWhiteSpace(TeacherLName))
            {
                TempData["ErrorMessage"] = "First Name and Last Name cannot be empty.";
                return RedirectToAction("Edit", new { id });
            }

            // Error Handling on Update when the Teacher Hire Date is in the future
            if (HireDate > DateTime.Now)
            {
                TempData["ErrorMessage"] = "Hire Date can NOT be in the future.";
                return RedirectToAction("Edit", new { id });
            }

            // Error Handling on Update when the Salary is less than 0
            if (Salary < 0)
            {
                TempData["ErrorMessage"] = "Salary cannot be less than 0.";
                return RedirectToAction("Edit", new { id });
            }

            Teacher UpdatedTeacher = new Teacher();

            UpdatedTeacher.TeacherFName = TeacherFName;
            UpdatedTeacher.TeacherLName = TeacherLName;
            UpdatedTeacher.EmployeeNumber = EmployeeNumber;
            UpdatedTeacher.HireDate = HireDate;
            UpdatedTeacher.Salary = Salary;

            _api.UpdateTeacher(id, UpdatedTeacher);


            // redirect to /TeacherPage/Show/{teacherid}
            return RedirectToAction("Show", new { id = id });

        }

    }
}
