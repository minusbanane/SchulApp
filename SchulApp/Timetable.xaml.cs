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
using System.Threading.Tasks;

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

        private ListView selected_listview;
        private StackPanel open;

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
            selected_listview = lvw_Lessons_Monday;
        }

        private void btn_LessonTimes_Timetable_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LessonTimes));
        }

        private void btn_AddLesson_Timetable_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddLesson), pvt_TimeTable.SelectedIndex);
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (pvt_TimeTable.SelectedIndex)
            {
                case 0:
                    selected_listview = lvw_Lessons_Monday;
                    break;

                case 1:
                    selected_listview = lvw_Lessons_Tuesday;
                    break;

                case 2:
                    selected_listview = lvw_Lessons_Wednesday;
                    break;

                case 3:
                    selected_listview = lvw_Lessons_Thursday;
                    break;

                case 4:
                    selected_listview = lvw_Lessons_Friday;
                    break;
            }
        }

        private void lvw_Lessons_Monday_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ListView).SelectionMode == ListViewSelectionMode.Single && (sender as ListView).SelectedIndex != -1)
            {
                ListView listview = (ListView)sender;
                ItemsStackPanel itemspanel = (ItemsStackPanel)listview.ItemsPanelRoot;
                ListViewItem item = (ListViewItem)itemspanel.Children[(sender as ListView).SelectedIndex];
                Grid rootgrid = (Grid)item.ContentTemplateRoot;

                toggle_StackPanel((rootgrid.Children[1] as StackPanel));
            }
        }

        private void toggle_StackPanel(StackPanel stackpanel)
        {
            if (stackpanel.Visibility == Visibility.Collapsed)
            {
                if (open != null)
                {
                    close_StackPanel(open);
                }
                open_StackPanel(stackpanel);
            }
            else
            {
                close_StackPanel(stackpanel);
            }
        }

        private void close_StackPanel(StackPanel stackpanel)
        {
            stackpanel.Visibility = Visibility.Collapsed;
            open = null;
        }

        private void open_StackPanel(StackPanel stackpanel)
        {
            stackpanel.Visibility = Visibility.Visible;
            open = stackpanel;
        }

        public static async Task<ContentDialogResult> delete_lessons()
        {
            ContentDialog no_subject_created = new ContentDialog()
            {
                Title = "Stunde löschen?",
                Content = "Bist du dir sicher, dass du diese Stunde löschen willst? Das kannst du nicht rückgängig machen!",
                PrimaryButtonText = "Abbrechen",
                SecondaryButtonText = "Ich bin sicher"
            };

            ContentDialogResult result = await no_subject_created.ShowAsync();
            return result;
        }

        private async void btn_DeleteLesson_Lessons_Click(object sender, RoutedEventArgs e)
        {
            if (await delete_lessons() == ContentDialogResult.Secondary)
            {
                LessonManager.DeleteLesson((selected_listview.SelectedItem as Lesson).day, (selected_listview.SelectedItem as Lesson).lesson_time);
                Frame.Navigate(this.GetType());
                Frame.GoBack();
            }
        }

        private void btn_EditLesson_Lessons_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddLesson), selected_listview.SelectedItem as Lesson);
        }
    }
}
