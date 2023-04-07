using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using static Graduation.Controllers.Login;

namespace Graduation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class attendance : ControllerBase
    {
        [HttpPost]
        public IActionResult RegisterAttendance([FromBody] Student student)
        {
            String pass = "";
             //Omar
            MySqlConnection cnn;
            String trial = @"server=127.0.0.1;database=aast;userid=root;password=;";
            cnn = new MySqlConnection(trial);
            string query = $"SELECT pass FROM aast_person WHERE id='{student.Id}' AND type='1'";
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
