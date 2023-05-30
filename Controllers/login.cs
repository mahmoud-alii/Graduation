using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Graduation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Login : ControllerBase
    {
        public class Person
        {
            public int Id { get; set; }
            public string Pass { get; set; }
            public int Type { get; set; }
        }

        public class Instructor : Person
        {
            private string Email { get; set; }
        }

        public class Student : Person
        {

        }


        [HttpPost]
        public IActionResult Authenticate([FromBody] Person person)
        {
            int type = 0;

            MySqlConnection cnn;
            String trial = @"server=aast-db.cf4afzenuusl.us-east-1.rds.amazonaws.com;database=attendace;userid=ahmed_admin;password=777888999;";
            cnn = new MySqlConnection(trial);
            string query = $"SELECT is_doctor FROM users WHERE user_id='{person.Id}' AND password='{person.Pass}'";
            MySqlCommand command = new MySqlCommand(query, cnn);

            try
            {
                cnn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        type = int.Parse(reader.GetString(0));
                    }
                }
                reader.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine("Error" + e.Message);
            }
            Console.WriteLine(type); //to check the output of invalid login
            cnn.Close();

            return Ok(new { AccountType = type });
        }
    }
}
