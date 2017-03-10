using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Windows;
using Windows.UI.Xaml.Media;
using SchulApp.Models;

namespace SchulApp.Models
{
    [DataContract]
    public class Subject
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string subject_name { get; set; }
        [DataMember]
        public string teacher { get; set; }
        [DataMember]
        public SubjectColor color { get; set; }
        public decimal average { get { return SubjectManager.GetAverage(this.id); } }
        public List<Grade> grades { get { return GradeManager.GetGradesBySubject(this.id); } }
        public string first_letter { get { return this.subject_name[0].ToString(); }}

        public void DeleteSubject()
        {
            SubjectManager.DeleteSubject(this.id);
        }
    }

    public static class SubjectManager
    {
        public static int selected_subject_id { get; set; }

        public static bool AddSubject(string given_subject_name, string given_teacher, SubjectColor given_color, int given_id = 0)
        {
            bool erfolgreich;

            if(given_subject_name != "" && given_color != null)
            {
                Subject new_subject = new Subject() { subject_name = given_subject_name, teacher = given_teacher, color = given_color };
                if (given_id == 0)
                {
                    new_subject.id = SubjectManager.FreeId();
                }
                else
                {
                    new_subject.id = given_id;
                }
                MyData.my_subjects_list.Add(new_subject);
                erfolgreich = true;
            } else { not_completed_dialog(); erfolgreich = false; }

            return erfolgreich;
        }

        public static int FreeId()
        {
            List<Subject> subjects_list = MyData.my_subjects_list;
            subjects_list.Sort((x, y) => x.id.CompareTo(y.id));
            Subject last_subject = new Subject() { id = 0};
            if(subjects_list.Count() != 0)
            {
                last_subject = subjects_list.Last();
            }
            int last_id = last_subject.id;
            int free_id = last_id + 1;
            return free_id;
        }

        public static Subject GetSubject(int given_id)
        {
            Subject found_subject = new Subject();
            found_subject = MyData.my_subjects_list.Find(x => x.id == given_id);
            return found_subject;
        }

        public static List<Subject> GetSubjects()
        {
            List<Subject> subjects_list = MyData.my_subjects_list;
            List<Subject> return_list = new List<Subject> { };
            foreach(Subject subject in subjects_list)
            {
                return_list.Add(subject);
            }
            return return_list;
        }

        public static void DeleteSubject(int id)
        {
            List<Grade> grades = GradeManager.GetGradesBySubject(id);
            foreach(Grade grade in grades)
            {
                GradeManager.DeleteGrade(grade.id);
            }
            List<Lesson> lessons = LessonManager.GetLessonsBySubject(id);
            foreach(Lesson lesson in lessons)
            {
                LessonManager.DeleteLesson(lesson.day, lesson.lesson_time);
            }
            MyData.my_subjects_list.Remove(MyData.my_subjects_list.Find(x => x.id == id));
        }

        public static bool EditSubject(int given_id, string new_subject_name, string new_teacher, SubjectColor new_color)
        {
            bool erfolgreich = true;

            if(!String.IsNullOrWhiteSpace(new_subject_name) && !String.IsNullOrWhiteSpace(new_teacher))
            {
                MyData.my_subjects_list.Find(x => x.id == given_id).subject_name = new_subject_name;
                MyData.my_subjects_list.Find(x => x.id == given_id).teacher = new_teacher;
            } else { not_completed_dialog(); erfolgreich = false; }
            if (new_color != null)
            {
                MyData.my_subjects_list.Find(x => x.id == given_id).color = new_color;
            }

            /*if(new_subject_name != "")
            {
                Subject new_subject = new Subject() { };
                if(new_color == null)
                {
                    new_color = MyData.my_subjects_list.Find(x => x.id == given_id).color;
                }
                SubjectManager.DeleteSubject(given_id);
                SubjectManager.AddSubject(new_subject_name, new_teacher, new_color, given_id);
                List<Grade> grades_to_edit = GradeManager.GetGradesBySubject(given_id);
                foreach (Grade grade in grades_to_edit)
                {
                    GradeManager.DeleteGrade(grade.id);
                    grade.subject_name = new_subject_name;
                    GradeManager.AddGrade(grade);
                }
                erfolgreich = true;
            } */

            return erfolgreich;
        }

        public static decimal GetAverage(int subject_id)
        {
            decimal allclassworks = 0;
            decimal allnormaltests = 0;
            decimal average = 0;
            decimal allclassworksaverage = 0;
            decimal allnormaltestsaverage = 0;
            List<Grade> classwork_list = new List<Grade> { };
            List<Grade> normaltest_list = new List<Grade> { };

            Subject actual_subject = SubjectManager.GetSubject(subject_id);
            foreach(Grade grade in actual_subject.grades)
            {
                if(grade.type.typename == "classwork")
                {
                    classwork_list.Add(grade);
                }
                if (grade.type.typename == "normaltest")
                {
                    normaltest_list.Add(grade);
                }
            }

            foreach(Grade grade in classwork_list)
            {
                allclassworks += grade.grade;
            }
            foreach(Grade grade in normaltest_list)
            {
                allnormaltests += grade.grade;
            }

            if(allclassworks != 0)
            {
                allclassworksaverage = allclassworks / classwork_list.Count();
            }
            if(allnormaltests != 0)
            {
                allnormaltestsaverage = allnormaltests / normaltest_list.Count();
            }

            if(allclassworksaverage != 0 && allnormaltestsaverage != 0)
            {
                average = (allclassworksaverage + allnormaltestsaverage) / 2;
            } else
            {
                if(allclassworksaverage == 0 && allnormaltestsaverage != 0)
                {
                    average = allnormaltestsaverage;
                }
                if(allnormaltestsaverage == 0 && allclassworksaverage != 0)
                {
                    average = allclassworksaverage;
                }
            }

            average = Math.Round(average, 2);

            return average;
        }

        public static async void not_completed_dialog()
        {
            ContentDialog not_completed = new ContentDialog()
            {
                Title = "Fach nicht vollständig",
                Content = "Du hast noch nicht alle erforderlichen Felder ausgefüllt. Prüfe noch einmal nach!",
                PrimaryButtonText = "Alles klar!",
            };

            await not_completed.ShowAsync();
        }

        public static async Task<ContentDialogResult> no_subject_created()
        {
            ContentDialog no_subject_created = new ContentDialog()
            {
                Title = "Es existiert noch kein Fach!",
                Content = "Bevor du Noten eintragen kannst, musst du erst mindestens ein Fach erstellt haben!",
                PrimaryButtonText = "Jetzt erstellen!",
                SecondaryButtonText = "Vielleicht später!"
            };

            ContentDialogResult result = await no_subject_created.ShowAsync();
            return result;
        }
    }

    [DataContract]
    public class SubjectColor
    {
        [DataMember]
        public Color color { get; set; }
        [DataMember]
        public string name { get; set; }
        public SolidColorBrush brush { get{ return new SolidColorBrush(this.color); } }
    }

    public static class SubjectColorManager
    {
        public static List<SubjectColor> list_of_colors = new List<SubjectColor> { };

        public static List<SubjectColor> GetList()
        {
            list_of_colors = new List<SubjectColor> { };

            list_of_colors.Add(new SubjectColor { color = Colors.Red, name = "Rot" });
            list_of_colors.Add(new SubjectColor { color = Colors.Blue, name = "Blau" });
            list_of_colors.Add(new SubjectColor { color = Colors.Green, name = "Grün" });
            list_of_colors.Add(new SubjectColor { color = Colors.Yellow, name = "Gelb" });
            list_of_colors.Add(new SubjectColor { color = Colors.Brown, name = "Braun" });
            list_of_colors.Add(new SubjectColor { color = Colors.Gray, name = "Grau" });
            list_of_colors.Add(new SubjectColor { color = Colors.Violet, name = "Violett" });
            list_of_colors.Add(new SubjectColor { color = Colors.Pink, name = "Pink" });
            list_of_colors.Add(new SubjectColor { color = Colors.White, name = "Weiß" });
            list_of_colors.Add(new SubjectColor { color = Colors.Black, name = "Schwarz" });
            list_of_colors.Add(new SubjectColor { color = Colors.Cyan, name = "Zyan" });
            list_of_colors.Add(new SubjectColor { color = Colors.DarkBlue, name = "Dunkelblau" });
            list_of_colors.Add(new SubjectColor { color = Colors.Orange, name = "Orange" });

            return list_of_colors;
        }
    }
}
