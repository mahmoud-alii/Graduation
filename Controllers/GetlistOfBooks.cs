using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Graduation.Controllers.attendance;
using MySql.Data.MySqlClient;
using System.Security.Cryptography.X509Certificates;
using System.Drawing.Imaging;
using System.Collections;

namespace Graduation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class GetlistOfBooks : ControllerBase
    {

        public class Borrowed_books
        {
            public int student_id { get; set; }
            public DateOnly borrowed_date { get; set; }
            public DateOnly due_date { get; set; }
            public int book_id { get; set; }
            public DateOnly returned_time { get; set; }
            public decimal penalty { get; set; } 
            public int borrowing_id { get; set; } 

        }
        
        [HttpPost]
        public IActionResult getlistOfBooks([FromBody] Borrowed_books borrowed_books)
        {
            var borrowedlist = new ArrayList();
            MySqlConnection cnn;
            String trial = @"server=aast-db.cf4afzenuusl.us-east-1.rds.amazonaws.com;database=library;userid=ahmed_admin;password=777888999;";
            cnn = new MySqlConnection(trial);
            string query = $"SELECT * FROM library.borrowed_books WHERE student_id='{borrowed_books.student_id}'";
            MySqlCommand command = new MySqlCommand(query, cnn);
            try
            {
                cnn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        borrowedlist.Add(reader.GetString(0));
                        borrowedlist.Add(reader.GetString(1));
                        borrowedlist.Add(reader.GetString(2));
                        borrowedlist.Add(reader.GetString(3));
                        borrowedlist.Add(reader.GetString(4));
                        borrowedlist.Add(reader.GetString(5));
                        borrowedlist.Add(reader.GetString(6));
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
                borrowedlist = borrowedlist.ToArray()
            });
        }
    }
}
