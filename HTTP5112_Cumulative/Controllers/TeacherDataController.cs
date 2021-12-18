﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HTTP5112_Cumulative.Models;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace HTTP5112_Cumulative.Controllers
{
    public class TeacherDataController : ApiController
    {
        private SchoolDBContext School = new SchoolDBContext();

        //This Controller Will access the teachers table of our school database.
        /// Returns a list of Teachers in the system
        /// </summary>
        /// <returns>
        /// A list of teachers (first name, last name).
        /// </returns>
        /// <example>GET api/TeacherData/ListTeachers</example>
        [HttpGet]
        [Route("api/TeacherData/ListTeachers/{SearchKey?}")]
        public IEnumerable<Teacher> ListTeachers(string SearchKey=null)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "SELECT * from Teachers where lower(teacherfname) like lower(@key) or lower(teacherlname) like lower(@key) or lower(concat(teacherfname,' ',teacherlname)) like lower(@key)";

            cmd.Parameters.AddWithValue("@key","%"+ SearchKey+"%");

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create an empty list of Teacher Names
            List<Teacher> Teachers = new List<Teacher>{};

            //Loop Through Each Row the Result Set               
            while (ResultSet.Read())
            {
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();
                DateTime TeacherHireDate = (DateTime)ResultSet["hiredate"];
                decimal TeacherSalary = Convert.ToDecimal(ResultSet["salary"]);
                
                Teacher NewTeacher = new Teacher();
                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.TeacherHireDate = TeacherHireDate;
                NewTeacher.TeacherSalary = TeacherSalary;

                //Add a new Teacher
                Teachers.Add(NewTeacher);
            }

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

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
            cmd.CommandText = "SELECT * from Teachers where teacherid ="+id; 

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read())
            {
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();
                DateTime TeacherHireDate = (DateTime)ResultSet["hiredate"];
                decimal TeacherSalary = Convert.ToDecimal(ResultSet["salary"]);

                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.TeacherHireDate = TeacherHireDate;
                NewTeacher.TeacherSalary = TeacherSalary;
            }

            return NewTeacher;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <example>POST : /api/TeacherData/DeleteTeacher/2</example>
        /// <param name="id"></param>
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
            cmd.CommandText = "DELETE from Teachers where teacherid =@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            //Close the connection between the web server and database
            Conn.Close();

        }
        [HttpPost]
        public void AddTeacher(Teacher NewTeacher)
        {

            // Create an instance of a connection
             MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "INSERT into Teachers (teacherfname,teacherlname,salary,hiredate) values (@TeacherFname,@TeacherLname,@TeacherSalary,CURRENT_DATE())";
            cmd.Parameters.AddWithValue("@TeacherFname", NewTeacher.TeacherFname);
            cmd.Parameters.AddWithValue("@TeacherLname", NewTeacher.TeacherLname);
            cmd.Parameters.AddWithValue("@TeacherSalary", NewTeacher.TeacherSalary);

            cmd.Prepare();

            cmd.ExecuteNonQuery();

            //Close the connection between the web server and database
            Conn.Close();
        }

        [HttpPost]
        public void UpdateTeacher(int id, [FromBody]Teacher TeacherInfo)
        {
            // Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();
            Debug.WriteLine(TeacherInfo.TeacherFname);

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "update teachers set teacherfname=@TeacherFname,teacherlname=@TeacherLname,salary=@TeacherSalary where teacherid=@TeacherID";
            cmd.Parameters.AddWithValue("@TeacherFname", TeacherInfo.TeacherFname);
            cmd.Parameters.AddWithValue("@TeacherLname", TeacherInfo.TeacherLname);
            cmd.Parameters.AddWithValue("@TeacherSalary", TeacherInfo.TeacherSalary);
            cmd.Parameters.AddWithValue("@TeacherId", id);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            //Close the connection between the web server and database
            Conn.Close();
        }

    }

}    
