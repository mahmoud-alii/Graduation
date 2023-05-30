﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Graduation.Controllers.attendance;
using MySql.Data.MySqlClient;
using System.Security.Cryptography.X509Certificates;

namespace Graduation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetStudentID : ControllerBase
    {
        [HttpPost]
        public IActionResult getStudentID([FromBody] Nfc_serial nfc_Serial )
        {
            int student_id = 0;
            MySqlConnection cnn;
            String trial = @"server=aast-db.cf4afzenuusl.us-east-1.rds.amazonaws.com;database=attendance;userid=ahmed_admin;password=777888999;";
            cnn = new MySqlConnection(trial);
            string query = $"SELECT student_id FROM nfc_serial WHERE serial_no='{nfc_Serial.serial_no}'";
            MySqlCommand command = new MySqlCommand(query, cnn);
            try
            {
                cnn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        student_id = reader.GetInt32(0);
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
                StudentID = student_id
            });
        }
    }
}
