using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Microsoft.Toolkit.Uwp;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace SchulApp.Models
{
    public class Setting<T>
    {
        private DataContractSerializer serializer;
        public Setting()
        {
            serializer = new DataContractSerializer(typeof(T));
        }
        private string FileName(T Obj, string Handle)
        {
            var str = String.Concat(Handle, String.Format("{0}", Obj.GetType().ToString()));
            return str;
        }

        public async Task SaveAsync(string Key, T Obj)
        {
            string fileName = FileName(Activator.CreateInstance<T>(), Key);
            try
            {
                if (Obj != null)
                {
                    StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                    IRandomAccessStream writeStream = await file.OpenAsync(FileAccessMode.ReadWrite);
                    using (Stream outStream = Task.Run(() => writeStream.AsStreamForWrite()).Result)
                    {
                        serializer.WriteObject(outStream, Obj);
                        await outStream.FlushAsync();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Delete a stored instance of T from Windows.Storage.ApplicationData
        /// </summary>
        /// <returns></returns>
        public async Task DeleteAsync()
        {
            string fileName = FileName(Activator.CreateInstance<T>(), String.Empty);
            await DeleteAsync(fileName);
        }

        /// <summary>
        /// Delete a stored instance of T with a specified handle from Windows.Storage.ApplicationData.
        /// Specification of a handle supports storage and deletion of different instances of T.
        /// </summary>
        /// <param name="Handle">User-defined handle for the stored object</param>
        public async Task DeleteAsync(string Key)
        {
            if (Key == null)
                throw new ArgumentNullException("Handle");
            string fileName = FileName(Activator.CreateInstance<T>(), Key);
            try
            {
                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                if (file != null)
                {
                    await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieve a stored instance of T with a specified handle from Windows.Storage.ApplicationData.
        /// Specification of a handle supports storage and deletion of different instances of T.
        /// </summary>
        /// <param name="Handle">User-defined handle for the stored object</param>
        public async Task<T> LoadAsync(string Key)
        {
            string fileName = FileName(Activator.CreateInstance<T>(), Key);

            try
            {
                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                IRandomAccessStream readStream = await file.OpenAsync(FileAccessMode.Read);
                if(readStream == null)
                {
                    return default(T);
                }
                using (Stream inStream = Task.Run(() => readStream.AsStreamForRead()).Result)
                {
                    return (T)serializer.ReadObject(inStream);
                }
            }
            catch (FileNotFoundException)
            {
                //file not existing is perfectly valid so simply return the default 
                return default(T);
                //Interesting thread here: How to detect if a file exists (http://social.msdn.microsoft.com/Forums/en-US/winappswithcsharp/thread/1eb71a80-c59c-4146-aeb6-fefd69f4b4bb)
                //throw;
            }
            catch (Exception)
            {
                //Unable to load contents of file
                throw;
            }
        }
    }




    public static class MyData
    {
        public static List<Subject> my_subjects_list = new List<Subject> { };
        public static List<Grade> my_grades_list = new List<Grade> { };
        public static List<LessonTime> my_lessontimes_list = new List<LessonTime> { };
        public static List<Lesson> my_lessons_list = new List<Lesson> { };

        public static async void LoadMyData()
        {
            my_subjects_list = await Load<List<Subject>>("subjects", my_subjects_list);
            my_grades_list = await Load<List<Grade>>("grades", my_grades_list);
            my_lessons_list = await Load<List<Lesson>>("lessons", my_lessons_list);
            my_lessontimes_list = await Load<List<LessonTime>>("lessontimes", my_lessontimes_list);
        }

        public static void SaveMyData()
        {
            Save<Subject>(my_subjects_list, "subjects");
            Save<Grade>(my_grades_list, "grades");
            Save<Lesson>(my_lessons_list, "lessons");
            Save<LessonTime>(my_lessontimes_list, "lessontimes");
        }

        public static async void Save<T>(List<T> list, string filename)
        {
            var helper = new LocalObjectStorageHelper();
            if (list.Count != 0)
            {
                await helper.SaveFileAsync(filename, list);
            }
            else
            {
                if (await StorageFileHelper.FileExistsAsync(ApplicationData.Current.LocalFolder, filename))
                {
                    StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(filename);
                    await file.DeleteAsync();
                }
            }
        }

        public static async Task<T> Load<T>(string filename, T list)
        {
            var helper = new LocalObjectStorageHelper();
            if (await helper.FileExistsAsync(filename))
            {
               return await helper.ReadFileAsync<T>(filename);
            }

            return list;
        }
    }
}
