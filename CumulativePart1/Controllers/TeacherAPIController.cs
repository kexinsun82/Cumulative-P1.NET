using School.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using CumulativePart1.Models;

namespace School.Controllers
{
    [Route("api/Teacher")]
    [ApiController]
    public class TeacherAPIController : ControllerBase
    {
        // get information about the database
        private readonly SchoolDbContext _context;

        public TeacherAPIController(SchoolDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// This method will return information on all teachers
        /// </summary>
        /// <example>
        /// GET api/
        /// </example>
        /// <returns>
        /// A list of teacher informations
        /// </returns>
        [HttpGet]
        [Route(template:"ListTeachersInfo")]
        public List<Teacher> ListTeachersInfo()
        {
            // create a empty list for the article titles
            List<Teacher> Teachers = new List<Teacher>();

            // create a connection to the database
            MySqlConnection Connection = _context.AccessDatabase();

            // open the connection to the database
                Connection.Open();

            // create a database command
                MySqlCommand Command = Connection.CreateCommand();

            // create a string for the query ""
                string query = "select * from teachers";

            // set the database command text to the query
                Command.CommandText = query;

            // gather Result Set of Query into a variable
                MySqlDataReader ResultSet = Command.ExecuteReader();

            // read through the results in a loop
                while (ResultSet.Read()) 
                {
                    // for each results, gather the teachers info
                    int ID = Convert.ToInt32(ResultSet["teacherid"]);
                    string FirstName = ResultSet["teacherfname"].ToString();
                    string LastName = ResultSet["teacherlname"].ToString();
                    string EmployeeNumber = ResultSet["employeenumber"].ToString();
                    DateTime HireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                    decimal Salary = Convert.ToDecimal(ResultSet["salary"]);
                    //Access Column information by the DB column name as an index
                    // string TeacherInfo = $"{TeacherFName} {TeacherLName}";
                    //Add the Author Name to the List
                    Teacher CurrentTeacher = new Teacher()
                    {
                        TeacherId = ID,
                        TeacherFName = FirstName,
                        TeacherLName = LastName,
                        EmployeeNumber = EmployeeNumber,
                        HireDate = HireDate,
                        Salary = Salary
                    };

                    Teachers.Add(CurrentTeacher);
                }

                ResultSet.Close();

                Connection.Close();

                // return the list of teachers

                return Teachers;
        }

        /// <summary>
        /// Returns a teacher in the database by their ID
        /// </summary>
        /// <example>
        /// GET api/Teacher/FindTeacher/1 -> {}
        /// </example>
        /// <returns>
        /// A matching teacher object by its ID. Empty object if teacher not found
        /// </returns>
        [HttpGet]
        [Route(template: "FindTeacher/{id}")]
        public Teacher FindTeacher(int id)
        {
            // Empty Teacher
            Teacher SelectedTeacher = new Teacher();

            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = "select * from teachers where teacherid=@id";
                Command.Parameters.AddWithValue("@id", id);

                // Gather Result set of query into a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    // Loop through each row the result set
                    while (ResultSet.Read())
                    {
                        int ID = Convert.ToInt32(ResultSet["teacherid"]);
                        string FirstName = ResultSet["teacherfname"].ToString();
                        string LastName = ResultSet["teacherlname"].ToString();
                        string EmployeeNumber = ResultSet["employeenumber"].ToString();
                        DateTime HireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                        decimal Salary = Convert.ToDecimal(ResultSet["salary"]);

                        SelectedTeacher.TeacherId = ID;
                        SelectedTeacher.TeacherFName = FirstName;
                        SelectedTeacher.TeacherLName = LastName;
                        SelectedTeacher.EmployeeNumber = EmployeeNumber;
                        SelectedTeacher.HireDate = HireDate;
                        SelectedTeacher.Salary = Salary;
                    }
                }
            }

            // Return the teacher's info by ID
            return SelectedTeacher;
        }
    }
}