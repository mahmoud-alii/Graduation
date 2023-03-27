using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace GradProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Login : ControllerBase
    {
        public class LoginModel
        {
            public int Id { get; set; }
            public string Pass { get; set; }
        }

        [HttpPost]
        public IActionResult Authenticate([FromBody] LoginModel login)
        {
            int type = 0;

            MySqlConnection cnn;
            String trial = @"server=127.0.0.1;database=aast;userid=root;password=;";
            cnn = new MySqlConnection(trial);
            string query = $"SELECT type FROM aast_person WHERE id='{login.Id}' AND pass='{login.Pass}'";
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
