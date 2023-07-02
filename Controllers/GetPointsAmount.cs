using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Security.Cryptography.X509Certificates;

namespace Graduation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetPointsAmount : ControllerBase
    {
        public class Points
        {
            public int student_id { get; set; }
            public int points_balance { get; set; }

        }
        public class Transactions
        {
            public int transaction_id { get; set; }
            public int student_id { get; set; }
            public int merchant_id { get; set; }      
            public DateTime transaction_date { get; set; }
            public decimal transaction_amount { get; set; }

        }

        [HttpPost]
        //[Authorize]
        public IActionResult getPointsAmount([FromBody] Points points)
        {
            int points_amount = 0;
            MySqlConnection cnn;
            String trial = @"server=aast-db.cf4afzenuusl.us-east-1.rds.amazonaws.com;database=payment;userid=ahmed_admin;password=777888999;";
            cnn = new MySqlConnection(trial);
            string query = $"SELECT points_balance FROM points WHERE student_id='{points.student_id}'";
            MySqlCommand command = new MySqlCommand(query, cnn);
            try
            {
                cnn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        points_amount = reader.GetInt32(0);
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
                points = points_amount
            });
        }
    }
}

