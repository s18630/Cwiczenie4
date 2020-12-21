using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Cwiczenie4.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cwiczenie4.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private const string ConString = "Data Source=db-mssql;Initial Catalog=2019SBD;Integrated Security=True";

        [HttpGet]
        public IActionResult GetStudent()
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


        [HttpGet("{IndexNumber}")]
        public IActionResult GetStudent(string indexNumber)
        {
            var list = new List<Student>();
            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "select * from Student where indexnumber=@index";

                SqlParameter par = new SqlParameter();
                par.Value = indexNumber;
                par.ParameterName = "index";
                com.Parameters.Add(par);

                con.Open();
                var dr = com.ExecuteReader();
                if(dr.Read())
                {
                    var st = new Student();
                    st.IndexNumber = dr["IndexNumber"].ToString();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.BirthDate = dr["BirthDate"].ToString();
                    st.IdEnrollment = dr["IdEnrollment"].ToString();

                    return Ok(st);


                }

            }
            return NotFound();
        }













    }
}
