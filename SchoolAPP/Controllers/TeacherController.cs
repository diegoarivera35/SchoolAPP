﻿using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolAPP.Models;
using System.Security.Cryptography.X509Certificates;

namespace SchoolAPP.Controllers
{
    public class TeacherController : Controller
    {
        // GET: Teacher
        public ActionResult Index()
        {
            return View();
        }

        // GET: /Teachers/List
        public ActionResult List(string SearchKey = null) 
        {
            TeacherDataController controller = new TeacherDataController();
            IEnumerable<Teacher> Teachers = controller.ListTeachers(SearchKey);


            return View(Teachers);
        }

        //GET: /Teacher/Show/{id}
        public ActionResult Show(int id) 
        {

            TeacherDataController controller = new TeacherDataController();
            Teacher NewTeacher = controller.FindTeacher(id);



            //Teacher NewTeacher = new Teacher();
            //NewTeacher.Teacherfname = "Diego";
            //NewTeacher.Teacherlname = "Rivera";


            return View(NewTeacher); 
        }

        //GET: /Teacher/DeleteConfirm/{id}
        public ActionResult DeleteConfirm(int id)
        {

            TeacherDataController controller = new TeacherDataController();
            Teacher NewTeacher = controller.FindTeacher(id);



            return View(NewTeacher);
        }


        //POST: /Teacher/Delete/{id}
        [HttpPost]
        public ActionResult Delete(int id)
        {

            TeacherDataController controller = new TeacherDataController();
            controller.DeleteTeacher(id);

            return RedirectToAction("List");
        }

        //GET: /Teacher/New

        public ActionResult New()
        {
            return View();
        }

        public string errorMessage = "";
        //POST: /Teacher/Create
        [HttpPost]
        public ActionResult Create(string TeacherFname, string TeacherLname, decimal? Salary)
        {
            
            Teacher NewTeacher = new Teacher();
            NewTeacher.Teacherfname = TeacherFname;
            NewTeacher.Teacherlname = TeacherLname;
            NewTeacher.Salary = Salary;

            // Check for empty strings or zero salary
            if (string.IsNullOrWhiteSpace(NewTeacher.Teacherfname) ||
                string.IsNullOrWhiteSpace(NewTeacher.Teacherlname) ||
                NewTeacher.Salary == null)
            {
                errorMessage = "**All fields are required**";
                Console.WriteLine(NewTeacher.Salary);
                return Content(errorMessage);
            }

            TeacherDataController controller = new TeacherDataController();
            controller.AddTeacher(NewTeacher);

            return RedirectToAction("List");
        }


    }
}