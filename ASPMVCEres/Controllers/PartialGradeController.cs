using EresData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ASPMVCEres.Controllers
{
    public class PartialGradeController : Controller
    {
        //
        // GET: /Test/

        public ActionResult Index(int id)
        {
            
            GradesAccess g = new GradesAccess();
            List<Grades> items = g.getGrades(id);

            if (items.Count == 0)
                return Content("Brak ocen cząstkowych dla podanej realizacji.");
            return View(items);
        }

        //
        // GET: /Test/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Test/Create
        /*
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Test/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        */
        //
        // GET: /Test/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Test/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Test/Delete/5

        /*

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Test/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
         * 
         */

        public ActionResult Delete(int gradeId, string stamp)
        {

            GradesAccess g = new GradesAccess();

            //byte[] transfer = GetBytes(stamp);
            
            string[] parts = stamp.Split('?');
            byte[] transfer = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                transfer[i] = Convert.ToByte(parts[i]);
            }
             


            GradesResult r = g.removeGrades(gradeId, transfer);
            //GradesResult r = new GradesResult { msg = null, data = new Grades { GradeID = gradeId } };
            return Json(r);
            //return Json(new { error = r.msg, id = gradeId });
        }

        //async Task<partialviewresult>
        //async Task<JsonResult>
        
        public ActionResult Create(int realizationId, string name, string maxValue)
        {
            //################### NO DB TESTS ###################
            //Grades g = new Grades { GradeID = 789, RealisationID = realizationId, Name = name, MaxValue = maxValue };
            //return Json(g);

            //GradesResult r = new GradesResult { msg = null, data = new Grades { GradeID = 898, RealisationID = realizationId, Name = name, MaxValue = maxValue } };
            //return Json(r);

            GradesAccess g = new GradesAccess();
            GradesResult r = g.addGrades(realizationId, name, maxValue);
            //return Json(r);
            GradesResult r2 = new GradesResult { msg = null, data = new Grades { GradeID = 898, RealisationID = realizationId, Name = name, MaxValue = maxValue, TimeStamp = new byte[8] } };
            return Json(r);
        }

        public ActionResult Save(int gradesId, string name, string maxValue, string stamp)
        {
            GradesAccess g = new GradesAccess();

            string[] parts = stamp.Split('?');
            byte[] transfer = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                transfer[i] = Convert.ToByte(parts[i]);
            }

            GradesResult r = g.editGrades(gradesId, name, maxValue, transfer);
            return Json(r);
            //################### NO DB TESTS ###################
            //GradesResult r = g.editGrades(gradesId, name, maxValue);

            //GradesResult r = new GradesResult { msg = null, data = new Grades { GradeID = gradesId } };
            //return Json(r);
            //return Content("asdf" + realizationId + name + maxValue);
        }

        //string
        public ActionResult AjaxTest(int id)
        {

            return Json(new { error = "some error message" });
            
            //returns value to a div the same like a string
            //return Content("provided id: " + ViewBag.partialGradeName + id);   
        }
    }
}
