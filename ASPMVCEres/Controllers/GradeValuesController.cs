using EresData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASPMVCEres.Controllers
{
    public class GradeValuesController : Controller
    {
        //
        // GET: /GradeValues/

        public ActionResult Index()
        {
            GradeValuesAccess g = new GradeValuesAccess();
            List<Students> items = g.getStudents();

            return View(items);
        }

        public ActionResult SemesterFilter(int studentId)
        {
            GradeValuesAccess g = new GradeValuesAccess();

            /*
            List<Semesters> l = g.getSemesters();
            List<Semesters> result = new List<Semesters>();

            foreach (Semesters item in l)
            {
                result.Add(new Semesters { SemesterID = item.SemesterID, Name = item.Name, TimeStamp = item.TimeStamp, Realisations = null });
            }
            */

            List<Semesters> result = g.getSemesters(studentId);

            return Json(data: result);
        }

        public ActionResult SubjectFilter(int semesterId, int studentId)
        {
            GradeValuesAccess g = new GradeValuesAccess();

            return Json(data: g.getSubjects(semesterId, studentId));
        }

        public ActionResult GradeValuesFilter(int semesterId, int subjectId, int studentId)
        {
            GradeValuesAccess g = new GradeValuesAccess();

            return Json(data: g.getGradeValues(semesterId, subjectId, studentId));
        } 


        


    }
}
