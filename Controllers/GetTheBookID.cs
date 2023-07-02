using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Security.Cryptography.X509Certificates;

namespace Graduation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetTheBookID : ControllerBase
    {
        public class Books
        {
            public int book_id { get; set; }
            public string title { get; set; }
            public string author { get; set; }
            public int publication_year { get; set; }
            public string isbn { get; set; }
            public int copies_available { get; set; }

        }

        [HttpPost]
        //[Authorize]
        public IActionResult getTheBookID([FromBody] Books books)
        {
            int book_id = 0;
            MySqlConnection cnn;
            String trial = @"server=aast-db.cf4afzenuusl.us-east-1.rds.amazonaws.com;database=library;userid=ahmed_admin;password=777888999;";
            cnn = new MySqlConnection(trial);
            string query = $"SELECT book_id FROM books WHERE isbn='{books.isbn}'";
            MySqlCommand command = new MySqlCommand(query, cnn);
            try
            {
                cnn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        book_id = reader.GetInt32(0);
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
                BookID = book_id
            });
        }
    }
}

