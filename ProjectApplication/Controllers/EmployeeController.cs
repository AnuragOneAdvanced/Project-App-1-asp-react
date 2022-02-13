using Microsoft.AspNetCore.Mvc;
using ProjectApplication.Models;
using System.Data;
using System.Data.SqlClient;

namespace ProjectApplication.Controllers
{   
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(IConfiguration config , IWebHostEnvironment environment)
        {
            this.configuration = config;
            _env = environment;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                            select EmployeeId, EmployeeName,Department,
                            convert(varchar(10),DateOfJoining,120) as DateOfJoining,PhotoFileName
                            from
                            dbo.Employee
                            ";
            DataTable dataTable = new DataTable();
            string sqlDataSoource = this.configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader sqlDataReader = null;
            using (SqlConnection myCon = new SqlConnection(sqlDataSoource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    sqlDataReader = myCommand.ExecuteReader();
                    dataTable.Load(sqlDataReader);
                }
            }

            ResponseMapper responseMapper = new ResponseMapper { StatusCode = 200, Value = dataTable, Message = "Success" };
            return new JsonResult(responseMapper);
        }


        [HttpPost]
        public JsonResult Post(Employee employee)
        {
            string query = @"
                           insert into dbo.Employee
                           (EmployeeName,Department,DateOfJoining,PhotoFileName)
                    values (@EmployeeName,@Department,@DateOfJoining,@PhotoFileName)
                            ";
            DataTable dataTable = new DataTable();
            string sqlDataSoource = this.configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader sqlDataReader = null;
            using (SqlConnection myCon = new SqlConnection(sqlDataSoource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName);
                    myCommand.Parameters.AddWithValue("@Department", employee.Department);
                    myCommand.Parameters.AddWithValue("@DateOfJoining", employee.DateOfJoining);
                    myCommand.Parameters.AddWithValue("@PhotoFileName", employee.PhotoFileName);
                    sqlDataReader = myCommand.ExecuteReader();
                    dataTable.Load(sqlDataReader);
                    sqlDataReader.Close();
                    myCon.Close();
                }
            }

            ResponseMapper responseMapper = new ResponseMapper { StatusCode = 200, Value = dataTable, Message = "Success" };
            return new JsonResult(responseMapper);
        }

        [HttpPut]
        public JsonResult Put(Employee employee)
        {
            string query = @"
                           update dbo.Employee
                           set EmployeeName= @EmployeeName,
                            Department=@Department,
                            DateOfJoining=@DateOfJoining,
                            PhotoFileName=@PhotoFileName
                            where EmployeeId=@EmployeeId
                            ";
            DataTable dataTable = new DataTable();
            string sqlDataSoource = this.configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader sqlDataReader = null;
            using (SqlConnection myCon = new SqlConnection(sqlDataSoource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);
                    myCommand.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName);
                    myCommand.Parameters.AddWithValue("@Department", employee.Department);
                    myCommand.Parameters.AddWithValue("@DateOfJoining", employee.DateOfJoining);
                    myCommand.Parameters.AddWithValue("@PhotoFileName", employee.PhotoFileName);
                    sqlDataReader = myCommand.ExecuteReader();
                    dataTable.Load(sqlDataReader);
                    sqlDataReader.Close();
                    myCon.Close();
                }
            }

            ResponseMapper responseMapper = new ResponseMapper { StatusCode = 200, Value = dataTable, Message = "Success" };
            return new JsonResult(responseMapper);
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"delete from dbo.Employee
                                where EmployeeId = @EmployeeId ";
            DataTable dataTable = new DataTable();
            string sqlDataSoource = this.configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader sqlDataReader = null;
            using (SqlConnection myCon = new SqlConnection(sqlDataSoource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@EmployeeId", id);
                    sqlDataReader = myCommand.ExecuteReader();
                    dataTable.Load(sqlDataReader);
                    sqlDataReader.Close();
                    myCon.Close();
                }
            }

            ResponseMapper responseMapper = new ResponseMapper { StatusCode = 200, Value = dataTable, Message = "Success" };
            return new JsonResult(responseMapper);
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + filename;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                ResponseMapper responseMapper = new ResponseMapper { StatusCode = 200, Value = filename, Message = "Success" };
                return new JsonResult(responseMapper);
            }
            catch (Exception)
            {
                ResponseMapper responseMapper = new ResponseMapper { StatusCode = 500, Value = "anonymous.png", Message = "Failed" };
                return new JsonResult(responseMapper);
            }
        }
    }
}
