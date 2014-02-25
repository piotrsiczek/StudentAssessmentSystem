using EresData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASPMVCEres.Controllers
{
    /*
    public class Realization
    {
        public string Semester;
        public string Subject;
    }
     */

    public class GradeController : Controller
    {
        //
        // GET: /Grade/
        /*
        List<Realization> realizations = new List<Realization>
        {
            new Realization {Semester = "13Z", Subject= "NTR"},
            new Realization {Semester = "13Z", Subject= "PAIN"}
        };
        */


        public ActionResult Index()
        {
            ViewBag.Test = "";
            return View();
        }

        public ActionResult Test(string name)
        {
            ViewBag.Test = name;
            return View("Index");
        }

        public ActionResult ShowRealizations()
        {
            GradesAccess g = new GradesAccess();
            List<Realization> items = g.getRealizations();

            foreach (Realization r in items)
            {
                Console.WriteLine(r.Id + " " + r.Semester + " " + r.Subject);
            }


            return View(items);
        }

        public ActionResult ShowPartialGrades(string id)
        {
            GradesAccess g = new GradesAccess();
            List<Grades> items = g.getGrades(2);
            
            return View(items);
        }

    }
}
