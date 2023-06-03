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
            public int user_id { get; set; }
            public string password { get; set; }
            public int is_doctor { get; set; }
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
            string account_type = "Not Authorized";
            MySqlConnection cnn;
            String trial = @"server=aast-db.cf4afzenuusl.us-east-1.rds.amazonaws.com;database=usersdb;userid=ahmed_admin;password=777888999;";
            cnn = new MySqlConnection(trial);
            string query = $"SELECT user_type FROM users WHERE user_id='{person.user_id}' AND password='{person.password}'";
            MySqlCommand command = new MySqlCommand(query, cnn);

            try
            {
                cnn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        account_type = reader.GetString(0);
                    }
                }
                reader.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine("Error" + e.Message);
            }
            Console.WriteLine(account_type); //to check the output of invalid login
            cnn.Close();
            return Ok(new { AccountType = account_type });
        }
    }
}
