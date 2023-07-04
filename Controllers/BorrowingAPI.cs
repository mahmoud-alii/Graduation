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
using static Graduation.Controllers.GetTheBookID;
using Microsoft.VisualBasic;
using System.Runtime.ConstrainedExecution;
using Microsoft.AspNetCore.Authorization;

namespace Graduation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class BorrowingAPI : ControllerBase
    {

        [HttpPost]
        //[Authorize]
        public IActionResult borrowingAPI([FromBody] Borrowed_books borrowed_books )
        {
            DateTime today_date = DateTime.Now.Date;
            string formattedDate = today_date.ToString("yyyy-MM-dd");
            int num_cp = 0 ;
            int total_penalty = 0;
            int num_books = 0;
            MySqlConnection cnn;
            //String trial = @"server=127.0.0.1;database=attendance;userid=root;password=;";
            String trial = @"server=aast-db.cf4afzenuusl.us-east-1.rds.amazonaws.com;database=library;userid=ahmed_admin;password=777888999;";
            cnn = new MySqlConnection(trial);
            string query = $"SELECT copies_available FROM books WHERE book_id='{borrowed_books.book_id}'";
            MySqlCommand command = new MySqlCommand(query, cnn);

            string query2 = $"SELECT penalty FROM borrowed_books WHERE student_id='{borrowed_books.student_id}'";
            MySqlCommand command2 = new MySqlCommand(query2, cnn);

            string query3 = $"SELECT returned_date FROM borrowed_books WHERE student_id='{borrowed_books.student_id}'";
            MySqlCommand command3 = new MySqlCommand(query3, cnn);

            try
            {
                cnn.Open();
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        num_cp = int.Parse(reader.GetString(0));
                    }
                }
                reader.Close();
            }
            
            catch (Exception e)
            {
                Console.WriteLine("Error" + e.Message);
            }
            cnn.Close();
            try
            {
                cnn.Open();
                MySqlDataReader reader2 = command2.ExecuteReader();


                if (reader2.HasRows)
                {
                    while (reader2.Read())
                    {
                        int x = reader2.GetInt32(0);
                        total_penalty += x;
                    }
                }
                reader2.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error" + e.Message);
            }
            cnn.Close();
            try
            {
                cnn.Open();
                MySqlDataReader reader3 = command3.ExecuteReader();


                if (reader3.HasRows)
                {
                    while (reader3.Read())
                    {
                        DateTime returnedDate = reader3.GetDateTime(0);
                        string x = returnedDate.ToString("yyyy-MM-dd");
                        DateTime y = new DateTime(1970,1,1);
                        if  (returnedDate == y)
                        { 
                        num_books += 1;
                        }                                    
                    }
                }
                reader3.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error" + e.Message);
            }
            cnn.Close();

            if (total_penalty == 0 && num_books < 3)
            {
                if (num_cp > 1)
                {
                    cnn.Open();
                    DateTime dueDate = today_date.AddDays(7);
                    string formattedDate2 = dueDate.ToString("yyyy-MM-dd");
                    DateTime x = new DateTime (1970, 1, 1, 0, 0, 0);
                    string Zero = x.ToString("yyyy-MM-dd");
                    string sql = $"INSERT INTO borrowed_books (book_id, student_id, borrowed_date, due_date , returned_date , penalty) VALUES ('{borrowed_books.book_id}', '{borrowed_books.student_id}', '{formattedDate}','{formattedDate2}' , '{Zero}' , 0 )";
                    MySqlCommand cmd = new MySqlCommand(sql, cnn);
                    cmd.ExecuteNonQuery();
                    string sql2 = $"UPDATE books SET copies_available = copies_available-1  WHERE book_id='{borrowed_books.book_id}'";
                    MySqlCommand cmd2 = new MySqlCommand(sql2, cnn);
                    cmd2.ExecuteNonQuery();
                    cnn.Close();
                    var message = new { message = "The Borrowed booked added " };
                    return Ok(message); ;
                }
                else if (num_cp == 1)
                {
                    cnn.Open();
                    DateTime dueDate = today_date.AddDays(2);
                    string formattedDate2 = dueDate.ToString("yyyy-MM-dd");
                    DateTime x = new DateTime(1970, 1, 1, 0, 0, 0);
                    string Zero = x.ToString("yyyy-MM-dd");
                    string sql = $"INSERT INTO borrowed_books (book_id, student_id, borrowed_date, due_date , returned_date , penalty ) VALUES ('{borrowed_books.book_id}', '{borrowed_books.student_id}', '{formattedDate}','{formattedDate2}' , '{Zero}' , 0 )";
                    MySqlCommand cmd = new MySqlCommand(sql, cnn);
                    cmd.ExecuteNonQuery();
                    string sql2 = $"UPDATE books SET copies_available = copies_available-1  WHERE book_id='{borrowed_books.book_id}'";
                    MySqlCommand cmd2 = new MySqlCommand(sql2, cnn);
                    cmd2.ExecuteNonQuery();
                    cnn.Close();
                    var message = new { message = "The Borrowed booked added and this was the last copy" };
                    return Ok(message);
                }
                else
                {
                    var message = new { message = "There is no copies" };
                    return Ok(new { num_cp });
                }
                //string result = $"The difference between {x.ToShortDateString()} and {today_date.ToShortDateString()} is {difference.TotalDays} days.";
            }
            else if (total_penalty != 0 && num_books < 3)
            {
                string message = $" there is penalty { total_penalty } days on this custome ";
                return Ok(new { message });
            }
            else if (total_penalty == 0 && num_books >= 3)
            {
                string message =  $"this customer borrowed already {num_books} books ";
                return Ok(new { message });
            }
            else
            {
                string message = $"this customer borrowed already {num_books} books and have penalty {total_penalty} " ;
                return Ok(new { message });
            }
        }
    }
}

