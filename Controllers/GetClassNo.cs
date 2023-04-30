using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Graduation.Controllers.attendance;
using MySql.Data.MySqlClient;

namespace Graduation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetClassNo : ControllerBase
    {
        [HttpPost]
        public IActionResult getClassNo([FromBody] Schedules schedules)
        {
            String class_no = "";
            String schedule_id = "";
            MySqlConnection cnn;
            String trial = @"server=127.0.0.1;database=attendance;userid=root;password=;";
            cnn = new MySqlConnection(trial);
            string query = $"SELECT class_no, schedule_id FROM schedules WHERE course_code='{schedules.course_code}' AND instructor_id='{schedules.instructor_id}' AND slots='{schedules.slots}'";
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
                ClassNumber = class_no
                ,
                ScheduleId = schedule_id
            });
        }
    }
}
