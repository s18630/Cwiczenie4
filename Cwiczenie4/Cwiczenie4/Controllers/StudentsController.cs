using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Cwiczenie4.Models;
using Cwiczenie4.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cwiczenie4.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private const string ConString = "Data Source=db-mssql;Initial Catalog=2019SBD;Integrated Security=True";
        private IStudentsDal _dbService;

        public StudentsController(IStudentsDal dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public IActionResult GetStudent([FromServices] IStudentsDal dbService)
        {
            var list = new List<Student>();
            using (SqlConnection con = new SqlConnection(ConString))
            using(SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "select * from Student";

                con.Open();
                SqlDataReader dr = com.ExecuteReader();
                while (dr.Read())
                {
                    var st = new Student();
                    st.IndexNumber = dr["IndexNumber"].ToString();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.BirthDate = dr["BirthDate"].ToString();
                    st.IdEnrollment = dr["IdEnrollment"].ToString();

                    list.Add(st);


                }
                    
            }
            return Ok(list);
        }


        [HttpGet("{indexNumber}")]
        public IActionResult GetStudent(string indexNumber)
        {
            
            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText =" SELECT Studies.Name, Enrollment.Semester ,Enrollment.StartDate "+
                     "from Student "+
                     "inner join Enrollment on Enrollment.IdEnrollment = Student.IdEnrollment "+
                     "inner join Studies on Enrollment.IdStudy = Studies.IdStudy "+
                     "WHERE IndexNumber = @index";

                com.Parameters.AddWithValue("index", indexNumber); 
                con.Open();

                var dr = com.ExecuteReader();
                if (dr.Read())
                {
                    string Name = dr["Name"].ToString();
                    string Semester = dr["Semester"].ToString();
                    string StartDate = dr["StartDate"].ToString();

                    return Ok("Studies: " +Name +"\nSemester: " +Semester + "\nStart Date: " + StartDate);
                }

            } return NotFound();
           
            
        }













    }
}
