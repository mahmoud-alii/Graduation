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
using Microsoft.AspNetCore.Authorization;

namespace Graduation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ReturningAPI : ControllerBase
    {

        [HttpPost]
        [Authorize]
        public IActionResult returningAPI([FromBody] Borrowed_books borrowed_books)
        {
            DateTime today_date = DateTime.Now.Date;
            string formattedDate = today_date.ToString("yyyy-MM-dd");
            DateTime x = new DateTime();
            TimeSpan difference = new TimeSpan() ;
            MySqlConnection cnn;
            //String trial = @"server=127.0.0.1;database=attendance;userid=root;password=;";
            String trial = @"server=aast-db.cf4afzenuusl.us-east-1.rds.amazonaws.com;database=library;userid=ahmed_admin;password=777888999;";
            cnn = new MySqlConnection(trial);
            string query = $"SELECT due_date  FROM borrowed_books WHERE student_id='{borrowed_books.student_id}'AND book_id='{borrowed_books.book_id}' ";
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

                        difference = today_date - x ;
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
                string sql = $"UPDATE borrowed_books SET penalty = '{total_days}' , returned_date = '{formattedDate}' WHERE student_id='{borrowed_books.student_id}'AND borrowing_id='{borrowed_books.borrowing_id}'";
                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                cmd.ExecuteNonQuery();
                string sql2 = $"UPDATE books SET copies_available = copies_available+1  WHERE book_id='{borrowed_books.book_id}'";
                MySqlCommand cmd2 = new MySqlCommand(sql2, cnn);
                cmd2.ExecuteNonQuery();
                cnn.Close();
                var message = new { message = "The borrowed book has returned and there is penalty added" };
                //string result = $"The difference between {x.ToShortDateString()} and {today_date.ToShortDateString()} is {difference.TotalDays} days.";
                return Ok(message);
            }
            else
            {
                string sql = $"UPDATE borrowed_books SET returned_date = '{formattedDate}' WHERE student_id='{borrowed_books.student_id}'AND borrowing_id='{borrowed_books.borrowing_id}'";
                MySqlCommand cmd = new MySqlCommand(sql, cnn);
                cmd.ExecuteNonQuery();
                string sql2 = $"UPDATE books SET copies_available = copies_available+1  WHERE book_id='{borrowed_books.book_id}'";
                MySqlCommand cmd2 = new MySqlCommand(sql2, cnn);
                cmd2.ExecuteNonQuery();
                cnn.Close();
                var message = new { message = "The borrowed book has returned and there is no penalty" };
                //string result = $"The difference between {x.ToShortDateString()} and {today_date.ToShortDateString()} is {difference.TotalDays} days.";
                return Ok(message);
            }
            //string result = $"The difference between {x.ToShortDateString} and {today_date.ToShortDateString} is {difference.TotalDays} days.";
            //return Ok(result);
        }
    }
}

