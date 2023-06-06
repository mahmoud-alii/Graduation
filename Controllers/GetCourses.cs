using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Graduation.Controllers.CheckTakes;
using System.Collections;
using MySql.Data.MySqlClient;

namespace Graduation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetCourses : ControllerBase
    {
        [HttpPost]
        public IActionResult getCourses([FromBody] Teaches teaches)
        {
            var courses = new ArrayList();
            int i = 0;
            MySqlConnection cnn;
            String trial = @"server=aast-db.cf4afzenuusl.us-east-1.rds.amazonaws.com;database=attendance;userid=ahmed_admin;password=777888999;";
            cnn = new MySqlConnection(trial);
            string query = $"SELECT DISTINCT course_code FROM teaches WHERE instructor_id='{teaches.instructor_id}'";
            MySqlCommand command = new MySqlCommand(query, cnn);

            try
            {
                cnn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        courses.Add(reader.GetString(0));
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
                Courses = courses.ToArray(),
                NumberOfCourses = i
            });
        }
    }
}
