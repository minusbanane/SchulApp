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
    public sealed partial class Subjects : Page
    {
        public List<Subject> subjects_list;

        public Subjects()
        {
            this.InitializeComponent();
            subjects_list = SubjectManager.GetSubjects();
            subjects_list.Sort((x, y) => x.subject_name.CompareTo(y.subject_name));
        }


        private void btn_AddSubject_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddSubject));
        }

        private void lvw_Subjects_ItemClick(object sender, ItemClickEventArgs e)
        {
            btn_SubjectDetail.Visibility = Visibility.Visible;
        }

        private void lvw_Subjects_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var current_subject = (Subject)lvw_Subjects.SelectedItem;
            SubjectManager.selected_subject_id = current_subject.id;
            Frame.Navigate(typeof(SubjectDetail));
        }
    }
}
