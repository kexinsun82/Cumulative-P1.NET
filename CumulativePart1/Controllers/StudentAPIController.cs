using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CumulativePart1.Models;
using System;
using MySql.Data.MySqlClient;

namespace CumulativePart1.Controllers
{
    [Route("api/Student")]
    [ApiController]
    public class StudentAPIController : ControllerBase
    {
        private readonly SchoolDbContext _context;
        // dependency injection of database context
        public StudentAPIController(SchoolDbContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Returns a list of Students in the system
        /// </summary>
        /// <example>
        /// GET api/Student/ListStudents -> [{ "studentId": 1,"studentFName": "Sarah","studentLName": "Valdez","studentNumber": "N1678","enrolDate": "2018-06-18T00:00:00"},{"studentId": 2,"studentFName": "Jennifer","studentLName": "Faulkner","studentNumber": "N1679","enrolDate": "2018-08-02T00:00:00"},{"studentId": 3,"studentFName": "Austin","studentLName": "Simon","studentNumber": "N1682","enrolDate": "2018-06-14T00:00:00"},..]
        /// </example>
        /// <returns>
        /// A list of student object 
        /// </returns>
        [HttpGet]
        [Route(template:"ListStudentsInfo")]
        public List<Student> ListStudentsInfo()
        {
            // Create an empty list of Students
            List<Student> Students = new List<Student>();

            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();

                //SQL QUERY
                Command.CommandText = "select * from students";

                // Gather Result Set of Query into a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    //Loop Through Each Row the Result Set
                    while (ResultSet.Read())
                    {
                        // for each results, gather the students info
                        int ID = Convert.ToInt32(ResultSet["studentid"]);
                        string FirstName = ResultSet["studentfname"].ToString();
                        string LastName = ResultSet["studentlname"].ToString();
                        string StudentNum = ResultSet["studentnumber"].ToString();
                        DateTime EnrolDate = Convert.ToDateTime(ResultSet["enroldate"]);
                        //Access Column information by the DB column name as an index
                        //Add the Student Name to the List
                        Student CurrentStudent = new Student()
                        {
                            StudentId = ID,
                            StudentFName = FirstName,
                            StudentLName = LastName,
                            StudentNumber = StudentNum,
                            EnrolDate = EnrolDate
                        };

                        Students.Add(CurrentStudent);

                    }
                }                    
            }
            

            //Return the final list of Students
            return Students;
        }


        /// <summary>
        /// Returns an student in the database by their ID
        /// </summary>
        /// <example>
        /// GET api/Student/FindStudent/1 -> {"studentId": 1,"studentFName": "Sarah","studentNumber": "N1678","enrolDate": "2018-06-18T00:00:00"}
        /// GET api/Student/FindStudent/40 -> {"studentId": 0,"studentFName": null,"studentLName": null,"studentNumber": null,"enrolDate": "0001-01-01T00:00:00"}
        /// </example>
        /// <returns>
        /// A matching Student object by its ID. Empty object if Student not found
        /// </returns>
        [HttpGet]
        [Route(template: "FindStudent/{id}")]
        public Student FindStudent(int id)
        {
            
            //Empty Student
            Student SelectedStudent = new Student();

            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();

                Command.CommandText = "select * from students where studentid=@id";
                Command.Parameters.AddWithValue("@id", id);

                // Gather Result Set of Query into a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    //Loop Through Each Row the Result Set
                    while (ResultSet.Read())
                    {
                        //Access Column information by the DB column name as an index
                        int ID = Convert.ToInt32(ResultSet["studentid"]);
                        string FirstName = ResultSet["studentfname"].ToString();
                        string LastName = ResultSet["studentlname"].ToString();
                        string StudentNumber = ResultSet["studentnumber"].ToString();
                        DateTime EnrolDate = Convert.ToDateTime(ResultSet["enroldate"]);

                        SelectedStudent.StudentId = ID;
                        SelectedStudent.StudentFName = FirstName;
                        SelectedStudent.StudentLName = LastName;
                        SelectedStudent.StudentNumber = StudentNumber;
                        SelectedStudent.EnrolDate = EnrolDate;
                    }
                }
            }


            //Return the final list of student names
            return SelectedStudent;
        }
    }
}
