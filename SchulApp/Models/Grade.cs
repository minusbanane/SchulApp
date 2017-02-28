using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace SchulApp.Models
{
    [DataContract]
    public class Grade
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public int subject_id { get; set; }
        [DataMember]
        public int grade { get; set; }
        [DataMember]
        public DateTimeOffset date { get; set; }
        [DataMember]
        public string readable_date { get { return this.date.Day.ToString() + "." + this.date.Month.ToString() + "." + this.date.Year.ToString(); } }
        [DataMember]
        public GradeType type { get; set; }
        public Subject subject { get{ return SubjectManager.GetSubject(this.subject_id); } }

        public Grade(int given_subject_id, int given_grade, DateTimeOffset given_date, GradeType given_type)
        {
            subject_id = given_subject_id;
            grade = given_grade;
            date = given_date;
            type = given_type;
        }

        public void AddGrade()
        {
            this.id = GradeManager.FreeId();
            MyData.my_grades_list.Add(this);
        }

        public void DeleteGrade()
        {
            GradeManager.DeleteGrade(this.id);
        }
    }

    public static class GradeManager
    {
        public static List<Grade> selected_grades;

        public static int FreeId()
        {
            List<Grade> grades_list = MyData.my_grades_list;
            grades_list.Sort((x, y) => x.id.CompareTo(y.id));
            int last_id = 0;
            if (grades_list.Count != 0)
            {
                Grade last_grade = grades_list.Last();
                last_id = last_grade.id;
            }
            int free_id = last_id + 1;
            return free_id;
        }

        public static void AddGrade(Grade given_grade)
        {
            given_grade.id = GradeManager.FreeId();
            MyData.my_grades_list.Add(given_grade);
        }

        public static List<Grade> GetGradesBySubject(int given_subject_id)
        {
            List<Grade> grades_list = MyData.my_grades_list.FindAll(x => x.subject_id == given_subject_id);
            List<Grade> return_list = new List<Grade> { };
            foreach (Grade grade in grades_list)
            {
                return_list.Add(grade);
            }
            return return_list;
        }

        public static List<Grade> GetAllGrades()
        {
            List<Grade> grades_list = MyData.my_grades_list;
            List<Grade> return_list = new List<Grade> { };
            foreach(Grade grade in grades_list)
            {
                return_list.Add(grade);
            }
            return return_list;
        }

        public static void DeleteGrade(int given_id)
        {
            MyData.my_grades_list.Remove(MyData.my_grades_list.Find(x => x.id == given_id));
        }

        public static async void not_completed_dialog()
        {
            ContentDialog not_completed_dialog = new ContentDialog()
            {
                Title = "Note nicht vollständig!",
                Content = "Du hast noch nicht alle felder ausgefüllt. Prüfe noch einmal nach!",
                PrimaryButtonText = "Alles klar!"
            };

            await not_completed_dialog.ShowAsync();
        }
    }

    [DataContract]
    public class GradeType
    {
        [DataMember]
        public string typename { get; set; }
        [DataMember]
        public string readable_typename { get; set; }
        [DataMember]
        public int typeid { get; set; }
        [DataMember]
        public int rate { get; set; }

        public GradeType(string given_typename)
        {
            if (given_typename == "normaltest")
            {
                this.typename = "normaltest";
                this.readable_typename = "Test";
                this.typeid = 2;
                this.rate = 50;
            }

            if (given_typename == "classwork")
            {
                this.typename = "classwork";
                this.readable_typename = "Klassenarbeit";
                this.typeid = 2;
                this.rate = 50;
            }
        }

        public static List<GradeType> GetGradeTypes()
        {
            List<GradeType> gradetypes = new List<GradeType> { };
            gradetypes.Add(new GradeType("normaltest"));
            gradetypes.Add(new GradeType("classwork"));

            return gradetypes;
        }
    }
}
