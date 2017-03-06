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
        public static Lesson GetLesson(DayOfWeek day, LessonTime lesson_time)
        {
            return MyData.my_lessons_list.Find(x => x.day == day && x.lesson_time == lesson_time);
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

        public static async Task<ContentDialogResult> already_exists_dialog()
        {
            ContentDialog already_exists_dialog = new ContentDialog()
            {
                Title = "Nicht verfügbar!",
                Content = "Du hast bereits eine Stunde an diesem Tag und um diese Uhrzeit eingetragen. Möchtest du es jetzt ersetzen?",
                PrimaryButtonText = "Nein",
                SecondaryButtonText = "Ja"
            };

            ContentDialogResult result = await already_exists_dialog.ShowAsync();
            return result;
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
    public class Time
    {
        [DataMember]
        public int hour { get; set; }
        [DataMember]
        public int minute { get; set; }
        [DataMember]
        public int second { get; set; }

        public Time(int hour = 0, int minute = 0, int second = 0)
        {
            if(0 >= hour || hour < 24)
            {
                this.hour = hour;
            }
            if (0 >= minute || minute < 60)
            {
                this.minute = minute;
            }
            if (0 >= second || second < 60)
            {
                this.second = second;
            }
        }

        public TimeSpan GetTimeSpan()
        {
            return new TimeSpan(hour, minute, second);
        }

        public override string ToString()
        {
            string shour = hour.ToString();
            string sminute = minute.ToString();
            string ssecond = second.ToString();
            string gesamt = "";

            if(hour < 10)
            {
                shour = "0" + hour;
            }
            if (minute < 10)
            {
                sminute = "0" + minute;
            }
            if (second < 10)
            {
                ssecond = "0" + second;
            }

            gesamt = shour + ":" + sminute;
            if(second != 0)
            {
                gesamt += ":" + ssecond;
            }
            return gesamt + " Uhr";
        }
    }

    [DataContract]
    public class LessonTime
    {
        [DataMember]
        public int lesson_number { get; set; }
        [DataMember]
        public Time start_time { get; set; }
        [DataMember]
        public Time end_time { get; set; }
        [DataMember]
        public bool show_as_double { get; set; }
        public TimeSpan total_time { get { return new DateTime(2017,1,1,end_time.hour, end_time.minute, end_time.second).Subtract(new DateTime(2017, 1, 1, start_time.hour, start_time.minute, start_time.second)); } }
        public string readable_type { get { string type = null; if (this.show_as_double == true) { type = "Doppelstunde"; } if (this.show_as_double == false) { type = "Einfache Stunde"; } return type; } }
        public string readable_start_time { get { return this.start_time.ToString(); } }
        public string readable_end_time { get { return this.end_time.ToString(); } }
        public string readable_total_time { get { string hour = " Stunde "; string minute = " Minute"; if (this.total_time.Hours > 1 || this.total_time.Hours == 0) { hour = " Stunden "; } if (this.total_time.Minutes > 1 || this.total_time.Minutes == 0) { minute = " Minuten"; } return this.total_time.Hours + hour + this.total_time.Minutes + minute; } }

        public LessonTime(int given_lesson_number, Time given_start_time, Time given_end_time, bool given_show_as_double)
        {
            this.lesson_number = given_lesson_number;
            this.start_time = given_start_time;
            this.end_time = given_end_time;
            this.show_as_double = given_show_as_double;
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

        public static void DeleteLessonTime(LessonTime time)
        {
            MyData.my_lessontimes_list.Remove(time);
        }
    }
}
