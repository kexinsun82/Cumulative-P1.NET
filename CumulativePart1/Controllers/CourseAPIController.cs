using Microsoft.AspNetCore.Mvc;
using CumulativePart1.Models;
using MySql.Data.MySqlClient;
using System.Diagnostics;

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
                        int CourseId = Convert.ToInt32(ResultSet["courseid"]);
                        string CourseCode = ResultSet["coursecode"].ToString();
                        int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                        DateTime StartDate = Convert.ToDateTime(ResultSet["startdate"]);
                        DateTime FinishDate = Convert.ToDateTime(ResultSet["finishdate"]);
                        string CourseName = ResultSet["coursename"].ToString();
                        //Access Column information by the DB column name as an index
                        //Add the Course Name to the List
                        Course CurrentCourse = new Course()
                        {
                            CourseId = CourseId,
                            CourseCode = CourseCode,
                            TeacherId = TeacherId,
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
                        int CourseId = Convert.ToInt32(ResultSet["courseid"]);
                        string CourseCode = ResultSet["coursecode"].ToString();
                        int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                        DateTime StartDate = Convert.ToDateTime(ResultSet["startdate"]);
                        DateTime FinishDate = Convert.ToDateTime(ResultSet["finishdate"]);
                        string CourseName = ResultSet["coursename"].ToString();

                        SelectedCourse.CourseId = CourseId;
                        SelectedCourse.CourseCode = CourseCode;
                        SelectedCourse.TeacherId = TeacherId;
                        SelectedCourse.StartDate = StartDate;
                        SelectedCourse.FinishDate = FinishDate;
                        SelectedCourse.CourseName = CourseName;

                    }
                }
            }


            //Return the final list of course names
            return SelectedCourse;
        }

        /// <summary>
        /// This endpoint will receive Course Data and add the Course to the database
        /// </summary>
        /// <returns>
        /// The inserted Author Id from the database is successful. 0 or Duplicate alter is Unsuccessful.
        /// </returns>
        /// <param name="CourseInfo">The Course object to add, see example</param>
        /// <example>
        /// POST : api/CourseAPI/AddCourse
        /// Header: Content-Type: application/json
        /// Data: {"courseId": 18,"courseCode": "HTTP9999","teacherId": 3,"startDate": "2024-11-29","finishDate": "2024-12-29","courseName": "New Course Test4"}'
        /// -> 
        /// "18"
        /// </example>
        /// <example>
        /// POST : api/CourseAPI/AddCourse
        /// Header: Content-Type: application/json
        /// Data: {"courseId": 18,"courseCode": "HTTP9999","teacherId": 3,"startDate": "2024-11-29","finishDate": "2024-12-29","courseName": "New Course Test4"}'
        /// -> 
        /// "Duplicate entry '18' for key 'PRIMARY'"
        /// </example>
        [HttpPost(template: "AddCourse")]
        public int AddCourse([FromBody] Course CourseInfo)
        {
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();

                // SQL query to insert the new Course
                string query = @"
                    INSERT INTO courses (courseid, coursecode, teacherid, startdate, finishdate, coursename)
                    VALUES (@CourseId, @CourseCode, @TeacherId, @StartDate, @FinishDate, @CourseName);";

                // Create a MySQL command
                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = query;

                // Bind parameters to prevent SQL injection
                Command.Parameters.AddWithValue("@CourseId", CourseInfo.CourseId);
                Command.Parameters.AddWithValue("@CourseCode", CourseInfo.CourseCode);
                Command.Parameters.AddWithValue("@TeacherId", CourseInfo.TeacherId);
                Command.Parameters.AddWithValue("@StartDate", CourseInfo.StartDate);
                Command.Parameters.AddWithValue("@FinishDate", CourseInfo.FinishDate);
                Command.Parameters.AddWithValue("@CourseName", CourseInfo.CourseName);

                // Execute the insert query
                Command.ExecuteNonQuery();

                // Get the ID of the last inserted row —— Didn't work
                // return Convert.ToInt32(Command.LastInsertedId);
                // Directly return the provided CourseId
                return CourseInfo.CourseId;
            }
                return 0;

        }

        /// <summary>
        /// Receives an ID and deletes the Course from the system
        /// </summary>
        /// <param name="CourseId">The Course Id primary key to delete</param>
        /// <returns>
        /// 1 if successful. 0 if Unsuccessful
        /// </returns>
        /// <example>
        /// DELETE api/CourseAPI/DeleteCourse/17 -> 1
        /// DELETE api/CourseAPI/DeleteCourse/17 -> 0
        /// DELETE api/CourseAPI/DeleteCourse/-17 -> 0
        /// </example>
        [HttpDelete(template:"DeleteCourse/{CourseId}")]
        public int DeleteCourse(int CourseId)
        {

            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                string query = "delete from courses where courseid=@id";

                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = query;
                Command.Parameters.AddWithValue("@id", CourseId);
                

                
                return Command.ExecuteNonQuery();
            }

            return 0;
        }

    }
}
