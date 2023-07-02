using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using static Graduation.Controllers.GetPointsAmount;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authorization;

namespace Graduation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Paymentprocess : ControllerBase
    {

        [HttpPost]
        //[Authorize]
        public IActionResult paymentprocess([FromBody] Transactions transactions )
        {
            DateTime today_date = DateTime.Now;
            decimal points_amount = 0;
            decimal trans_amount = transactions.transaction_amount;
            MySqlConnection cnn;
            String trial = @"server=aast-db.cf4afzenuusl.us-east-1.rds.amazonaws.com;database=payment;userid=ahmed_admin;password=777888999;";
            cnn = new MySqlConnection(trial);
            string query = $"SELECT points_balance FROM points WHERE student_id='{transactions.student_id}'";
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
           int y = Convert.ToInt32(trans_amount);
            if (points_amount > trans_amount || points_amount == trans_amount)
            {
                cnn.Open();
                DateTime trans_date = today_date;
                string formattedDate = trans_date.ToString("yyyy-MM-dd HH:mm:ss");
                string sql = $"INSERT INTO Transactions ( student_id, merchant_id , transaction_date , transaction_amount ) VALUES ( '{transactions.student_id}', '{transactions.merchant_id}','{formattedDate}' , '{trans_amount}' )";
                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                cmd.ExecuteNonQuery();
                decimal x = points_amount - trans_amount ;
                string sql2 = $"UPDATE points SET points_balance = '{x}'  WHERE student_id='{transactions.student_id}'";
                MySqlCommand cmd2 = new MySqlCommand(sql2, cnn);
                cmd2.ExecuteNonQuery();
                cnn.Close();
                string message = " transcation done ";
                return Ok(message);
            }
            else
            {
                string message = "the customer doesn't have enough points ";
                return Ok(message);
            }
        }
    }
}