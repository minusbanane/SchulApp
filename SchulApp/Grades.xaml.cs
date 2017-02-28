using SchulApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
          //  lvw_Grades.ItemTemplate = dtEnlarged;
        }

        private void btn_Delete_Grades_Click(object sender, RoutedEventArgs e)
        { 
            foreach(Grade grade in lvw_Grades.SelectedItems)
            {
                GradeManager.DeleteGrade(grade.id);
            }
            Frame.Navigate(this.GetType());
            Frame.GoBack(); 
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
        }

        private void btn_CloseMultipleSelection_Grades_Click(object sender, RoutedEventArgs e)
        { 
        }

        private void lvw_Grades_ItemClick(object sender, ItemClickEventArgs e)
        {
        }

        private void btn_MoreInfos_Click(object sender, RoutedEventArgs e)
        {
            StackPanel more_info = (StackPanel)((sender as Button).Parent as Grid).Children[1];
            if(more_info.Visibility == Visibility.Collapsed)
            {
                if (open != null)
                {
                    close_StackPanel(open);
                }
                open_StackPanel(more_info);
            }
            else
            {
                close_StackPanel(more_info);
            }
        }

        private void close_StackPanel(StackPanel stackpanel)
        {
            stackpanel.Visibility = Visibility.Collapsed;
            ((stackpanel.Parent as Grid).Children[0] as Button).Content = "Mehr Infos";
            open = null;
        }

        private void open_StackPanel(StackPanel stackpanel)
        {
            stackpanel.Visibility = Visibility.Visible;
            ((stackpanel.Parent as Grid).Children[0] as Button).Content = "Weniger Infos";
            open = stackpanel;
        }
    }
}
