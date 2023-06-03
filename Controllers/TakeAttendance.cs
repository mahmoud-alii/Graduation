using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Graduation.Controllers.CheckTakes;
using MySql.Data.MySqlClient;

namespace Graduation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TakeAttendance : ControllerBase
    {
        [HttpPost]
        public IActionResult takeAttendance([FromBody] Attendance attendance)
        {
            Boolean x = true;
            MySqlConnection cnn;
            String trial = @"server=aast-db.cf4afzenuusl.us-east-1.rds.amazonaws.com;database=attendance;userid=ahmed_admin;password=777888999;";
            cnn = new MySqlConnection(trial);
            string query = $"SELECT CASE WHEN EXISTS (SELECT * FROM attendance WHERE student_id= '{attendance.student_id}' AND schedule_id = '{attendance.schedule_id}' AND week_number = '{attendance.week_no}') THEN 'TRUE' ELSE 'FALSE' END ";
            MySqlCommand command = new MySqlCommand(query, cnn);
            try
            {
                cnn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        x = reader.GetBoolean(0);
                    }
                }
                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error" + e.Message);
            }

            if (x == false) { 
                string query2 = $"INSERT INTO attendance (student_id,schedule_id,week_number,attended) VALUES (@Value1, @Value2 , @Value3, @Value4 )";
                MySqlCommand cmd = new MySqlCommand(query2, cnn);
                cmd.Parameters.AddWithValue("@Value1", attendance.student_id);
                cmd.Parameters.AddWithValue("@Value2", attendance.schedule_id);
                cmd.Parameters.AddWithValue("@Value3", attendance.week_no);
                cmd.Parameters.AddWithValue("@Value4", 1);
                cmd.ExecuteNonQuery();
                cnn.Close();
                var message = new { message = "Attendance taken" };
                return Ok(message);
            }
            else
            {
                //string query3 = $"SELECT attended FROM attendance WHERE student_id= '{attendance.student_id}' AND schedule_id = '{attendance.schedule_id}' AND week_no = '{attendance.week_no}'";
                cnn.Close();
                var message = new { message = "Already taken" };
                return Ok(message);
            }
        }
    }
}
