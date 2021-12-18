using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HTTP5112_Cumulative.Models;
using System.Diagnostics;

namespace HTTP5112_Cumulative.Controllers
{
    public class TeacherController : Controller
    {
        // GET: Teacher
        public ActionResult Index()
        {
            return View();
        }

        //GET: /Teacher/List
        public ActionResult List(string SearchKey = null)
        {
            TeacherDataController controller = new TeacherDataController();
            IEnumerable<Teacher> Teachers = controller.ListTeachers(SearchKey);

            return View(Teachers);
        }

        //GET /Teacher/Show/{id}
        public ActionResult Show(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher SelectedTeacher = controller.FindTeacher(id);


            return View(SelectedTeacher);
        }

        //GET /Teacher/DeleteConfirm/{id}

        public ActionResult DeleteConfirm(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher NewTeacher = controller.FindTeacher(id);


            return View(NewTeacher);
        }

        //POST /Teacher/Delete/{id}
        [HttpPost]
        public ActionResult Delete(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            controller.DeleteTeacher(id);
            return RedirectToAction("List");
        }

        //GET /Teacher/New

        public ActionResult New()
        {
            return View();
        }

        //POST /Teacher/Create
        [HttpPost]
        public ActionResult Create(string TeacherFname, string TeacherLname, string TeacherSalary)
        {
            //identify method is running

            //indetify the inputs provided from the form
            Debug.WriteLine("I have accessed the Create Method!");
            Debug.WriteLine(TeacherFname);
            Debug.WriteLine(TeacherLname);
            Debug.WriteLine(TeacherSalary);

            Teacher NewTeacher = new Teacher();
            NewTeacher.TeacherFname = TeacherFname;
            NewTeacher.TeacherLname = TeacherLname;
            NewTeacher.TeacherSalary = Convert.ToDecimal(TeacherSalary);

            TeacherDataController controller = new TeacherDataController();
            controller.AddTeacher(NewTeacher);

            return RedirectToAction("List");
        }
        /// <summary>
        /// Routes to a dynamically generated "Teacher Update" page.Gets information from the database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A dynamic "Upadate Teacher" webpage which provides the current information of the author and asks the user for new information.</returns>
        ///<example>GET /Teacher/Update/{id}</example>

        public ActionResult Update(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher SelectedTeacher = controller.FindTeacher(id);

            return View(SelectedTeacher);
        }
        /// <summary>
        /// Recieves a POST request with information about an existing teacher, with new values.Conveys this information to the API and redirects to the "Show" page of an updated teacher.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="TeacherFname">The updated first name</param>
        /// <param name="TeacherLname">The updated last name</param>
        /// <param name="TeacherSalary">The updated salary</param>
        /// <returns>A dynamic web page with teachers' current information </returns>
        ///<example>POST /Teacher/Update/{id}</example>
        [HttpPost]

        public ActionResult Update(int id, string TeacherFname, string TeacherLname, string TeacherSalary)
        {
            Teacher TeacherInfo = new Teacher();
            TeacherInfo.TeacherFname = TeacherFname;
            TeacherInfo.TeacherLname = TeacherLname;
            TeacherInfo.TeacherSalary = Convert.ToDecimal(TeacherSalary);

            TeacherDataController controller = new TeacherDataController();
            controller.UpdateTeacher(id, TeacherInfo);
            return RedirectToAction("Show/" + id);
        }
    }
}