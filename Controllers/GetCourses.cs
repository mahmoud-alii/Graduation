using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Graduation.Controllers.CheckTakes;
using System.Collections;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Authorization;

namespace Graduation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetCourses : ControllerBase
    {
        [HttpPost]
        [Authorize]
        public IActionResult getCourses([FromBody] Teaches teaches)
        {
            ArrayList courseList = new ArrayList();
            int i = 0;
            MySqlConnection cnn;
            String trial = @"server=aast-db.cf4afzenuusl.us-east-1.rds.amazonaws.com;database=attendance;userid=ahmed_admin;password=777888999;";
            cnn = new MySqlConnection(trial);
            //string query = $"SELECT DISTINCT course_code FROM teaches WHERE instructor_id='{teaches.instructor_id}'";
            string query = $"SELECT c.course_code, c.course_name FROM courses c JOIN teaches t ON c.course_code = t.course_code WHERE t.instructor_id = '{teaches.instructor_id}' GROUP BY c.course_code, c.course_name";
            
            MySqlCommand command = new MySqlCommand(query, cnn);

            try
            {
                cnn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ArrayList course = new ArrayList
                        {
                            reader.GetString(0),
                            reader.GetString(1)
                        };
                        courseList.Add(course);
                        i++;
                    }
                }
                reader.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine("Error" + e.Message);
            }

            cnn.Close();

            return Ok(new
            {
                Courses = courseList.ToArray(),
                NumberOfCourses = i
            });
        }
    }
}
