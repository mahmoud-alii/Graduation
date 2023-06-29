using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace Graduation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Login : ControllerBase
    {
        private readonly int _jwtExpirationMinutes = 1440;

        private readonly IConfiguration _configuration;

        public Login(IConfiguration configuration)
        {
            _configuration = configuration;
            // other code
        }

        public class User
        {
            public int user_id { get; set; }
            public string password { get; set; }
            public int user_type { get; set; }
        }

        public class Instructor : User
        {
            private string Email { get; set; }
        }

        public class Student : User
        {

        }


        [HttpPost]
        [AllowAnonymous]
        public IActionResult Authenticate([FromBody] User user)
        {
            string account_type = "Not Authorized";
            MySqlConnection cnn;
            String trial = @"server=aast-db.cf4afzenuusl.us-east-1.rds.amazonaws.com;database=usersdb;userid=ahmed_admin;password=777888999;";
            cnn = new MySqlConnection(trial);
            string query = $"SELECT user_type FROM users WHERE user_id='{user.user_id}' AND password='{user.password}'";
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
                    if(account_type != "Not Authorized")
                    {
                        // Generate JWT token
                        string jwtToken = GenerateJwtToken(user.user_id.ToString(), account_type);

                        // Return JWT token as response
                        return Ok(new {
                        token = jwtToken,
                        User_id = user.user_id,
                        User_type = account_type
                        });
                    }
                }
                reader.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine("Error" + e.Message);
            }
            //Console.WriteLine(account_type); //to check the output of invalid login
            cnn.Close();
            return Unauthorized();
        }

        private string GenerateJwtToken(string userId, string accountType)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecretKey = _configuration["Jwt:Key"]; // Retrieve the secret key from appsettings.json
            var jwtIssuer = _configuration["Jwt:Issuer"]; // Retrieve the issuer from appsettings.json
            var jwtAudience = _configuration["Jwt:Audience"]; // Retrieve the audience from appsettings.json

            var key = Encoding.ASCII.GetBytes(jwtSecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Name, userId),
            new Claim("AccountType", accountType)
        }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtExpirationMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtIssuer, // Set the issuer
                Audience = jwtAudience // Set the audience
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


    }
}
