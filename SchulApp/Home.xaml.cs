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
using Microsoft.Toolkit.Uwp;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SchulApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Home : Page
    {
        double rdg_average_value;

        public Home()
        {
            this.InitializeComponent();

            htb_Average_Home.Text = GetTotalAverage().ToString();
            SetBestSubjects();
        }

        public void SetBestSubjects()
        {
            List<Subject> best_subjects = GetBestSubject();
            string text = "-";

            if(best_subjects.Count() == 0) { text = "Noch keine Noten eingetragen!"; }
            if(best_subjects.Count() == 1) { text = best_subjects[0].subject_name + " (" + Math.Round(best_subjects[0].average, 2) + ")"; }
            if(best_subjects.Count() > 1)
            {
                text = "";
                foreach(Subject subject in best_subjects)
                {
                    htb_BestSubject_Home.Header = "Beste Fächer";
                    text += subject.subject_name + " (" + Math.Round(subject.average, 2) + ") \n";
                }
            }
            htb_BestSubject_Home.Text = text;
        }

        public double GetTotalAverage()
        {
            double all_averages = 0;
            int number_of_subjects = 0;
            double total_average;

            foreach(Subject subject in SubjectManager.GetSubjects())
            {
                all_averages += (double)subject.average;
                if(subject.average != 0)
                {
                    number_of_subjects += 1;
                }
            }

            if(number_of_subjects != 0)
            {
                total_average = (all_averages / number_of_subjects);
            } else
            {
                total_average = 0;
            }


            return total_average;
        }

        public List<Subject> GetBestSubject()
        {
            List<Subject> best_subjects = new List<Subject> { };
            List<Subject> all_subjects = SubjectManager.GetSubjects();
            all_subjects.RemoveAll(x => x.average == 0);
            all_subjects.Sort((x, y) => x.average.CompareTo(y.average));
            
            decimal last_average = 6;
            foreach(Subject subject in all_subjects)
            {
                if(subject.average < last_average)
                {
                    best_subjects.Clear();
                    best_subjects.Add(subject);
                }
                else if(subject.average == last_average)
                {
                    best_subjects.Add(subject);
                }
                last_average = subject.average;
            }

            return best_subjects;
        }
    }
}
