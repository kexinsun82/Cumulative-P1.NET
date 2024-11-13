using School.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

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
        /// This method will return teachers' information
        /// </summary>
        /// <returns>A list of teacher names</returns>
        [HttpGet]
        [Route(template:"ListTeachersInfo")]
        public List<string> ListTeachersInfo()
        {
            // create a empty list for the article titles
            List<string> TeachersInfo = new List<string>();

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
                    string TeacherFName = ResultSet["teacherfname"].ToString();
                    string TeacherLName = ResultSet["teacherlname"].ToString();
                    //Access Column information by the DB column name as an index
                    string TeacherInfo = $"{TeacherFName} {TeacherLName}";
                    //Add the Author Name to the List
                    TeachersInfo.Add(TeacherInfo);
                }

                ResultSet.Close();

                Connection.Close();

                // return the teachers information

                return TeachersInfo;
        }
    }
}
