using FileWatcherIniParser;
using System;
using System.Collections.Generic;
using System.IO;

namespace FileWatcherService
{
    public static class FolderWatcher
    {
        public static void Initialize()
        {
            IniData.Initialize();

            CreateFolders();
        }

        private static void CreateFolders()
        {
            foreach(var section in IniData.Parameters)
            {
                string folderName = null;
                section.Parameters.TryGetValue(IniData.FOLDER_NAME_KEY,out folderName);

                if(!string.IsNullOrEmpty(IniData.WatchedFolder) && !string.IsNullOrEmpty(folderName))
                {
                    try
                    {
                        var path = Path.Combine(IniData.WatchedFolder, folderName);
                        Directory.CreateDirectory(path);
                    }
                    catch(Exception ex)
                    {
                        //TODO: add logger ;
                    }
                }
            }
        }

        public static void MoveFiles()
        {
            foreach(var setting in IniData.Parameters)
            {
                if (setting.SectionName != IniData.GLOBAL_SETTINGS)
                {
                    string folderName = null;
                    setting.Parameters.TryGetValue(IniData.FOLDER_NAME_KEY, out folderName);
                    string formatValue = null;
                    setting.Parameters.TryGetValue(IniData.FORMAT_KEY, out formatValue);

                    if(folderName != null && formatValue != null)
                    {
                        string[] formats = GetFormats(formatValue);
                        foreach(string format in formats)
                        {
                            MoveFilesByFormat(folderName, format);
                        }
                    }
                }
            }
        }
        private static string[] GetFormats(string formatParameter)
        {
            string[] formats = formatParameter.Split(new char[] { ',' });
            for (int i = 0; i < formats.Length; i++)
            {
                formats[i] = formats[i].Replace(".", string.Empty).Trim();
            }
            return formats;
        }
        private static void MoveFilesByFormat(string folderName, string format)
        {
            try
            {
                List<string> files = new List<string>();

                if (!string.IsNullOrEmpty(IniData.WatchedFolder) && !string.IsNullOrEmpty(folderName))
                {
                    files.AddRange(Directory.GetFiles(IniData.WatchedFolder, $"*.{format.ToLower()}"));
                    files.AddRange(Directory.GetFiles(IniData.WatchedFolder, $"*.{format.ToUpper()}"));
                }
                foreach (var filePath in files)
                {
                    var fileName = Path.GetFileName(filePath);
                    string destPath = Path.Combine(IniData.WatchedFolder, folderName, fileName);
                    File.Move(filePath, destPath);
                }
            }
            catch(Exception ex)
            {
                //TODO add logger exception
            }
        }

    }
}
