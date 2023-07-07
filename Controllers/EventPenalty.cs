using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Graduation.Controllers.GetlistOfBooks;
using MySql.Data.MySqlClient;
using System.Security.Cryptography.X509Certificates;
using System.Drawing.Imaging;
using System.Collections;
using OfficeOpenXml.Table.PivotTable;
using System.Data;
using MySqlX.XDevAPI.Common;
using System.Web.Http.Results;
using System;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Authorization;

namespace Graduation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventPenalty : ControllerBase
    {

        [HttpPost]
        //[Authorize]
        public IActionResult eventPenalty()
        {
            DateTime today_date = DateTime.Now.Date;
            DateTime x = new DateTime();
            string formattedDate = x.ToString("yyyy-MM-dd");
            DateTime a = new DateTime();
            TimeSpan difference = new TimeSpan();
            TimeSpan difference2 = new TimeSpan();
            decimal y = 0 ;
            int b = 0 ;
            int z = 0 ;
            int total_days = 0;
            MySqlConnection cnn;
            //String trial = @"server=127.0.0.1;database=attendance;userid=root;password=;";
            String trial = @"server=aast-db.cf4afzenuusl.us-east-1.rds.amazonaws.com;database=library;userid=ahmed_admin;password=777888999;";
            cnn = new MySqlConnection(trial);
            string query = $"SELECT returned_date , student_id  , penalty , book_id  FROM borrowed_books";
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
                        string formatedDate = x.ToString("yyyy-MM-dd");
                        b = reader.GetInt32(1);
                        y = reader.GetDecimal(2);
                        z = reader.GetInt32(3);
                        difference = today_date - x;

                        total_days = (int)difference.TotalDays;
                        
                            if (formatedDate != "1970-01-01")
                            {
                                cnn.Close();
                                cnn.Open();
                                y = y - 1;
                            if (y != 0)
                            {
                                string sql1 = $"UPDATE borrowed_books SET penalty = '{y}' WHERE student_id='{b}' AND book_id = '{z}' ";
                                MySqlCommand cmd = new MySqlCommand(sql1, cnn);
                                cmd.ExecuteNonQuery();
                            }
                            }
                            else 
                            {
                                cnn.Close();
                                string sql2 = $"SELECT due_date ,student_id , penalty , book_id  FROM borrowed_books";
                                MySqlCommand cmd2 = new MySqlCommand(sql2, cnn);
                                try
                                {
                                    cnn.Open();
                                    MySqlDataReader reader2 = cmd2.ExecuteReader();
                                    if (reader2.HasRows)
                                    {
                                        while (reader2.Read())
                                        {
                                            a = reader2.GetDateTime(0);
                                            int h = reader2.GetInt32(1);
                                            int k = reader2.GetInt32(2);
                                            int bid = reader2.GetInt32(3);
                                            difference2 = today_date - x;

                                            int total_days2 = (int)difference2.TotalDays;

                                            if (total_days2 > 0)
                                            {
                                                cnn.Close();
                                                cnn.Open();
                                                string sql3 = $"UPDATE borrowed_books SET penalty = '{total_days2}' WHERE student_id = '{h}' AND book_id = '{bid}' ";
                                                MySqlCommand cmd3 = new MySqlCommand(sql3, cnn);
                                                cmd3.ExecuteNonQuery();

                                            }
                                            else
                                            {
                                                cnn.Close();
                                                return Ok();
                                            }
                                        }
                                    }
                                    reader2.Close();
                                }

                                catch (Exception e)
                                {
                                    Console.WriteLine("Error" + e.Message);
                                }

                            }
                        
                    }
                }
                reader.Close();
            }

            catch (Exception e)
            {
                Console.WriteLine("Error" + e.Message);
            }
            cnn.Close();
            int total_days22 = (int)difference2.TotalDays;
            //y = y - 1;
            //string sql1 = $"UPDATE borrowed_books SET penalty = '{y}' WHERE student_id='{b}' ";
            //MySqlCommand cmd = new MySqlCommand(sql1, cnn);
            //cmd.ExecuteNonQuery();
            //cnn.Close();
            var message = new { message = "all penalty are updated" };
            return Ok(message);
         }
     }
}

