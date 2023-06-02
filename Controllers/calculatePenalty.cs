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
using System;


namespace Graduation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class calculatePenalty : ControllerBase
    {

        [HttpPost]
        public IActionResult getlistOfBooks([FromBody] Borrowed_books borrowed_books)
        {
            DateOnly date;
            DateOnly date2;
            DateTime x = new DateTime();
            DateTime y = new DateTime();
            TimeSpan difference = new TimeSpan() ;
            MySqlConnection cnn;
            //String trial = @"server=127.0.0.1;database=attendance;userid=root;password=;";
            String trial = @"server=aast-db.cf4afzenuusl.us-east-1.rds.amazonaws.com;database=library;userid=ahmed_admin;password=777888999;";
            cnn = new MySqlConnection(trial);
            string query = $"SELECT due_date , returned_date  FROM borrowed_books WHERE student_id='{borrowed_books.student_id}'AND book_id='{borrowed_books.book_id}' ";
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
                        difference = y - x ;
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
                string sql = $"UPDATE borrowed_books SET penalty = '{total_days}' WHERE student_id='{borrowed_books.student_id}'AND book_id='{borrowed_books.book_id}'"; 
                MySqlCommand cmd = new MySqlCommand(sql, cnn);
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
            //string result = $"The difference between {x.ToShortDateString} and {y.ToShortDateString} is {difference.TotalDays} days.";
            //return Ok(result);
        }
    }
}

