using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections;
using static Graduation.Controllers.Login;

namespace Graduation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class attendance : ControllerBase
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
        }
        [HttpPost]
        public IActionResult AuthenticateStudent_takeattendance([FromHeader] Takes takes, [FromBody] Attendance attendance )
        {
            Boolean x = false;
            MySqlConnection cnn;
            String trial = @"server=127.0.0.1;database=attendance;userid=root;password=;";
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



            if (x == true)
            {
                string sql = "INSERT INTO attendance (student_id,schedule_id,week_no) VALUES (@Value1, @Value2 , @value3 )";
                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("@Value1", attendance.student_id);
                cmd.Parameters.AddWithValue("@Value2", attendance.schedule_id);
                cmd.Parameters.AddWithValue("@Value3", attendance.week_no);
                cmd.ExecuteNonQuery();
                cnn.Close();
                var message = new { message = "Attendance have been taken" };
                return Ok(message);
            }
            else
            {
                cnn.Close();
                var message = new { message = "the student is not in this class" };
                return Ok(message);
            }
            
        }
    }
}
