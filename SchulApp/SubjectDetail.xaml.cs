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
using SchulApp;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SchulApp.Models
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SubjectDetail : Page
    {
        Subject subject_to_show = new Subject();

        public SubjectDetail()
        {
            this.InitializeComponent();
            subject_to_show = SubjectManager.GetSubject(SubjectManager.selected_subject_id);
        }

        private void btn_Edit_SubjectDetail_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddSubject), subject_to_show);
        }

        private void btn_Delete_SubjectDetail_Click(object sender, RoutedEventArgs e)
        {
            cdg_DeleteSubject();
        }

        public async void cdg_DeleteSubject()
        {
            ContentDialog deletesubject = new ContentDialog()
            {
                Title = "Fach löschen?",
                Content = "Wenn du dieses Fach löschst, werden alle damit verbundenen Daten wie Noten, Stundenplaneinträge, etc. verloren gehen!",
                PrimaryButtonText = "Abbrechen",
                SecondaryButtonText = "Bestätigen"
            };

            ContentDialogResult result = await deletesubject.ShowAsync();


            if (result == ContentDialogResult.Secondary)
            {
                Subject selected_subject = subject_to_show;
                SubjectManager.DeleteSubject(SubjectManager.selected_subject_id);
                Frame.Navigate(typeof(Subjects));
            }
        }

        private void btn_Back_SubjectDetail_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Subjects));
        }
    }
}
