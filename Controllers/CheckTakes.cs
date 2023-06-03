using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Collections;
using static Graduation.Controllers.Login;

namespace Graduation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CheckTakes : ControllerBase
    {
        public class Schedules
        {
            public int schedule_id { get; set; }
            public string course_code { get; set; }
            public int instructor_id { get; set; }
            public string day { get; set; }
            public string slots { get; set; }
            public string class_no { get; set; }
        }
        public class Teaches
        {
            public int instructor_id { get; set; }
            public string course_code { get; set; }
            public string class_no { get; set; }

        }
        public class Nfc_serial
        {
            public int serial_no { get; set; }
            public int student_id { get; set; }

        }
        public class Takes
        {
            public int student_id { get; set; }
            public string course_code { get; set; }
            public string class_no { get; set; }

        }
        public class Attendance
        {
            public int attendance_id { get; set; }
            public int student_id { get; set; }
            public int schedule_id { get; set; }
            public int week_no { get; set; }
            public int attended { get; set; }
        }
        [HttpPost]
        public IActionResult checkTakes([FromBody] Takes takes)
        {
            Boolean x = false;
            String msg = "";
            MySqlConnection cnn;
            String trial = @"server=aast-db.cf4afzenuusl.us-east-1.rds.amazonaws.com;database=attendance;userid=ahmed_admin;password=777888999;";
            cnn = new MySqlConnection(trial);
            string query = $"SELECT CASE WHEN EXISTS (SELECT * FROM takes WHERE student_id= '{takes.student_id}' AND course_code = '{takes.course_code}' AND class_no = '{takes.class_no}') THEN 'TRUE' ELSE 'FALSE' END ";
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

            if (x == true) { msg = "Exists"; }
            else { msg = "This student is not enrolled in this course."; }

            return Ok(new
            {
                Message = msg
            });

            //if (x == true)
            //{
            //    string query2 = $"SELECT CASE WHEN EXISTS (SELECT * FROM attendance WHERE student_id= '{takes.student_id}' AND schedule_id = '{attendance.schedule_id}' AND week_no = '{attendance.week_no}') THEN 'TRUE' ELSE 'FALSE' END ";


            //    string sql = "INSERT INTO attendance (student_id,schedule_id,week_no) VALUES (@Value1, @Value2 , @value3 )";
            //    MySqlCommand cmd = new MySqlCommand(sql, cnn);
            //    cmd.Parameters.AddWithValue("@Value1", attendance.student_id);
            //    cmd.Parameters.AddWithValue("@Value2", attendance.schedule_id);
            //    cmd.Parameters.AddWithValue("@Value3", attendance.week_no);
            //    cmd.Parameters.AddWithValue("1", attendance.attended);
            //    cmd.ExecuteNonQuery();
            //    cnn.Close();
            //    var message = new { message = "Attendance have been taken" };
            //    return Ok(message);
            //}
            //else
            //{
            //    cnn.Close();
            //    var message = new { message = "the student is not in this class" };
            //    return Ok(message);
            //}

        }
    }
}
