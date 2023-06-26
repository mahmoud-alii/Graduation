using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Security.Cryptography.X509Certificates;
using System.Drawing.Imaging;
using System.Collections;
using System.Security.Policy;

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
            public DateTime returned_date { get; set; }
            public decimal penalty { get; set; }  

        }
        
        [HttpPost]
        public IActionResult getlistOfBooks([FromBody] Borrowed_books borrowed_books)
        {
            ArrayList borrowedlist = new ArrayList();
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

                        ArrayList list = new ArrayList
                        {
                            reader.GetInt32(0),
                            reader.GetInt32(1),
                            reader.GetInt32(2),
                            reader.GetDateTime(3),
                            reader.GetDateTime(4),
                            reader.GetDateTime(5),
                            //reader.GetDecimal(6)
                        };
                        borrowedlist.Add(list);

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
                BorrowedList = borrowedlist.ToArray()
            });
        }
    }
}
