using EresData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASPMVCEres.Controllers
{
    public class GradeValuesTutorController : Controller
    {
        //
        // GET: /GradeValues/

        public ActionResult Index()
        {
            GradeValuesAccess g = new GradeValuesAccess();
            List<Semesters> items = g.getSemesters();

            return View(items);
        }

        
        public ActionResult StudentFilter(int semesterId, int subjectId)
        {
            GradeValuesAccess g = new GradeValuesAccess();
            List<Students> result = g.getStudents(semesterId, subjectId);

            return Json(data: result);
        }

        public ActionResult SubjectFilter(int semesterId)
        {
            GradeValuesAccess g = new GradeValuesAccess();

            return Json(data: g.getSubjects(semesterId));
        }

        public ActionResult GradeValuesFilter(int semesterId, int subjectId, int studentId)
        {
            GradeValuesAccess g = new GradeValuesAccess();

            return Json(data: g.getGradeValues(semesterId, subjectId, studentId));
        }

        public ActionResult Delete(int gradeId, int registrationId, string stamp)
        {
            string[] parts = stamp.Split('?');
            byte[] transfer = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                transfer[i] = Convert.ToByte(parts[i]);
            }

            GradeValuesAccess g = new GradeValuesAccess();
            return Json(g.removeGradeValues(gradeId, registrationId, transfer));
        }

        public ActionResult Add(int gradeId, int registrationId, string value)
        {
            /*
            string[] parts = stamp.Split('?');
            byte[] transfer = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                transfer[i] = Convert.ToByte(parts[i]);
            }
             */

            GradeValuesAccess g = new GradeValuesAccess();

            return Json(g.addGradeValues(gradeId, registrationId, value));
        }

        public ActionResult Update(int gradeId, int registrationId, string value, string stamp)
        {
            
            string[] parts = stamp.Split('?');
            byte[] transfer = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                transfer[i] = Convert.ToByte(parts[i]);
            }
             

            GradeValuesAccess g = new GradeValuesAccess();

            return Json(g.editGradeValue(gradeId, registrationId, value, transfer));
        }


        
        
    }
}
