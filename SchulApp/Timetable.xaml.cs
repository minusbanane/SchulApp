using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SchulApp.Models;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SchulApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Timetable : Page
    {
        public List<Lesson> monday_list = new List<Lesson> { };
        public List<Lesson> tuesday_list = new List<Lesson> { };
        public List<Lesson> wednesday_list = new List<Lesson> { };
        public List<Lesson> thursday_list = new List<Lesson> { };
        public List<Lesson> friday_list = new List<Lesson> { };
        public List<List<Lesson>> days_list = new List<List<Lesson>> { };

        public Timetable()
        {
            this.InitializeComponent();
            LoadLessons();
        }

        private void LoadLessons()
        {
            foreach (Lesson lesson in LessonManager.GetAllLessons())
            {
                if (lesson.day == DayOfWeek.Monday)
                {
                    monday_list.Add(lesson);
                }
                else if (lesson.day == DayOfWeek.Tuesday)
                {
                    tuesday_list.Add(lesson);
                }
                else if (lesson.day == DayOfWeek.Wednesday)
                {
                    wednesday_list.Add(lesson);
                }
                else if (lesson.day == DayOfWeek.Thursday)
                {
                    thursday_list.Add(lesson);
                }
                else if (lesson.day == DayOfWeek.Friday)
                {
                    friday_list.Add(lesson);
                }
            }

            days_list.Add(monday_list);
            days_list.Add(tuesday_list);
            days_list.Add(wednesday_list);
            days_list.Add(thursday_list);
            days_list.Add(friday_list);

            foreach(List<Lesson> day_list in days_list)
            {
                day_list.Sort((x, y) => x.lesson_time.lesson_number.CompareTo(y.lesson_time.lesson_number));
            }
        }

        private void btn_LessonTimes_Timetable_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LessonTimes));
        }

        private void btn_AddLesson_Timetable_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddLesson));
        }
    }
}
