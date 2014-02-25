using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EresData;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ErestTest
{
    [TestClass]
    public class Test1
    {
        private static GradesAccess g = new GradesAccess();
        private static List<Semesters> semesters = new List<Semesters>();
        private static List<Subjects> subjects = new List<Subjects>();
        private static List<Realisations> realisations = new List<Realisations>();
        private static List<Grades> grades = new List<Grades>();

        [TestInitialize()]
        public void Initialize()
        {

        }

        ///<summary>
        /// Creates a Semester that doesn't exist
        /// </summary>
        [TestMethod]
        public void CreateStemesterDoesntExistTest()
        {   
            Semesters s = createUniqueSemester();

            Assert.IsNotNull(s, "Semestr nie zostal stworzony");
        }

        ///<summary>
        /// Creates a Semester that exists
        /// </summary>
        [TestMethod]
        public void CreateStemesterExistTest()
        {
            Semesters s = createUniqueSemester();
            Semesters s2 = g.createSemester(s.Name);

            Assert.IsNull(s2, "Semestr nie zostal stworzony");
        }

        ///<summary>
        /// Creates a Subject that doesn't exist
        /// </summary>
        [TestMethod]
        public void CreateSubjectDoesntExistTest()
        {
            Subjects s = createUniqueSubject();

            Assert.IsNotNull(s, "Przedmiot nie zostal stworzony");
        }

        ///<summary>
        /// Creates a Subject that exists
        /// </summary>
        [TestMethod]
        public void CreateSubjectExistTest()
        {
            Subjects s = createUniqueSubject();
            Subjects s2 = g.createSubject(s.Name, s.Conspect, s.url);

            Assert.IsNull(s2, "Przedmiot nie zostal stworzony");
        }

        ///<summary>
        /// Removes Grade with current timestamp
        /// </summary>
        [TestMethod]
        public void RemoveGradesCurrnetTimestamp()
        {
            Grades grade = createUniqueGrade();
            //GradesResult result = g.removeGrades(grade.GradeID, grade.TimeStamp);

            Assert.IsNull(g.removeGrades(grade.GradeID, grade.TimeStamp).msg, "Ocena nie zostala usunieta");
        }

        ///<summary>
        /// Removes Grade with not current timestamp
        /// </summary>
        [TestMethod]
        public void RemoveGradesNotCurrnetTimestamp()
        {
            Grades grade = createUniqueGrade();

            byte[] stamp = new byte[8];
            Assert.AreEqual(ErrorMessage.REMOVE_GRADES_CONCURRENCY, g.removeGrades(grade.GradeID, stamp).msg, "Ocena nie zostala zmieniona");
        }

        ///<summary>
        /// Removes Grade with not valid id
        /// </summary>
        [TestMethod]
        public void RemoveGradesDoesntExist()
        {
            Grades grade = createUniqueGrade();
            g.removeGrades(grade.GradeID, grade.TimeStamp);

            Assert.AreEqual(ErrorMessage.REMOVE_GRADES_COULD_NOT_FIND_GRADES_ID, g.removeGrades(grade.GradeID, grade.TimeStamp).msg, "Ocena nie zostala zmieniona");
        }

        ///<summary>
        /// Edits Grade with current timestamp
        /// </summary>
        [TestMethod]
        public void EditGradesCurrnetTimestamp()
        {
            Grades grade = createUniqueGrade();
            //GradesResult result = g.removeGrades(grade.GradeID, grade.TimeStamp);
            GradesResult result = g.editGrades(grade.GradeID, "", grade.MaxValue, grade.TimeStamp);

            Assert.AreEqual("", result.data.Name, "Ocena nie zostala zmieniona");
        }

        ///<summary>
        /// Edit Grade with not valid id
        /// </summary>
        [TestMethod]
        public void EditGradesDoesntExist()
        {
            Grades grade = createUniqueGrade();
            g.removeGrades(grade.GradeID, grade.TimeStamp);

            Assert.AreEqual(ErrorMessage.EDIT_GRADES_COULD_NOT_FIND_GRADES_ID, g.editGrades(grade.GradeID, grade.Name, grade.MaxValue, grade.TimeStamp).msg, "Ocena zostala zmieniona");
        }

        ///<summary>
        /// Controller remove Grade test
        /// </summary>
        [TestMethod]
        public void ControllerRemoveGradeTest()
        {
            Grades grade = createUniqueGrade();
            //g.removeGrades(grade.GradeID, grade.TimeStamp);

            ASPMVCEres.Controllers.PartialGradeController controller = new ASPMVCEres.Controllers.PartialGradeController();

            string stamp = "";
            for (int i = 0; i < 8; i++)
            {
                stamp += grade.TimeStamp[i] + "?";
            }
            stamp = stamp.Substring(0, stamp.Length - 1);

            JsonResult result = (JsonResult)controller.Delete(grade.GradeID, stamp);

            Assert.IsNull(((GradesResult)result.Data).msg, "Ocena nie zostala usunieta");
        }


        [TestCleanup()]
        public void Cleanup()
        {
            //Console.WriteLine("TestMethodCleanup");
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            foreach (Grades grede in grades)
                g.removeGrade(grede.GradeID);
            foreach (Realisations r in realisations)
                g.removeRealisation(r.RealisationID);
            foreach (Semesters s in semesters)
                g.removeSemester(s.SemesterID);
            foreach (Subjects s in subjects)
                g.removeSubject(s.SubjectID);

            
                
            /*
            semesters = new List<Semesters>();
        private List<Subjects> subjects = new List<Subjects>();
        private List<Realisations> realisations = new List<Realisations>();
        private List<Grades> grades = new List<Grades>();
                 * */
            //clean
            //Console.WriteLine("ClassCleanup");
        }

        private Semesters createUniqueSemester()
        {
            while (true)
            {
                Semesters s = g.createSemester(uniqueIndexGenerator());
                if (s != null)
                {
                    semesters.Add(s);
                    return s;
                }
            }
        }

        private Subjects createUniqueSubject()
        {
            while (true)
            {
                Subjects s = g.createSubject(uniqueIndexGenerator(), uniqueIndexGenerator(), uniqueIndexGenerator());
                if (s != null)
                {
                    subjects.Add(s);
                    return s;
                }
            }
        }

        private Realisations createUniqueRealisation()
        {
            while (true)
            {
                Realisations r = g.createRealisation(createUniqueSubject().SubjectID, createUniqueSemester().SemesterID);
                if (r != null)
                {
                    realisations.Add(r);
                    return r;
                }
            }
        }

        private Grades createUniqueGrade()
        {
            while (true)
            {
                Grades grade = g.createGrade(createUniqueRealisation().RealisationID, uniqueIndexGenerator(), uniqueIndexGenerator());
                if (grade != null)
                {
                    grades.Add(grade);
                    return grade;
                }
            }
        }

        static private int _InternalCounter = 0;

        private string uniqueIndexGenerator()
        {
            var now = DateTime.Now;

            var days = (int)(now - new DateTime(2000, 1, 1)).TotalDays;
            var seconds = (int)(now - DateTime.Today).TotalSeconds;

            var counter = _InternalCounter++ % 100;

            return days.ToString("00000") + seconds.ToString("00000");
        }
    }
}
