using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;

namespace EresData
{
    public class Realization
    {
        public int Id;
        public string Semester;
        public string Subject;
    }

    public class GradesResult
    {
        public Grades data;
        public string msg;

        public GradesResult() { }

        public GradesResult(Grades data)
        {
            this.data = data;
            this.msg = null;
        }

        public GradesResult(string msg)
        {
            this.data = null;
            this.msg = msg;
        }
    }

    public class GradesAccess
    {

        public Semesters createSemester(string name)
        {

            using (var db = new NTR2013Entities())
            {
                var original = db.Semesters.ToList();
                foreach (Semesters s in original)
                {
                    
                    if (s.Name.Trim().Equals(name))
                        return null;
                }

                var semester = new Semesters { Name = name };
                db.Semesters.Add(semester);
                db.SaveChanges();
                return semester;

            }
        }

        public Subjects createSubject(string name, string conspect, string url)
        {
            using (var db = new NTR2013Entities())
            {
                var original = db.Subjects.ToList();
                foreach (Subjects s in original)
                {

                    if (s.Name.Trim().Equals(name))
                        return null;
                }

                var subject = new Subjects { Name = name, Conspect = conspect, url = url };
                db.Subjects.Add(subject);
                db.SaveChanges();
                return subject;
            }
        }

        public Realisations createRealisation(int subjectId, int semesterId)
        {
            using (var db = new NTR2013Entities())
            {
                var realisation = new Realisations { SubjectID = subjectId, SemesterID = semesterId };
                db.Realisations.Add(realisation);
                db.SaveChanges();
                return realisation;
            }
        }

        public Grades createGrade(int realisationId, string name, string maxValue)
        {
            using (var db = new NTR2013Entities())
            {
                var grade = new Grades { RealisationID = realisationId, Name = name, MaxValue = maxValue };
                db.Grades.Add(grade);
                db.SaveChanges();
                return grade;
            }
        }

