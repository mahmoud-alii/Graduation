using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using static Graduation.Controllers.GetPointsAmount;
using System.Security.Cryptography.X509Certificates;

namespace Graduation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Chargeprocess : ControllerBase
    {

        [HttpPost]
        public IActionResult chargeprocess([FromBody] Transactions transactions)
        {
            DateTime today_date = DateTime.Now;
            DateTime trans_date = today_date;
            string formattedDate = trans_date.ToString("yyyy-MM-dd HH:mm:ss");
            decimal x = transactions.transaction_amount;
            MySqlConnection cnn;
            String trial = @"server=aast-db.cf4afzenuusl.us-east-1.rds.amazonaws.com;database=payment;userid=ahmed_admin;password=777888999;";
            cnn = new MySqlConnection(trial);
            cnn.Open();
            string sql = $"INSERT INTO Transactions ( student_id, merchant_id , transaction_date , transaction_amount ) VALUES ( '{transactions.student_id}', '{transactions.merchant_id}','{formattedDate}' , '{x}' )";
            MySqlCommand cmd = new MySqlCommand(sql, cnn);
            cmd.ExecuteNonQuery();
            string query = $"UPDATE points SET points_balance = points_balance + '{x}'  WHERE student_id='{transactions.student_id}'";
            MySqlCommand cmd2 = new MySqlCommand(query, cnn);
            cmd2.ExecuteNonQuery();
            cnn.Close();
            string message = "points added";
            return Ok(message);
        }
    }
}