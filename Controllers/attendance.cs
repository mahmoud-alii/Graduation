﻿using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
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

        [HttpPost]
        public IActionResult GetCourses([FromBody] Teaches teaches)
        {
            string[] courses = null ;
            int i =0;
            MySqlConnection cnn;
            String trial = @"server=127.0.0.1;database=attendance;userid=root;password=;";
            cnn = new MySqlConnection(trial);
            string query = $"SELECT course_code FROM teaches WHERE instructor_id='{teaches.instructor_id}'";
            MySqlCommand command = new MySqlCommand(query, cnn);

            try
            {
                cnn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        courses[i] = reader.GetString(i);
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
                Courses= courses , NumberOfCourses=i
            });
        }

        [HttpPost]
        public IActionResult GetClassNo([FromBody] Schedules schedules)
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

            return Ok(new {
                ClassNumber = class_no 
                , ScheduleId = schedule_id
            });
        }


        //[HttpPost]
        //public IActionResult RegisterAttendance([FromBody] Student student)
        //{
        //    String pass = "";
        //    MySqlConnection cnn;
        //    String trial = @"server=127.0.0.1;database=attendance;userid=root;password=;";
        //    cnn = new MySqlConnection(trial);
        //    string query = $"SELECT pass FROM person WHERE id='{student.Id}' AND type='1'";
        //    MySqlCommand command = new MySqlCommand(query, cnn);

        //    try
        //    {
        //        cnn.Open();
        //        MySqlDataReader reader = command.ExecuteReader();

        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                pass = reader.GetString(0);
        //            }
        //        }
        //        reader.Close();

        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("Error" + e.Message);
        //    }

        //    cnn.Close();

        //    return Ok(new { AttendancePass = pass });
        //}
    }
}