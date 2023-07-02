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
    public class GetSlots : ControllerBase
    {
        [HttpPost]
        //[Authorize]
        public IActionResult getSlots([FromBody] Schedules schedules)
        {
            ArrayList slotsList = new ArrayList();
            int i = 0;
            MySqlConnection cnn;
            String trial = @"server=aast-db.cf4afzenuusl.us-east-1.rds.amazonaws.com;database=attendance;userid=ahmed_admin;password=777888999;";
            cnn = new MySqlConnection(trial);
            //string query = $"SELECT DISTINCT course_code FROM teaches WHERE instructor_id='{teaches.instructor_id}'";
            string query = $"SELECT day, slots FROM schedules WHERE instructor_id='{schedules.instructor_id}' AND course_code='{schedules.course_code}'";


            MySqlCommand command = new MySqlCommand(query, cnn);

            try
            {
                cnn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ArrayList slot = new ArrayList
                        {
                            reader.GetString(0),
                            reader.GetString(1)
                        };
                        slotsList.Add(slot);
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
                Courses = slotsList.ToArray(),
                Slots = i
            });
        }
    }
}
