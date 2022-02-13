using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using ProjectApplication.Models;

namespace ProjectApplication.Controllers
{   
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public DepartmentController(IConfiguration config)
        {
            this.configuration = config;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select DepartmentId, DepartmentName from dbo.Department";
            DataTable dataTable = new DataTable();
            string sqlDataSoource = this.configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader sqlDataReader = null;
            using (SqlConnection myCon = new SqlConnection(sqlDataSoource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query , myCon))
                {
                    sqlDataReader = myCommand.ExecuteReader();
                    dataTable.Load(sqlDataReader);
                }
            }

            return new JsonResult(dataTable);
        }

           
        [HttpPost]
        public JsonResult Post(Department department)
        {
            Console.WriteLine(department.DepartmentName);
            string query = @"insert into dbo.Department values(@DepartmentName)";
            DataTable dataTable = new DataTable();
            string sqlDataSoource = this.configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader sqlDataReader = null;
            using (SqlConnection myCon = new SqlConnection(sqlDataSoource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@DepartmentName", department.DepartmentName);
                    sqlDataReader = myCommand.ExecuteReader();
                    dataTable.Load(sqlDataReader);
                    sqlDataReader.Close();
                    myCon.Close();
                }
            }

            ResponseMapper responseMapper = new ResponseMapper { StatusCode = 200, Value = dataTable, Message = "Successfullt Added" };
            return new JsonResult (responseMapper);
        }

        [HttpPut]
        public JsonResult Put(Department department)
        {
            string query = @"update dbo.Department set DepartmentName= @DepartmentName
                                where DepartmentId = @DepartmentId ";
            DataTable dataTable = new DataTable();
            string sqlDataSoource = this.configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader sqlDataReader = null;
            using (SqlConnection myCon = new SqlConnection(sqlDataSoource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@DepartmentName", department.DepartmentName);
                    myCommand.Parameters.AddWithValue("@DepartmentId", department.DepartmentId);
                    sqlDataReader = myCommand.ExecuteReader();
                    dataTable.Load(sqlDataReader);
                    sqlDataReader.Close();
                    myCon.Close();
                }
            }

            ResponseMapper responseMapper = new ResponseMapper { StatusCode = 200, Value = dataTable, Message = "Successfullt Added" };
            return new JsonResult(responseMapper);
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"delete from dbo.Department
                                where DepartmentId = @DepartmentId ";
            DataTable dataTable = new DataTable();
            string sqlDataSoource = this.configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader sqlDataReader = null;
            using (SqlConnection myCon = new SqlConnection(sqlDataSoource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@DepartmentId", id);
                    sqlDataReader = myCommand.ExecuteReader();
                    dataTable.Load(sqlDataReader);
                    sqlDataReader.Close();
                    myCon.Close();
                }
            }

            ResponseMapper responseMapper = new ResponseMapper { StatusCode = 200, Value = dataTable, Message = "Successfullt Added" };
            return new JsonResult(responseMapper);
        }
    }
}
