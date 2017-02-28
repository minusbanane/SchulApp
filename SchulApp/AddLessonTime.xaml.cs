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
    public sealed partial class AddLessonTime : Page
    {
        public AddLessonTime()
        {
            this.InitializeComponent();
        }

        private void btn_Done_AddLesson_Click(object sender, RoutedEventArgs e)
        {
            bool exeption_comes = false;
            LessonTime new_lesson_time = new LessonTime(int.Parse(tbx_LessonNumber_AddGrade_Timetable_Settings.Text), tmp_Starttime_AddLesson_Timetable_Settings.Time, tmp_Endtime_AddLesson_Timetable_Settings.Time, tgs_DoubleLesson_AddLessonTime.IsOn);
            try
            {
                new_lesson_time.Save();
            }
            catch (ArgumentException ae)
            {
                exeption_comes = true;
                if (ae.Message == "lessontime already exists")
                {
                    LessonTimeManager.lesson_already_exists_dialog();
                }
            }
            if (exeption_comes == false)
            {
                Frame.GoBack();
            }
        }

        private void tmp_Time_AddLesson_Timetable_Settings_TimeChanged(object sender, TimePickerValueChangedEventArgs e)
        {
            var difference = tmp_Endtime_AddLesson_Timetable_Settings.Time.Subtract(tmp_Starttime_AddLesson_Timetable_Settings.Time);
            string hour = " Stunde ";
            string minute = " Minute";

            if (difference.Hours > 1 || difference.Hours == 0)
            {
                hour = " Stunden ";
            }
            if (difference.Minutes > 1 || difference.Minutes == 0)
            {
                minute = " Minuten";
            }

            tbk_Time_AddLesson_Timetable_Settings.Text = difference.Hours + hour + difference.Minutes + minute;

            if (difference.Hours < 0 || difference.Minutes < 0)
            {
                tbk_Time_AddLesson_Timetable_Settings.Text = "Nicht Möglich!";
                btn_Done_AddLesson.IsEnabled = false;
            }
            else { btn_Done_AddLesson.IsEnabled = true; }
        }
    }
}
