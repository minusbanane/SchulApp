using SchulApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SchulApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Grades : Page
    {
        List<Grade> grades;
        private DataTemplate dtSmall;
        private DataTemplate dtEnlarged;
        private StackPanel open;

        public Grades()
        {
            this.InitializeComponent();
            grades = GradeManager.GetAllGrades();

            dtSmall = (DataTemplate)Resources["dtm_GradeList_Grades"];
            dtEnlarged = (DataTemplate)Resources["dtm_GradeDetail_Grades"];
        }
        
        private void btn_AddGrade_Grades_Click(object sender, RoutedEventArgs e)
        {
            if(SubjectManager.GetSubjects().Count == 0)
            {
                no_subjects_created();
            } else
            {
                Frame.Navigate(typeof(AddGrade));
            }
        }

        private void lvw_Grades_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lvw_Grades.SelectionMode == ListViewSelectionMode.Single && lvw_Grades.SelectedIndex != -1)
            {
                ListView listview = (ListView)sender;
                ItemsStackPanel itemspanel = (ItemsStackPanel)listview.ItemsPanelRoot;
                ListViewItem item = (ListViewItem)itemspanel.Children[(sender as ListView).SelectedIndex];
                Grid rootgrid = (Grid)item.ContentTemplateRoot;

                toggle_StackPanel(((rootgrid.Children[1] as Grid).Children[0] as StackPanel));
            }
        }

        private async void btn_Delete_Grades_Click(object sender, RoutedEventArgs e)
        {
            if (await delete_grades() == ContentDialogResult.Secondary)
            {
                foreach (Grade grade in lvw_Grades.SelectedItems)
                {
                    GradeManager.DeleteGrade(grade.id);
                }
                Frame.Navigate(this.GetType());
                Frame.GoBack();
            }
        }

        public async void no_subjects_created()
        {
            var result = await SubjectManager.no_subject_created();
            if(result == ContentDialogResult.Primary)
            {
                Frame.Navigate(typeof(AddSubject));
            }
        }

        private void btn_SelectionList_Grades_Click(object sender, RoutedEventArgs e)
        {
            lvw_Grades.SelectionMode = ListViewSelectionMode.Multiple;
            btn_Delete_Grades.Visibility = Visibility.Visible;
            btn_CloseMultipleSelection_Grades.Visibility = Visibility.Visible;
            btn_SelectionList_Grades.Visibility = Visibility.Collapsed;
            if(open != null)
            {
                close_StackPanel(open);
            }
        }

        private void btn_CloseMultipleSelection_Grades_Click(object sender, RoutedEventArgs e)
        {
            lvw_Grades.SelectionMode = ListViewSelectionMode.Single;
            btn_Delete_Grades.Visibility = Visibility.Collapsed;
            btn_CloseMultipleSelection_Grades.Visibility = Visibility.Collapsed;
            btn_SelectionList_Grades.Visibility = Visibility.Visible;
        }

        private void toggle_StackPanel(StackPanel stackpanel)
        {
            if(stackpanel.Visibility == Visibility.Collapsed)
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
            if(await delete_grades() == ContentDialogResult.Secondary)
            {
                GradeManager.DeleteGrade((lvw_Grades.SelectedItem as Grade).id);
                Frame.Navigate(this.GetType());
                Frame.GoBack();
            }
        }

        public static async Task<ContentDialogResult> delete_grades()
        {
            ContentDialog no_subject_created = new ContentDialog()
            {
                Title = "Note löschen?",
                Content = "Bist du dir sicher, dass du diese Note(n) löschen willst? Das kannst du nicht rückgängig machen!",
                PrimaryButtonText = "Abbrechen",
                SecondaryButtonText = "Ich bin sicher"
            };

            ContentDialogResult result = await no_subject_created.ShowAsync();
            return result;
        }

        private void btn_EditGrade_Grades_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddGrade), (lvw_Grades.SelectedItem as Grade).id);
        }
    }
}
