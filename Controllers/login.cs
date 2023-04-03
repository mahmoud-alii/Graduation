using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace GradProject.Controllers
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
            private bool IsDoctor { get; set; }
        }

        public class Student : Person
        {

        }

        [HttpPost]
        public IActionResult Authenticate([FromBody] Person person)
        {
            int type = 0;

            MySqlConnection cnn;
            String trial = @"server=127.0.0.1;database=aast;userid=root;password=;";
            cnn = new MySqlConnection(trial);
            string query = $"SELECT type FROM aast_person WHERE id='{person.Id}' AND pass='{person.Pass}'";
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

        [HttpGet]
        public IActionResult RegisterAttendance([FromBody] Person person)
        {
            String pass = "";

            MySqlConnection cnn;
            String trial = @"server=127.0.0.1;database=aast;userid=root;password=;";
            cnn = new MySqlConnection(trial);
            string query = $"SELECT pass FROM aast_person WHERE id='{person.Id}' AND type='1'";
            MySqlCommand command = new MySqlCommand(query, cnn);

            try
            {
                cnn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        pass = reader.GetString(0);
                    }
                }
                reader.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine("Error" + e.Message);
            }
            
            cnn.Close();

            return Ok(new { AttendancePass = pass });
        }
    }
}
