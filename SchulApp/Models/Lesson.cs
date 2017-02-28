using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchulApp.Models;
using System.Runtime.Serialization;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace SchulApp.Models
{
    [DataContract]
    public class Lesson
    {
        [DataMember]
        public DayOfWeek day { get; set; }
        [DataMember]
        public LessonTime lesson_time { get; set; }
        [DataMember]
        public int subject_id { get; set; }
        public Subject subject { get { return SubjectManager.GetSubject(this.subject_id); } }
    }

    public static class LessonManager
    {
        public static IEnumerable<IGrouping<DayOfWeek, Lesson>> GetLessonsGrouped()
        {
            var result = from lessons in MyData.my_lessons_list group lessons by lessons.day;
            return result;
        }

        public static List<Lesson> GetAllLessons()
        {
            List<Lesson> lessons_list = MyData.my_lessons_list;
            List<Lesson> return_list = new List<Lesson> { };
            foreach (Lesson lesson in lessons_list)
            {
                return_list.Add(lesson);
            }
            return return_list;
        }

        public static List<Lesson> GetLessonsBySubject(int subject_id)
        {
            List<Lesson> lessons_list = MyData.my_lessons_list.FindAll(x => x.subject.id == subject_id);
            List<Lesson> return_list = new List<Lesson> { };
            foreach (Lesson lesson in lessons_list)
            {
                return_list.Add(lesson);
            }
            return return_list;
        }

        public static void DeleteLesson(DayOfWeek given_day, LessonTime given_lesson_time)
        {
            MyData.my_lessons_list.Remove(MyData.my_lessons_list.Find(x => x.day == given_day && x.lesson_time == given_lesson_time));
        }

        public static void AddLesson(Lesson lesson)
        {
            MyData.my_lessons_list.Add(lesson);
        }

        public static async void not_completed_dialog()
        {
            ContentDialog not_completed_dialog = new ContentDialog()
            {
                Title = "Stunde nicht vollständig!",
                Content = "Du hast noch nicht alle felder ausgefüllt. Prüfe noch einmal nach!",
                PrimaryButtonText = "Alles klar!"
            };

            await not_completed_dialog.ShowAsync();
        }
    }

    [DataContract]
    public class LessonTime
    {
        [DataMember]
        public int lesson_number { get; set; }
        [DataMember]
        public TimeSpan start_time { get; set; }
        [DataMember]
        public TimeSpan end_time { get; set; }
        [DataMember]
        public TimeSpan total_time { get; set; }
        [DataMember]
        public bool show_as_double { get; set; }
        public string readable_type { get { string type = null; if (this.show_as_double == true) { type = "Doppelstunde"; } if (this.show_as_double == false) { type = "Einfache Stunde"; } return type; } }
        public string readable_start_time { get { return this.start_time.ToString(@"hh\:mm"); } }
        public string readable_end_time { get { return this.end_time.ToString(@"hh\:mm"); } }
        public string readable_total_time { get { string hour = " Stunde "; string minute = " Minute"; if (this.total_time.Hours > 1 || this.total_time.Hours == 0) { hour = " Stunden "; } if (this.total_time.Minutes > 1 || this.total_time.Minutes == 0) { minute = " Minuten"; } return this.total_time.Hours + hour + this.total_time.Minutes + minute; } }

        public LessonTime(int given_lesson_number, TimeSpan given_start_time, TimeSpan given_end_time, bool given_show_as_double = false)
        {
            this.lesson_number = given_lesson_number;
            this.start_time = given_start_time;
            this.end_time = given_end_time;
            this.show_as_double = given_show_as_double;
            this.total_time = given_end_time.Subtract(given_start_time);
        }

        public void Save()
        {
            LessonTimeManager.SaveLessonTime(this);
        }
    }

    public static class LessonTimeManager
    {
        public static void SaveLessonTime(LessonTime given_lesson_time)
        {
            if(!MyData.my_lessontimes_list.Exists(x => x.lesson_number == given_lesson_time.lesson_number))
            {
                MyData.my_lessontimes_list.Add(given_lesson_time);
            } else { throw new ArgumentException("lessontime already exists", "lesson_time.lesson_number"); }
        }

        public static async void lesson_already_exists_dialog()
        {
            ContentDialog already_exists = new ContentDialog()
            {
                Title = "Diese Stunde existiert bereits!",
                Content = "Du hast bereits eine Stunde mit dieser Nummer",
                PrimaryButtonText = "Okay!",
            };

            await already_exists.ShowAsync();
        }

        public static List<LessonTime> GetLessonTimes()
        {
            return MyData.my_lessontimes_list;
        }
    }
}
