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
    public sealed partial class AddGrade : Page
    {
        public List<GradeType> gradetypes;
        public int grade = 0;
        private bool edit = false;
        private int edit_id;

        public AddGrade()
        {
            this.InitializeComponent();

            gradetypes = GradeType.GetGradeTypes();

            foreach (Subject subject in MyData.my_subjects_list)
            {
                cmb_Subject_AddGrade.Items.Add(subject);
            }
            foreach(GradeType gradetype in gradetypes)
            {
                cmb_GradeType_AddGrade.Items.Add(gradetype);
            }
            
            cmb_GradeType_AddGrade.SelectedItem = gradetypes[0];
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter != null)
            {
                edit_id = (int)e.Parameter;
                Grade edit_grade = GradeManager.GetGrade(edit_id);
                cmb_Subject_AddGrade.SelectedItem = edit_grade.subject;
                foreach(RadioButton rb in rdbs_Grade.Children)
                {
                    if(rb.Tag.ToString() == edit_grade.grade.ToString())
                    {
                        rb.IsChecked = true;
                    }
                }
                cmb_GradeType_AddGrade.SelectedIndex = edit_grade.type.typeid;
                dtp_Date_AddGrade.Date = edit_grade.date;

                edit = true;
            } else
            {
                edit = false;
            }
        }

        private void btn_Done_AddGrade_Click(object sender, RoutedEventArgs e)
        {
            if(cmb_Subject_AddGrade.SelectedIndex != -1 && grade != 0)
            {
                if(edit)
                {
                    GradeManager.DeleteGrade(edit_id);
                }
                Subject selected_subject = (Subject)cmb_Subject_AddGrade.SelectedItem;
                GradeType selected_gradetype = (GradeType)cmb_GradeType_AddGrade.SelectedItem;
                Grade new_grade = new Grade(selected_subject.id, grade, dtp_Date_AddGrade.Date, selected_gradetype);
                new_grade.AddGrade();
                Frame.GoBack();
            } else
            {
                GradeManager.not_completed_dialog();
            }
        }

        private void btn_Cancel_AddGrade_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Grades));
        }

        private void cdp_Date_AddGrade_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {

        }

        private void rdb_Grade_AddGrade_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            grade = int.Parse(rb.Tag.ToString());
        }
    }
}
