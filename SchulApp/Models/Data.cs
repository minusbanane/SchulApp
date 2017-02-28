using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Microsoft.Toolkit.Uwp;

namespace SchulApp.Models
{
    public static class MyData
    {
        public static List<Subject> my_subjects_list = new List<Subject> { };
        public static List<Grade> my_grades_list = new List<Grade> { };
        public static List<LessonTime> my_lessontimes_list = new List<LessonTime> { };
        public static List<Lesson> my_lessons_list = new List<Lesson> { };

        public static void LoadMyData()
        {
            LoadSubjects();
            LoadGrades();
            LoadLessons();
            LoadLessonTimes();
        }

        public static void SaveMyData()
        {
            if(my_subjects_list.Count() != 0)
            {
                SaveSubjects();
            } else
            {
                DeleteFile("subjects_list");
            }

            if (my_grades_list.Count() != 0)
            {
                SaveGrades();
            }
            else
            {
                DeleteFile("grades_list");
            }

            if (my_lessons_list.Count() != 0)
            {
                SaveLessons();
            }
            else
            {
                DeleteFile("lessons_list");
            }

            if (my_lessontimes_list.Count() != 0)
            {
                SaveLessonTimes();
            }
            else
            {
                DeleteFile("lessontimes_list");
            }
        }

        public static async void DeleteFile(string file_name)
        {
            if(await StorageFileHelper.FileExistsAsync(ApplicationData.Current.LocalFolder, file_name))
            {
                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(file_name);
                await file.DeleteAsync();
            }
        }

        public static async void LoadSubjects()
        {
            if(await StorageFileHelper.FileExistsAsync(ApplicationData.Current.LocalFolder, "subjects_list"))
            {
                StorageFile grades_file = await ApplicationData.Current.LocalFolder.GetFileAsync("subjects_list");
                IRandomAccessStream inStream = await grades_file.OpenReadAsync();

                DataContractSerializer deserializer = new DataContractSerializer(typeof(List<Subject>));
                my_subjects_list = (List<Subject>)deserializer.ReadObject(inStream.AsStreamForRead());
                inStream.Dispose();
            }
        }

        public static async void LoadGrades()
        {
            if (await ApplicationData.Current.LocalFolder.TryGetItemAsync("grades_list") != null)
            {
                StorageFile grades_file = await ApplicationData.Current.LocalFolder.GetFileAsync("grades_list");
                IRandomAccessStream inStream = await grades_file.OpenReadAsync();

                DataContractSerializer deserializer = new DataContractSerializer(typeof(List<Grade>));
                my_grades_list = (List<Grade>)deserializer.ReadObject(inStream.AsStreamForRead());
                inStream.Dispose();
            }
        }

        public static async void LoadLessonTimes()
        {
            if (await ApplicationData.Current.LocalFolder.TryGetItemAsync("lessontimes_list") != null)
            {
                StorageFile lessontimes_file = await ApplicationData.Current.LocalFolder.GetFileAsync("lessontimes_list");
                IRandomAccessStream inStream = await lessontimes_file.OpenReadAsync();

                DataContractSerializer deserializer = new DataContractSerializer(typeof(List<LessonTime>));
                my_lessontimes_list = (List<LessonTime>)deserializer.ReadObject(inStream.AsStreamForRead());
                inStream.Dispose();
            }
        }

        public static async void LoadLessons()
        {
            if (await ApplicationData.Current.LocalFolder.TryGetItemAsync("lessons_list") != null)
            {
                StorageFile lessons_file = await ApplicationData.Current.LocalFolder.GetFileAsync("lessons_list");
                IRandomAccessStream inStream2 = await lessons_file.OpenReadAsync();

                DataContractSerializer deserializer2 = new DataContractSerializer(typeof(List<Lesson>));
                my_lessons_list = (List<Lesson>)deserializer2.ReadObject(inStream2.AsStreamForRead());
                inStream2.Dispose();
            }
        }

        public static async void SaveSubjects()
        {
            List<Subject> subjects_list = my_subjects_list;
            StorageFile subjects_file = await ApplicationData.Current.LocalFolder.CreateFileAsync("subjects_list", CreationCollisionOption.OpenIfExists);

            IRandomAccessStream raStream = await subjects_file.OpenAsync(FileAccessMode.ReadWrite);
            using (IOutputStream outStream = raStream.GetOutputStreamAt(0))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(List<Subject>));
                serializer.WriteObject(outStream.AsStreamForWrite(), subjects_list);
                await outStream.FlushAsync();
                outStream.Dispose();
            }
            raStream.Dispose();
        }

        public static async void SaveGrades()
        {
            List<Grade> grades_list = my_grades_list;
            StorageFile grades_file = await ApplicationData.Current.LocalFolder.CreateFileAsync("grades_list", CreationCollisionOption.OpenIfExists);

            IRandomAccessStream raStream = await grades_file.OpenAsync(FileAccessMode.ReadWrite);
            using (IOutputStream outStream = raStream.GetOutputStreamAt(0))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(List<Grade>));
                serializer.WriteObject(outStream.AsStreamForWrite(), grades_list);
                await outStream.FlushAsync();
                outStream.Dispose();
            }
            raStream.Dispose();
        }

        public static async void SaveLessons()
        {
            List<Lesson> lessons_list = my_lessons_list;
            StorageFile lessons_file = await ApplicationData.Current.LocalFolder.CreateFileAsync("lessons_list", CreationCollisionOption.OpenIfExists);

            IRandomAccessStream raStream2 = await lessons_file.OpenAsync(FileAccessMode.ReadWrite);
            using (IOutputStream outStream2 = raStream2.GetOutputStreamAt(0))
            {
                DataContractSerializer serializer2 = new DataContractSerializer(typeof(List<Lesson>));
                serializer2.WriteObject(outStream2.AsStreamForWrite(), lessons_list);
                await outStream2.FlushAsync();
                outStream2.Dispose();
            }
            raStream2.Dispose();
        }

        public static async void SaveLessonTimes()
        {
            List<LessonTime> lessontimes_list = my_lessontimes_list;
            StorageFile lessontimes_file = await ApplicationData.Current.LocalFolder.CreateFileAsync("lessontimes_list", CreationCollisionOption.OpenIfExists);

            IRandomAccessStream raStream = await lessontimes_file.OpenAsync(FileAccessMode.ReadWrite);
            using (IOutputStream outStream = raStream.GetOutputStreamAt(0))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(List<LessonTime>));
                serializer.WriteObject(outStream.AsStreamForWrite(), lessontimes_list);
                await outStream.FlushAsync();
                outStream.Dispose();
            }
            raStream.Dispose();
        }

    }
}
