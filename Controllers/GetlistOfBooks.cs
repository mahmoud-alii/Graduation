using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Graduation.Controllers.CheckTakes;
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
            public int borrowing_id { get; set; }
            public int book_id { get; set; }
            public int student_id { get; set; }
            public DateTime borrowed_date { get; set; }
            public DateTime due_date { get; set; }
            public DateTime returned_time { get; set; }
            public decimal penalty { get; set; }  

        }
        
        [HttpPost]
        public IActionResult getlistOfBooks([FromBody] Borrowed_books borrowed_books)
        {
            var borrowedlist = new ArrayList();
            MySqlConnection cnn;
            String trial = @"server=aast-db.cf4afzenuusl.us-east-1.rds.amazonaws.com;database=library;userid=ahmed_admin;password=777888999;";
            cnn = new MySqlConnection(trial);
            string query = $"SELECT * FROM borrowed_books WHERE student_id='{borrowed_books.student_id}'";
            MySqlCommand command = new MySqlCommand(query, cnn);
            try
            {
                cnn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        borrowedlist.Add(reader.GetInt32(0));
                        borrowedlist.Add(reader.GetInt32(1));
                        borrowedlist.Add(reader.GetInt32(2));
                        borrowedlist.Add(reader.GetDateTime(3));
                        borrowedlist.Add(reader.GetDateTime(4));
                        borrowedlist.Add(reader.GetDateTime(5));
                        borrowedlist.Add(reader.GetDecimal(6));
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
