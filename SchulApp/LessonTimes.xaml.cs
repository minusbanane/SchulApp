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
    public sealed partial class LessonTimes : Page
    {
        List<LessonTime> lessontimes_list;
        StackPanel open;

        public LessonTimes()
        {
            this.InitializeComponent();
            lessontimes_list = MyData.my_lessontimes_list;
            lessontimes_list.Sort((x, y) => x.lesson_number.CompareTo(y.lesson_number));
        }

        private void btn_AddLesson_LessonTimes_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddLessonTime));
        }

        private void lvw_LessonTimes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvw_LessonTimes.SelectionMode == ListViewSelectionMode.Single && lvw_LessonTimes.SelectedIndex != -1)
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

        private async void btn_DeleteGrade_Grades_Click(object sender, RoutedEventArgs e)
        {
            if (await delete_grades() == ContentDialogResult.Secondary)
            {
                LessonTimeManager.DeleteLessonTime((lvw_LessonTimes.SelectedItem as LessonTime));
                Frame.Navigate(this.GetType());
                Frame.GoBack();
            }
        }

        public static async Task<ContentDialogResult> delete_grades()
        {
            ContentDialog no_subject_created = new ContentDialog()
            {
                Title = "Stundenzeit löschen?",
                Content = "Bist du dir sicher, dass du diese Stundenzeit(en) löschen willst? Das kannst du nicht rückgängig machen!",
                PrimaryButtonText = "Abbrechen",
                SecondaryButtonText = "Ich bin sicher"
            };

            ContentDialogResult result = await no_subject_created.ShowAsync();
            return result;
        }

        private void btn_EditGrade_Grades_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddLessonTime), lvw_LessonTimes.SelectedItem as LessonTime );
        }
    }
}
