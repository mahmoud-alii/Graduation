﻿using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        public IActionResult AuthenticateStudent_Course([FromBody] Takes takes, bool x)
        {
            MySqlConnection cnn;
            String trial = @"server=127.0.0.1;database=attendance;userid=root;password=;";
            cnn = new MySqlConnection(trial);
            string query = $"SELECT CASE WHEN EXISTS (SELECT * FROM takes WHERE student_id= '{takes.student_id}' AND course_code = '{takes.course_code}' AND class_no = '{takes.class_no}') THEN 'TRUE' ELSE 'FALSE' END ";
            MySqlCommand command = new MySqlCommand(query, cnn);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("https://localhost:7195/api/GetStudentID" + stundent_id);
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

            

            if (x== true)
            {
                string sql = "INSERT INTO attendance (student_id,schedule_id,status) VALUES (@Value1, @Value2 , @value3)";
                command.Parameters.AddWithValue("@Value1", "Hello");
                command.Parameters.AddWithValue("@Value2", "World");
                command.Parameters.AddWithValue("@Value3", "World");
                command.ExecuteNonQuery();
            }
            else
            {

            }
            cnn.Close();
            return Ok(new
            {
                isFound = x
            });
        }
    }
}
