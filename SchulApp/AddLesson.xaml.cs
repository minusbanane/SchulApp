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
    public sealed partial class AddLesson : Page
    {
        public AddLesson()
        {
            this.InitializeComponent();
            fill_data();
        }

        public void fill_data()
        {
            foreach(LessonTime lesson_time in MyData.my_lessontimes_list)
            {
                cmb_LessonTime_AddLesson.Items.Add(lesson_time);
            }

            foreach(Subject subject in SubjectManager.GetSubjects())
            {
                cmb_Subject_AddLesson.Items.Add(subject);
            }
        }

        private void btn_Done_AddLesson_Click(object sender, RoutedEventArgs e)
        {
            int daynumber = cmb_Day_AddLesson.SelectedIndex;
            DayOfWeek day = DayOfWeek.Monday;
            bool day_complete = true;
            LessonTime lessontime = LessonTimeManager.GetLessonTimes()[0];
            bool lessontime_complete = true;
            Subject subject = SubjectManager.GetSubjects()[0];
            bool subject_complete = true;

            if(daynumber == 0)
            {
                day = DayOfWeek.Monday;
            }
            else if (daynumber == 1)
            {
                day = DayOfWeek.Tuesday;
            }
            else if (daynumber == 2)
            {
                day = DayOfWeek.Wednesday;
            }
            else if (daynumber == 3)
            {
                day = DayOfWeek.Thursday;
            }
            else if (daynumber == 4)
            {
                day = DayOfWeek.Friday;
            }
            else
            {
                day_complete = false;
            }

            if(cmb_LessonTime_AddLesson.SelectedIndex != -1)
            {
                lessontime = (LessonTime)cmb_LessonTime_AddLesson.SelectedItem;
            } else
            {
                lessontime_complete = false;
            }

            if(cmb_Subject_AddLesson.SelectedIndex != -1)
            {
                subject = (Subject)cmb_Subject_AddLesson.SelectedItem;
            } else
            {
                subject_complete = false;
            }

            if(day_complete == false || lessontime_complete == false || subject_complete == false)
            {
                LessonManager.not_completed_dialog();
            } else
            {
                LessonManager.AddLesson(new Lesson { day = day, lesson_time = lessontime, subject_id = subject.id });
            }

            Frame.GoBack();
        }
    }
}
