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
    public sealed partial class LessonTimes : Page
    {
        List<LessonTime> lessontimes_list;

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
    }
}