        public bool removeSemester(int id)
        {
            using (var db = new NTR2013Entities())
            {
                var original = db.Semesters.Find(id);
                if (original != null)
                {
                    db.Semesters.Remove(original);
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public bool removeSubject(int id)
        {
            using (var db = new NTR2013Entities())
            {
                var original = db.Subjects.Find(id);
                if (original != null)
                {
                    db.Subjects.Remove(original);
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public bool removeRealisation(int id)
        {
            using (var db = new NTR2013Entities())
            {
                var original = db.Realisations.Find(id);
                if (original != null)
                {
                    db.Realisations.Remove(original);
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public bool removeGrade(int id)
        {
            using (var db = new NTR2013Entities())
            {
                var original = db.Grades.Find(id);
                if (original != null)
                {
                    db.Grades.Remove(original);
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        private string getSemesterName(int id)
        {
            using (var db = new NTR2013Entities())
            {
                var original = db.Semesters.Find(id);
                return original.Name;
            }
        }

        private string getSubjectName(int id)
        {
            using (var db = new NTR2013Entities())
            {
                var original = db.Subjects.Find(id);
                return original.Name;
            }
        }

        public List<Realization> getRealizations()
        {
            using (var db = new NTR2013Entities())
            {
                List<Realization> result = new List<Realization>();
                List<Realisations> list = db.Realisations.ToList();

                foreach (Realisations r in list)
                {
                    result.Add(new Realization { Id = r.RealisationID, Semester = this.getSemesterName(r.SemesterID), Subject = this.getSubjectName(r.SubjectID) });
                }
                return result;
            }
        }

        public List<Grades> getGrades(int id)
        {
            using (var db = new NTR2013Entities())
            {
                //var db = new NTR2013Entities();
                List<Grades> result = new List<Grades>();
                List<Grades> list = db.Grades.ToList();

                foreach (Grades g in list)
                {
                    if (g.RealisationID == id)
                        result.Add(g);
                }
                return result;
            }
        }

        public GradesResult removeGrades(int id, byte[] currentTimeStamp)
        {
            using (var db = new NTR2013Entities())
            {
                var original = db.Grades.Find(id);
                if (original != null)
                {
                    //Console.WriteLine("sfasdfasdf stamp" + Convert.ToString(original.TimeStamp));
                    if (!compareTimeStamp(original.TimeStamp, currentTimeStamp))
                    {
                        GradesResult result = new GradesResult(ErrorMessage.REMOVE_GRADES_CONCURRENCY);
                        Grades grade = new Grades { GradeID = original.GradeID, RealisationID = original.RealisationID, Name = original.Name, MaxValue = original.MaxValue, TimeStamp = original.TimeStamp, Realisations = null, GradeValues = null };
                        result.data = grade;
                        return result;
                    }

                    List<GradeValues> list = db.GradeValues.ToList();
                    string studentsToEdit = "";

                    foreach (GradeValues g in list)
                    {
                        if (g.GradeID == id)
                        {
                            //Value 10 pleaces
                            if (g.Value.Equals("          "))
                                db.GradeValues.Remove(g);
                            else
                            {
                                //sprawdzenie studentow z ocena
                                var originalRegistration = db.Registrations.Find(g.RegistrationID);
                                var originalStudent = db.Students.Find(originalRegistration.StudentID);
                                studentsToEdit += originalStudent.FirstName + " " + originalStudent.LastName + ", ";
                            }
                        }
                    }

                    if (!studentsToEdit.Equals(""))
                    {
                        studentsToEdit = studentsToEdit.Substring(0, studentsToEdit.Length - 2);
                        return new GradesResult(studentsToEdit);
                    }

                    db.Grades.Remove(original);
                    db.SaveChanges();
                    return new GradesResult(original);
                }

                return new GradesResult(ErrorMessage.REMOVE_GRADES_COULD_NOT_FIND_GRADES_ID);
            }
        }

        //dodaje ocene do realizacji
        //szuka realizacji
        //sprawdza czy name jest unikatowe w realizacji
        public GradesResult addGrades(int realizationId, string name, string maxValue)
        {
            using (var db = new NTR2013Entities())
            {
                var original = db.Realisations.Find(realizationId);
                /*
                List<Realisations> i = db.Realisations.ToList();
                bool f = false;
                foreach (Realisations r in i)
                {
                    if (r.RealisationID == realizationId)
                    {
                        f = true;
                        break;
                    }
                }
                */
                //
                if (original == null)
                    return new GradesResult(ErrorMessage.ADD_GRADES_COULD_NOT_FIND_REALIZATIONS_ID);
                
                List<Grades> list = getGrades(realizationId);
                foreach (Grades g in list)
                {
                    if (g.Name.Equals(name))
                        return new GradesResult(ErrorMessage.ADD_GRADES_NAME_EXISTS);
                }

                Grades grade = new Grades { RealisationID = realizationId, Name = name, MaxValue = maxValue, Realisations = null };
                Grades result = db.Grades.Add(grade);
                db.SaveChanges();
                result.Realisations = null;

                return new GradesResult(result);
            }
        }

        public GradesResult editGrades(int gradeId, string name, string maxValue, byte[] currentTimeStamp)
        {
            using (var db = new NTR2013Entities())
            {
                var original = db.Grades.Find(gradeId);
                if (original != null)
                {
                    if (!compareTimeStamp(original.TimeStamp, currentTimeStamp))
                    {
                        GradesResult result = new GradesResult(ErrorMessage.REMOVE_GRADES_CONCURRENCY);
                        Grades g = new Grades { GradeID = original.GradeID, RealisationID = original.RealisationID, Name = original.Name, MaxValue = original.MaxValue, TimeStamp = original.TimeStamp, Realisations = null, GradeValues = null };
                        result.data = g;
                        return result;
                    }

                    List<Grades> list = getGrades(original.RealisationID);
                    foreach (Grades g in list)
                    {
                        if (g.Name.Equals(name))
                            return new GradesResult(ErrorMessage.EDIT_GRADES_NAME_EXISTS);
                    }

                    if (!original.MaxValue.Equals(maxValue))
                    {
                        List<GradeValues> l = db.GradeValues.ToList();
                        string studentsToEdit = "";

                        foreach (GradeValues g in l)
                        {
                            if (g.GradeID == original.GradeID)
                            {
                                //Value 10 pleaces
                                if (!g.Value.Equals("          "))
                                {
                                    //sprawdzenie studentow z ocena
                                    var originalRegistration = db.Registrations.Find(g.RegistrationID);
                                    var originalStudent = db.Students.Find(originalRegistration.StudentID);
                                    studentsToEdit += originalStudent.FirstName + " " + originalStudent.LastName + ", ";
                                }
                            }
                        }

                        if (!studentsToEdit.Equals(""))
                        {
                            studentsToEdit = studentsToEdit.Substring(0, studentsToEdit.Length - 2);
                            return new GradesResult(studentsToEdit);
                        }
                    }

                    original.Name = name;
                    original.MaxValue = maxValue;
                    //original.TimeStamp = null;
                    db.SaveChanges();

                    Grades grade = new Grades { GradeID = original.GradeID, RealisationID = original.RealisationID, Name = original.Name, MaxValue = original.MaxValue, TimeStamp = original.TimeStamp, Realisations = null, GradeValues = null };


                    return new GradesResult(grade);
                }

                return new GradesResult(ErrorMessage.EDIT_GRADES_COULD_NOT_FIND_GRADES_ID);
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
