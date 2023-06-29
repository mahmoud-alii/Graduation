using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using static Graduation.Controllers.GetPointsAmount;
using System.Security.Cryptography.X509Certificates;
using System.Drawing.Imaging;
using System.Collections;
using System.Security.Policy;
using Microsoft.AspNetCore.Authorization;

namespace Graduation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class Gettransactions : ControllerBase
    {

        [HttpPost]
        [Authorize]
        public IActionResult gettransactions([FromBody] Transactions transactions )
        {
            ArrayList transactions_records = new ArrayList();
            MySqlConnection cnn;
            String trial = @"server=aast-db.cf4afzenuusl.us-east-1.rds.amazonaws.com;database=payment;userid=ahmed_admin;password=777888999;";
            cnn = new MySqlConnection(trial);
            string query = $"SELECT * FROM Transactions WHERE student_id='{transactions.student_id}'";
            MySqlCommand command = new MySqlCommand(query, cnn);
            try
            {
                cnn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        ArrayList list = new ArrayList
                        {
                            reader.GetInt32(0),
                            reader.GetInt32(1),
                            reader.GetInt32(2),
                            reader.GetDateTime(3),
                            reader.GetDecimal(4),
                        };
                        transactions_records.Add(list);

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
                TransactionsRecords = transactions_records.ToArray()
            });
        }
    }
}

