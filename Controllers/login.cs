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
            int doctor = 5;
            MySqlConnection cnn;
            String trial = @"server=aast-db.cf4afzenuusl.us-east-1.rds.amazonaws.com;database=attendance;userid=ahmed_admin;password=777888999;";
            cnn = new MySqlConnection(trial);
            string query = $"SELECT is_doctor FROM users WHERE user_id='{person.user_id}' AND password='{person.password}'";
            MySqlCommand command = new MySqlCommand(query, cnn);

            try
            {
                cnn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        doctor =reader.GetInt32(0);
                    }
                }
                reader.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine("Error" + e.Message);
            }
            Console.WriteLine(doctor); //to check the output of invalid login
            cnn.Close();

            return Ok(new { AccountType = doctor });
        }
    }
}
