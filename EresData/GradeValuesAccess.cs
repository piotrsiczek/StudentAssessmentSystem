using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EresData
{
    public class GradeValuesResult
    {
        public GradeValues data;
        public string msg;

        public GradeValuesResult() { }

        public GradeValuesResult(GradeValues data)
        {
            this.data = data;
            this.msg = null;
        }

        public GradeValuesResult(string msg)
        {
            this.data = null;
            this.msg = msg;
        }
    }

    public class GradeValuesAccess
    {
        public List<Students> getStudents()
        {
            using (var db = new NTR2013Entities())
            {
                return db.Students.ToList();
            }     
        }

        //w danym semestrze jedna realizacja przedmiotu
        //student jest raz zapisany na dana realizacje
        public List<Students> getStudents(int semesterId, int subjectId)
        {
            using (var db = new NTR2013Entities())
            {
                int realisationId = db.Realisations.Where(r => r.SemesterID == semesterId && r.SubjectID == subjectId).ToList().ElementAt(0).RealisationID;

                List<Registrations> items = db.Registrations.Where(r => r.RealisationID == realisationId).ToList();
                List<Students> result = new List<Students>();

                foreach (Registrations current in items)
                {
                    var o = db.Students.Find(current.StudentID);
                    result.Add(new Students { StudentID = o.StudentID, FirstName = o.FirstName, LastName = o.LastName, IndexNo = o.IndexNo, GroupID = o.GroupID, TimeStamp = o.TimeStamp, Groups = null, Registrations = null });

                }

                return result;
            }
        }

        public List<Semesters> getSemesters()
        {
            using (var db = new NTR2013Entities())
            {
                return db.Semesters.ToList();
            }
        }

        public List<Semesters> getSemesters(int studentId)
        {
            using (var db = new NTR2013Entities())
            {
                List<Registrations> list = db.Registrations.Where(r => r.StudentID == studentId).ToList();
                List<Semesters> result = new List<Semesters>();

                foreach (Registrations r in list)
                {
                    Realisations realisation = db.Realisations.Find(r.RealisationID);
                    bool semesterFlag = false;

                    foreach(Semesters s in result)
                    {
                        if (s.SemesterID == realisation.SemesterID)
                        {
                            semesterFlag = true;
                            break;
                        }
                    }
                    
                    if (!semesterFlag)
                    {
                        var original = db.Semesters.Find(realisation.SemesterID);
                        result.Add(new Semesters { SemesterID = original.SemesterID, Name = original.Name, TimeStamp = original.TimeStamp, Realisations = null });
                    }

                }

                return result;

            }
        }

        private bool isAlready(List<Subjects> items, int subjectId)
        {
            foreach (Subjects s in items)
            {
                if (s.SubjectID == subjectId)
                    return true;
            }

            return false;
        }

        public List<Subjects> getSubjects(int semesterId)
        {
            using (var db = new NTR2013Entities())
            {
                //return db.Realisations.Include("Subjects").Where(b => b.SemesterID == semesterId).ToList();

                List<Realisations> list = db.Realisations.Where(r => r.SemesterID == semesterId).ToList();
                List<Subjects> result = new List<Subjects>();

                foreach (Realisations r in list)
                {
                    var original = db.Subjects.Find(r.SubjectID);
                    if (!isAlready(result, r.SubjectID))
                        result.Add(new Subjects { SubjectID = original.SubjectID, Name = original.Name, Conspect = original.Conspect, TimeStamp = original.TimeStamp, url = original.url, Realisations = null });
                }

                return result;
            }
        }

        public List<Subjects> getSubjects(int semesterId, int studentId)
        {

            using (var db = new NTR2013Entities())
            {
                //return db.Realisations.Include("Subjects").Where(b => b.SemesterID == semesterId).ToList();

                List<Registrations> list = db.Registrations.Where(r => r.StudentID == studentId).ToList();
                List<Subjects> result = new List<Subjects>();

                foreach (Registrations r in list)
                {
                    var realisations = db.Realisations.Where(re => re.RealisationID == r.RealisationID).ToList().ElementAt(0);

                        if (realisations.SemesterID == semesterId)
                        {
                            var original = db.Subjects.Find(realisations.SubjectID);
                            result.Add(new Subjects { SubjectID = original.SubjectID, Name = original.Name, Conspect = original.Conspect, TimeStamp = original.TimeStamp, url = original.url, Realisations = null });

                        }
                }

                return result;
            }
        }

        public List<Grades> getGradeValues(int semesterId, int subjectId, int studentId)
        {

            using (var db = new NTR2013Entities())
            {
                //return db.Realisations.Include("Subjects").Where(b => b.SemesterID == semesterId).ToList();

                int realizationId = db.Realisations.Where(r => (r.SubjectID == subjectId && r.SemesterID == semesterId)).ToList().ElementAt(0).RealisationID;

                int registrationId = db.Registrations.Where(r => r.StudentID == studentId && r.RealisationID == realizationId).ToList().ElementAt(0).RegistrationID;


                //b => b.Posts.Select(p => p.Comments)
                var gradeList = db.Grades.Include("GradeValues").Where(g => g.RealisationID == realizationId).ToList();
                
                //var original = db.GradeValues.Include("Grades").Where(g => g.RegistrationID == registrationId).ToList();
                List<Grades> result = new List<Grades>();

                foreach (Grades grades in gradeList)
                {
                    bool flag = false;

                    foreach (GradeValues g in grades.GradeValues)
                    {
                        if (g.RegistrationID == registrationId)
                        {
                            ICollection<GradeValues> values = new List<GradeValues>();
                            values.Add(new GradeValues() { GradeValueID = g.GradeValueID, GradeID = g.GradeID, RegistrationID = g.RegistrationID, Value = g.Value, Date = g.Date, TimeStamp = g.TimeStamp, Grades = null, Registrations = null });
                            result.Add(new Grades { GradeID = grades.GradeID, Name = grades.Name, MaxValue = grades.MaxValue, RealisationID = grades.RealisationID, TimeStamp = grades.TimeStamp, Realisations = null, GradeValues = values });
                            flag = true;
                            break;
                        }
                    }

                    if (!flag)
                    {
                        result.Add(new Grades { GradeID = grades.GradeID, Name = grades.Name, MaxValue = grades.MaxValue, RealisationID = grades.RealisationID, TimeStamp = grades.TimeStamp, GradeValues = null, Realisations = null });
                    }

                }
               

                return result;

            }
        }

        //GradeValues functions

        public GradeValuesResult addGradeValues(int gradeId, int registrationId, string value)
        {
            using (var db = new NTR2013Entities())
            {
                GradeValues grade = new GradeValues { GradeID = gradeId, RegistrationID = registrationId, Date = "2012-11-17", Value = value, Grades = null, Registrations = null, TimeStamp = null };
                GradeValues result = db.GradeValues.Add(grade);
                db.SaveChanges();
                result.Grades = null;
                result.Registrations = null;

                return new GradeValuesResult(result);
            }
        }

        public GradeValuesResult removeGradeValues(int gradeId, int registrationId, byte[] currentTimeStamp)
        {
            using (var db = new NTR2013Entities())
            {
                var original = db.GradeValues.Where(g => g.GradeID == gradeId && g.RegistrationID == registrationId).ToList().ElementAt(0);

                if (!compareTimeStamp(original.TimeStamp, currentTimeStamp))
                {
                    GradeValuesResult result = new GradeValuesResult(ErrorMessage.REMOVE_GRADES_CONCURRENCY);
                    GradeValues grade = new GradeValues { GradeValueID = original.GradeValueID, GradeID = original.GradeID, RegistrationID = original.RegistrationID, Date = original.Date, Value = original.Value, TimeStamp = original.TimeStamp, Grades = null, Registrations = null };
                    result.data = grade;
                    return result;
                }

                db.GradeValues.Remove(original);
                db.SaveChanges();
                return new GradeValuesResult(original);
            }
        }

        public GradeValuesResult editGradeValue(int gradeId, int registrationId, string value, byte[] currentTimeStamp)
        {
            using (var db = new NTR2013Entities())
            {
                var original = db.GradeValues.Where(g => g.GradeID == gradeId && g.RegistrationID == registrationId).ToList().ElementAt(0);

                if (!compareTimeStamp(original.TimeStamp, currentTimeStamp))
                {
                    GradeValuesResult result = new GradeValuesResult(ErrorMessage.REMOVE_GRADES_CONCURRENCY);
                    GradeValues grade = new GradeValues { GradeValueID = original.GradeValueID, GradeID = original.GradeID, RegistrationID = original.RegistrationID, Date = original.Date, Value = original.Value, TimeStamp = original.TimeStamp, Grades = null, Registrations = null };
                    result.data = grade;
                    return result;
                }

                //db.GradeValues.Remove(original);
                original.Value = value;
                db.SaveChanges();

                GradeValues current = new GradeValues { GradeValueID = original.GradeValueID, GradeID = original.GradeID, RegistrationID = original.RegistrationID, Date = original.Date, Value = original.Value, TimeStamp = original.TimeStamp, Grades = null, Registrations = null };
                return new GradeValuesResult(current);
            }
        }




        private bool compareTimeStamp(byte[] row1, byte[] row2)
        {
            for (int i = 0; i < 8; i++)
            {
                if (row1[i] != row2[i])
                    return false;
            }

            return true;
        }
    }
}
