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
    public sealed partial class EditSubject : Page
    {
        Subject selected_subject = new Subject();
        List<SubjectColor> list_of_colors;

        public EditSubject()
        {
            this.InitializeComponent();
            selected_subject = SubjectManager.GetSubject(SubjectManager.selected_subject_id);
            list_of_colors = SubjectColorManager.GetList();

            tbx_Name_EditSubject.Text = selected_subject.subject_name;
            tbx_Teacher_EditSubject.Text = selected_subject.teacher;
        }

        private void btn_Done_EditSubject_Click(object sender, RoutedEventArgs e)
        {
            bool erfolgreich = SubjectManager.EditSubject(selected_subject.id, tbx_Name_EditSubject.Text, tbx_Teacher_EditSubject.Text, (SubjectColor)gvw_ColorPicker_EditSubject.SelectedItem);
            
            if(erfolgreich == true)
            {
                Frame.Navigate(typeof(SubjectDetail));
            }
        }

        private void btn_Cancel_EditSubject_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
