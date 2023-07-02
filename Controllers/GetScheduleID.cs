using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Graduation.Controllers.CheckTakes;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Authorization;

namespace Graduation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetScheduleID : ControllerBase
    {
        [HttpPost]
        //[Authorize]
        public IActionResult getScheduleID([FromBody] Schedules schedules)
        {
            String class_no = "";
            String schedule_id = "";
            MySqlConnection cnn;
            String trial = @"server=aast-db.cf4afzenuusl.us-east-1.rds.amazonaws.com;database=attendance;userid=ahmed_admin;password=777888999;";
            cnn = new MySqlConnection(trial);
            string query = $"SELECT class_no, schedule_id FROM schedules WHERE course_code='{schedules.course_code}' AND instructor_id='{schedules.instructor_id}' AND day='{schedules.day}' AND slots='{schedules.slots}'";
            MySqlCommand command = new MySqlCommand(query, cnn);

            try
            {
                cnn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        class_no = reader.GetString(0);
                        schedule_id = reader.GetString(1);
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
                ClassNumber = class_no,
                ScheduleId = schedule_id
            });
        }
    }
}
