using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Graduation.Controllers.CheckTakes;
using MySql.Data.MySqlClient;
using static Graduation.Controllers.GetlistOfBooks;

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
            var message = "";
            MySqlConnection cnn;
            //String trial = @"server=127.0.0.1;database=attendance;userid=root;password=;";
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
                string query2 = $"INSERT INTO attendance (student_id, schedule_id, week_number, attended) VALUES ('{attendance.student_id}', '{attendance.schedule_id}', '{attendance.week_no}','{1}')";
                //string query2 = $"INSERT INTO attendance1 (student_id,schedule_id,week_number,attended) VALUES (@Value1, @Value2 , @Value3, @Value4 )";
                MySqlCommand cmd = new MySqlCommand(query2, cnn);
                //cmd.Parameters.AddWithValue("@Value1", attendance.student_id);
                //cmd.Parameters.AddWithValue("@Value2", attendance.schedule_id);
                //cmd.Parameters.AddWithValue("@Value3", attendance.week_no);
                //cmd.Parameters.AddWithValue("@Value4", 1);
                cmd.ExecuteNonQuery();
                cnn.Close();
                message = "Attendance taken";
                return Ok(new { message = message });
            }
            else
            {
                int a = 2;
                string query3 = $"SELECT attended FROM attendance WHERE student_id = '{attendance.student_id}' AND schedule_id = '{attendance.schedule_id}' AND week_number = '{attendance.week_no}'";
                MySqlCommand cmnd = new MySqlCommand(query3, cnn);
                try
                {
                    cnn.Close() ;
                    cnn.Open();
                    MySqlDataReader reader = cmnd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            a = reader.GetInt32(0);
                        }
                    }
                    reader.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error" + e.Message);
                }

                if(a == 0) {
                    string query4 = $"UPDATE attendance SET attended = '{1}' WHERE student_id= '{attendance.student_id}' AND schedule_id = '{attendance.schedule_id}' AND week_number = '{attendance.week_no}';";
                    MySqlCommand cmnd2 = new MySqlCommand(query4, cnn);
                    cmnd2.ExecuteNonQuery();
                    message = "Attendance updated successfully";
                }
                else if (a == 1)
                {
                    message = "Already taken";
                }
                cnn.Close();
                return Ok(new { Message = message});
            }
        }
    }
}
