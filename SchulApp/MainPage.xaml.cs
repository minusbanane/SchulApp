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
using Windows.UI.Core;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SchulApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            frm_Content.Navigate(typeof(Home));
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            if (frm_Content.CanGoBack)
            {
                frm_Content.GoBack();
                e.Handled = true;
            }
        }

        private void btn_Hamburger_MainPage_Click(object sender, RoutedEventArgs e)
        {
            spv_SplitView_MainPage.IsPaneOpen = !spv_SplitView_MainPage.IsPaneOpen;

        }

        private void lbx_SplitView_MainPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Home.IsSelected)
            {
                frm_Content.Navigate(typeof(Home));
            }
            if (Subjects.IsSelected)
            {
                frm_Content.Navigate(typeof(Subjects));
            }
            if (Grades.IsSelected)
            {
                frm_Content.Navigate(typeof(Grades));
            }
            if (Timetable.IsSelected)
            {
                frm_Content.Navigate(typeof(Timetable));
            }
            if (Settings.IsSelected)
            {
                frm_Content.Navigate(typeof(Settings));
            }
            spv_SplitView_MainPage.IsPaneOpen = false;
        }

        public void set_title(string text)
        {
            tbk_Header_MainPage.Text = text;
        }

        private void frm_Content_Navigated(object sender, NavigationEventArgs e)
        {
            Type page = frm_Content.CurrentSourcePageType;

            if (page == typeof(Home))
            {
                tbk_Header_MainPage.Text = "Home";
                lbx_SplitView_MainPage.SelectedItem = Home;
            }
            if (page == typeof(Subjects) || page == typeof(SubjectDetail) || page == typeof(AddSubject) || page == typeof(EditSubject))
            {
                tbk_Header_MainPage.Text = "Fächer";
                lbx_SplitView_MainPage.SelectedItem = Subjects;
            }
            if (page == typeof(Grades) || page == typeof(AddGrade))
            {
                tbk_Header_MainPage.Text = "Noten";
                lbx_SplitView_MainPage.SelectedItem = Grades;
            }
            if (page == typeof(Timetable) || page == typeof(AddLesson) || page == typeof(LessonTimes) || page == typeof(AddLessonTime))
            {
                tbk_Header_MainPage.Text = "Stundenplan";
                lbx_SplitView_MainPage.SelectedItem = Timetable;
            }
            if (page == typeof(Settings))
            {
                tbk_Header_MainPage.Text = "Einstellungen";
                lbx_SplitView_MainPage.SelectedItem = Settings;
            }

            if(frm_Content.CanGoBack)
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            } else
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }
        }

        private void btm_GoBack_MainPage_Click(object sender, RoutedEventArgs e)
        {
            try { frm_Content.GoBack(); } catch { }
        }
    }
}
