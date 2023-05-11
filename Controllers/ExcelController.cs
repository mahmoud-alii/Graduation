using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using OfficeOpenXml;
using System.IO;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExcelController : ControllerBase
    {
        [HttpGet]
        public FileContentResult Get()
        {
            var connectionString = @"server=smart-campus.cewocktbczjl.us-east-1.rds.amazonaws.com;database=AAST;userid=admin;password=smart-campusadmin;";
            var fileName = "attendance.xlsx";

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                using (var connection = new MySqlConnection(connectionString))
                {
                    var query = "SELECT * FROM person";
                    var command = new MySqlCommand(query, connection);

                    connection.Open();
                    var reader = command.ExecuteReader();

                    var row = 2;
                    worksheet.Cells[1, 1].Value = "ID";
                    worksheet.Cells[1, 2].Value = "Password";
                    while (reader.Read())
                    {
                        worksheet.Cells[row, 1].Value = reader.GetString(0);
                        worksheet.Cells[row, 2].Value = reader.GetString(1);
                        // add more columns as needed
                        row++;
                    }

                    reader.Close();
                    connection.Close();
                }

                var fileBytes = package.GetAsByteArray();
                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }
    }
}
