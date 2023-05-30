using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Graduation.Controllers.Login;
using MySql.Data.MySqlClient;

namespace Graduation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpPost]
        public IActionResult RegisterAttendance([FromBody] Person person)
        {
            String pass = "";
            MySqlConnection cnn;
            String trial = @"server=smart-campus.cewocktbczjl.us-east-1.rds.amazonaws.com;database=AAST;userid=admin;password=smart-campusadmin;";
            cnn = new MySqlConnection(trial);
            string query = $"SELECT pass FROM person WHERE id='{person.user_id}' AND is_doctor='{person.is_doctor}'";
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
