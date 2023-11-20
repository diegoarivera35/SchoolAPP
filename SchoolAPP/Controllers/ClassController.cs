using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolAPP.Models;

namespace SchoolAPP.Controllers
{
    public class ClassController : Controller
    {
        // GET: Class
        public ActionResult Index()
        {
            return View();
        }

        // GET: /Classes/List
        public ActionResult List(string SearchKey = null)
        {
            ClassDataController controller = new ClassDataController();
            IEnumerable<Class> Classes = controller.ListClasses(SearchKey);


            return View(Classes);
        }

        //GET: /Class/Show/{id}
        public ActionResult Show(int id)
        {

            ClassDataController controller = new ClassDataController();
            Class NewClass = controller.FindClass(id);





            return View(NewClass);
        }
    }
}