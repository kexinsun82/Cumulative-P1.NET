using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CumulativePart1.Models;
using System;
using MySql.Data.MySqlClient;

namespace CumulativePart1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseAPIController : ControllerBase
    {
        private readonly SchoolDbContext _context;
        // dependency injection of database context
        public CourseAPIController(SchoolDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns a list of Courses in the system
        /// </summary>
        /// <example>
        /// GET api/Course/ListCourses -> [{},..]
        /// </example>
        /// <returns>
        /// A list of Course object 
        /// </returns>
        [HttpGet]
        [Route(template:"ListCoursesInfo")]
        public List<Course> ListCoursesInfo()
        {
            // Create an empty list of Courses
            List<Course> Courses = new List<Course>();

            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();

                //SQL QUERY
                Command.CommandText = "select * from courses";

                // Gather Result Set of Query into a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    //Loop Through Each Row the Result Set
                    while (ResultSet.Read())
                    {
                        // for each results, gather the Courses info
                        int ID = Convert.ToInt32(ResultSet["courseid"]);
                        string CourseCode = ResultSet["coursecode"].ToString();
                        int TeacherID = Convert.ToInt32(ResultSet["teacherid"]);
                        DateTime StartDate = Convert.ToDateTime(ResultSet["startdate"]);
                        DateTime FinishDate = Convert.ToDateTime(ResultSet["finishdate"]);
                        string CourseName = ResultSet["coursename"].ToString();
                        //Access Column information by the DB column name as an index
                        //Add the Course Name to the List
                        Course CurrentCourse = new Course()
                        {
                            CourseId = ID,
                            CourseCode = CourseCode,
                            TeacherID = TeacherID,
                            StartDate = StartDate,
                            FinishDate = FinishDate,
                            CourseName = CourseName
                        };

                       Courses.Add(CurrentCourse);

                    }
                }                    
            }

            //Return the final list of Courses
            return Courses;
        }


        /// <summary>
        /// Returns a course in the database by their ID
        /// </summary>
        /// <example>
        /// GET api/Course/FindCourse/1 -> {}
        /// GET api/Course/FindCourse/40 -> {}
        /// </example>
        /// <returns>
        /// A matching Course object by its ID. Empty object if Course not found
        /// </returns>
        [HttpGet]
        [Route(template: "FindCourse/{id}")]
        public Course FindCourse(int id)
        {
            
            //Empty Course
            Course SelectedCourse = new Course();

            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();

                Command.CommandText = "select * from courses where courseid=@id";
                Command.Parameters.AddWithValue("@id", id);

                // Gather Result Set of Query into a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    //Loop Through Each Row the Result Set
                    while (ResultSet.Read())
                    {
                        //Access Column information by the DB column name as an index
                        int ID = Convert.ToInt32(ResultSet["courseid"]);
                        string CourseCode = ResultSet["coursecode"].ToString();
                        int TeacherID = Convert.ToInt32(ResultSet["teacherid"]);
                        DateTime StartDate = Convert.ToDateTime(ResultSet["startdate"]);
                        DateTime FinishDate = Convert.ToDateTime(ResultSet["finishdate"]);
                        string CourseName = ResultSet["coursename"].ToString();

                        SelectedCourse.CourseId = ID;
                        SelectedCourse.CourseCode = CourseCode;
                        SelectedCourse.TeacherID = TeacherID;
                        SelectedCourse.StartDate = StartDate;
                        SelectedCourse.FinishDate = FinishDate;
                        SelectedCourse.CourseName = CourseName;

                    }
                }
            }


            //Return the final list of course names
            return SelectedCourse;
        }
    }
}
