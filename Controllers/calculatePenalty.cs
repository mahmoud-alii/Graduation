using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Graduation.Controllers.GetlistOfBooks;
using MySql.Data.MySqlClient;
using System.Security.Cryptography.X509Certificates;
using System.Drawing.Imaging;
using System.Collections;
using OfficeOpenXml.Table.PivotTable;
using System.Data;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using MySqlX.XDevAPI.Common;
using System.Web.Http.Results;

namespace Graduation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class calculatePenalty : ControllerBase
    {

        [HttpPost]
        public IActionResult getlistOfBooks([FromBody] Borrowed_books borrowed_books)
        {
            DateTime x = new DateTime() ;
            DateTime y = new DateTime() ;
            TimeSpan difference = new TimeSpan() ;
            MySqlConnection cnn;
            String trial = @"server=aast-db.cf4afzenuusl.us-east-1.rds.amazonaws.com;database=library;userid=ahmed_admin;password=777888999;";
            cnn = new MySqlConnection(trial);
            string query = $"SELECT due_date  FROM library.borrowed_books WHERE student_id='{borrowed_books.student_id}'and book_id'{borrowed_books.book_id}'";
            MySqlCommand command = new MySqlCommand(query, cnn);
            string query2 = $"SELECT returned_date  FROM library.borrowed_books WHERE student_id='{borrowed_books.student_id}'and book_id'{borrowed_books.book_id}'";
            MySqlCommand command2 = new MySqlCommand(query2, cnn);
            try
            {
                cnn.Open();
                MySqlDataReader reader = command.ExecuteReader();
                MySqlDataReader reader2 = command2.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                         x = reader.GetDateTime(0);
                         y = reader2.GetDateTime(0);

                        difference = y-x ;
                    }
                }
                reader.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine("Error" + e.Message);
            }

            int total_days = (int)difference.TotalDays;

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
                string result = $"The difference between {x.ToShortDateString()} and {y.ToShortDateString()} is {difference.TotalDays} days.";
                return Ok(result);
            }
        }
    }
}

