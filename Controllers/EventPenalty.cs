using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;

namespace Graduation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventPenalty : ControllerBase
    {
        [HttpPost]
        public IActionResult eventPenalty()
        {
            DateTime Zero = new DateTime(1970, 1, 1);
            MySqlConnection cnn;
            String trial = @"server=aast-db.cf4afzenuusl.us-east-1.rds.amazonaws.com;database=library;userid=ahmed_admin;password=777888999;";
            cnn = new MySqlConnection(trial);

            try
            {
                cnn.Open();


                string updateSql = $"UPDATE borrowed_books SET penalty = penalty - 1 WHERE returned_date != '{Zero}' AND penalty > 0";
                MySqlCommand updateCmd = new MySqlCommand(updateSql, cnn);
                updateCmd.ExecuteNonQuery();


                cnn.Close();

                var message = new { message = "All penalties are updated" };
                return Ok(message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                var errorResponse = new { message = "An error occurred while updating the penalties" };
                return StatusCode(500, errorResponse);
            }
        }
    }
}