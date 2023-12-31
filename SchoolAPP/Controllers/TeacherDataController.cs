﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SchoolAPP.Models;
using MySql.Data.MySqlClient;

namespace SchoolAPP.Controllers
{
    public class TeacherDataController : ApiController
    {


        // The database context class which allows us to access our MySQL Database.
        private SchoolDbContext School = new SchoolDbContext();

        //This Controller Will access the teachers table of our school database.
        /// <summary>
        /// Returns a list of Teachers in the system
        /// </summary>
        /// <example>GET api/TeacherData/ListTeachers</example>
        /// <returns>
        /// A list of teachers (first names and last names)
        /// </returns>
        [HttpGet]
        [Route("api/TeacherData/ListTeachers/{SearchKey?}")]
        public IEnumerable<Teacher> ListTeachers(string SearchKey = null)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from Teachers where lower(teacherfname) like lower(@key) or lower(teacherlname) like lower(@key) or (concat(teacherfname, ' ', teacherlname)) like lower(@key) or hiredate like @key or salary like @key";


            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            cmd.Prepare();

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create an empty list of Author Names
            List<Teacher> Teachers = new List<Teacher> { };

            //Loop Through Each Row the Result Set
            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                int Teacherid = (int)ResultSet["teacherid"];
                string Teacherfname = (string)ResultSet["teacherfname"];
                string Teacherlname = (string)ResultSet["teacherlname"];
                decimal Salary = (decimal)ResultSet["salary"];

                Teacher NewTeacher = new Teacher();
                NewTeacher.Teacherid = Teacherid;
                NewTeacher.Teacherfname = Teacherfname;
                NewTeacher.Teacherlname = Teacherlname;
                NewTeacher.Salary = Salary;

                //Add the Teacher Name to the List
                Teachers.Add(NewTeacher);
            }

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

            //Return the final list of author names
            return Teachers;
        }

        [HttpGet]
        public Teacher FindTeacher(int id)
        {
            Teacher NewTeacher = new Teacher();

            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from Teachers where teacherid = " + id;

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();


            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                int Teacherid = (int)ResultSet["teacherid"];
                string Teacherfname = (string)ResultSet["teacherfname"];
                string Teacherlname = (string)ResultSet["teacherlname"];
                DateTime Hiredate = (DateTime)ResultSet["hiredate"];
                decimal Salary = (decimal)ResultSet["salary"];

                NewTeacher.Teacherid = Teacherid;
                NewTeacher.Teacherfname = Teacherfname;
                NewTeacher.Teacherlname = Teacherlname;
                NewTeacher.Hiredate = Hiredate;
                NewTeacher.Salary = Salary;
            }

            return NewTeacher;
        }
        /// <summary>
        /// Deletes an Teacher from the connected MySQL Database if the ID of that teacher exists. Does NOT maintain relational integrity. Non-Deterministic.
        /// </summary>
        /// <param name="id">The id of the teacher</param>
        /// <example>POST : /api/TeacherData/DeleteTeacher/3</example>

        [HttpPost]
        public void DeleteTeacher(int id)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Delete from teachers where teacherid = @id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();


        }



        /// <summary>
        /// Adds an Teacher to the MySQL Database.
        /// </summary>
        /// <param name="NewTeacher">An object with fields that map to the columns of the teacher's table. Non-Deterministic.</param>
        /// <example>
        /// POST api/TeacherData/AddTeacher 
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"TeacherFname":"Diego",
        ///	"TeacherLname":"Rivera",
        ///	"Salary":"5.50"
        /// }
        /// </example>
        [HttpPost]
        public void AddTeacher(Teacher NewTeacher)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "insert into teachers (teacherfname, teacherlname, hiredate, salary) values (@TeacherFname,@TeacherLname,CURRENT_DATE(),@Salary)";
            cmd.Parameters.AddWithValue("@TeacherFname", NewTeacher.Teacherfname);
            cmd.Parameters.AddWithValue("@TeacherLname", NewTeacher.Teacherlname);
            cmd.Parameters.AddWithValue("@Salary", NewTeacher.Salary);


            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();
        }


        /// <summary>
        /// reveives a Teacher id and teacher Payload,
        /// updates the teacher with that id in the database with the payload information
        /// </summary>
        /// <param name="Teacherid">the primary key to update</param>
        /// <param name="UpdatedTeacher">the teacher object</param>
        /// <example>
        /// POST api/TeacherData/UpdateTeacher/{Teacherid}
        /// POST DATA:
        /// {
        ///     "TeacherFname" : "Diego",
        ///     "TeacherLname" : "Rivera",
        ///     "Salary" : 45.50
        /// }
        /// </example>
        /// <returns></returns>
        [HttpPost]
        [Route("api/TeacherData/UpdateTeacher/{Teacherid}")]
        public void UpdateTeacher(int Teacherid, [FromBody]Teacher TeacherInfo)
        {
            //SQL QUERY
            string query = "update teachers set teacherfname=@TeacherFname, teacherlname=@TeacherLname, salary=@Salary where teacherid=@Teacherid";
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand Cmd = Conn.CreateCommand();

            Cmd.CommandText = query;
            Cmd.Parameters.AddWithValue("@TeacherFname", TeacherInfo.Teacherfname);
            Cmd.Parameters.AddWithValue("@TeacherLname", TeacherInfo.Teacherlname);
            Cmd.Parameters.AddWithValue("@Salary", TeacherInfo.Salary);
            Cmd.Parameters.AddWithValue("@Teacherid", Teacherid);


            Cmd.ExecuteNonQuery();

            Conn.Close();
        }



    }
}
