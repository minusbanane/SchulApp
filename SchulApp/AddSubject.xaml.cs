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
using Windows.UI;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SchulApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddSubject : Page
    {
        List<SubjectColor> list_of_colors;
        bool edit = false;
        Subject subject = new Subject();
        public AddSubject()
        {
            this.InitializeComponent();
            var parent = this.Parent;
            list_of_colors = SubjectColorManager.GetList();

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter != null)
            {
                subject = SubjectManager.GetSubject((e.Parameter as Subject).id);
                tbx_Name_AddSubject.Text = subject.subject_name;
                tbx_Teacher_AddSubject.Text = subject.teacher;
                gvw_ColorPicker_AddSubject.SelectedItem = SubjectColorManager.GetList().Find(x => x.name == subject.color.name);

                edit = true;
            }
        }

        private void btn_DoneAddSubject_Click(object sender, RoutedEventArgs e)
        {
            bool erfolgreich = true;
            if(edit)
            {
                SubjectManager.EditSubject(subject.id, tbx_Name_AddSubject.Text, tbx_Teacher_AddSubject.Text, SubjectColorManager.GetList().Find(x => x.name == (gvw_ColorPicker_AddSubject.SelectedItem as SubjectColor).name));
            } else
            {
                erfolgreich = SubjectManager.AddSubject(tbx_Name_AddSubject.Text, tbx_Teacher_AddSubject.Text, (SubjectColor)gvw_ColorPicker_AddSubject.SelectedItem);
            }
            if (erfolgreich == true)
            {
                Frame.GoBack();
            }
        }

        private void btn_Cancel_AddSubject_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Subjects));
        }
    }
}
