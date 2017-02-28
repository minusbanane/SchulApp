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
        public AddSubject()
        {
            this.InitializeComponent();
            var parent = this.Parent;
            list_of_colors = SubjectColorManager.GetList();
        }

        private void btn_DoneAddSubject_Click(object sender, RoutedEventArgs e)
        {
            bool erfolgreich = SubjectManager.AddSubject(tbx_Name_AddSubject.Text, tbx_Teacher_AddSubject.Text, (SubjectColor)gvw_ColorPicker_AddSubject.SelectedItem);
            
            if(erfolgreich == true)
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
