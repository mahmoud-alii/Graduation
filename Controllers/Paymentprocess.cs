using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Graduation.Controllers.CheckTakes;
using MySql.Data.MySqlClient;
using System.Security.Cryptography.X509Certificates;

namespace Graduation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Paymentprocess : ControllerBase
    {
        public class Points
        {
            public int student_id { get; set; }
            public int points { get; set; }

        }
        public class Payment_records
        {
            public int student_id { get; set; }
            public int points { get; set; }      
            public DateTime date { get; set; }
            public string status { get; set; }

        }

        [HttpPost]
        public IActionResult paymentprocess([FromBody] Points points)
        {
            int student_id = 0;
            MySqlConnection cnn;
            String trial = @"server=aast-db.cf4afzenuusl.us-east-1.rds.amazonaws.com;database=payment;userid=ahmed_admin;password=777888999;";
            cnn = new MySqlConnection(trial);
            string query = $"SELECT Point FROM has WHERE student_id='{points.student_id}'";
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

