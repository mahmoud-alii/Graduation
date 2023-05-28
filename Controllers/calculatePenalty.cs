using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Graduation.Controllers.GetlistOfBooks;
using MySql.Data.MySqlClient;
using System.Security.Cryptography.X509Certificates;
using System.Drawing.Imaging;
using System.Collections;
using OfficeOpenXml.Table.PivotTable;

namespace Graduation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class calculatePenalty : ControllerBase
    {

        [HttpPost]
        public IActionResult getlistOfBooks([FromBody] Borrowed_books borrowed_books)
        {
            DateTime x ;
            DateTime y ;
            TimeSpan difference = new TimeSpan() ;
            MySqlConnection cnn;
            String trial = @"server=aast-db.cf4afzenuusl.us-east-1.rds.amazonaws.com;database=library;userid=ahmed_admin;password=777888999;";
            cnn = new MySqlConnection(trial);
            string query = $"SELECT due_date , returned_date FROM library.borrowed_books WHERE student_id='{borrowed_books.student_id}'and book_id'{borrowed_books.book_id}'";
            MySqlCommand command = new MySqlCommand(query, cnn);
            try
            {
                cnn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                         x = reader.GetDateTime(0);
                         y = reader.GetDateTime(1);

                         difference = y.Subtract(x);
                    }
                }
                reader.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine("Error" + e.Message);
            }
            decimal total_days = (decimal)difference.TotalDays;
            if (total_days > 0)
            {
                string sql = "INSERT INTO library (penalty) VALUES (@Value1 )";
                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                cmd.Parameters.AddWithValue("@Value1", total_days);
                cmd.ExecuteNonQuery();
                cnn.Close();
                var message = new { message = "penalty added" };
                return Ok(message);
            }
            else
            {
                cnn.Close();
                var message = new { message = "No penalty for this Book" };
                return Ok(message);
            }
        }
    }
}

