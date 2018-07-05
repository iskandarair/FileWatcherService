using System;
using System.Collections.Generic;
using IniParser;
using System.Linq;

namespace FileWatcherIniParser
{
    public static class IniData
    {
        public static readonly string INI_FILENAME = "FolderWatcher.ini";
        public static readonly string GLOBAL_SETTINGS = "GlobalSettings";
        public static readonly string WATCHED_FOLDER_KEY = "DownloadsFolders";
        public static readonly string FOLDER_NAME_KEY = "FolderName";
        public static readonly string FORMAT_KEY = "Formats";

        public static string WatchedFolder = string.Empty;
        public static List<SettingsModel> Parameters = new List<SettingsModel>();
        private static readonly string IniFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\" + INI_FILENAME;

        public static void Initialize()
        {
            var iniParser = new FileIniDataParser();
            var data = iniParser.ReadFile(IniFilePath);
            var sections = data.Sections;

            foreach (var section in data.Sections)
            {
                var sectionName = section.SectionName;
                var parameters = new Dictionary<string, string>();

                foreach (var key in data.Sections.GetSectionData(sectionName).Keys)
                {
                    if (sectionName == GLOBAL_SETTINGS && key.KeyName == WATCHED_FOLDER_KEY)
                    {
                        WatchedFolder = key.Value;
                    }

                    parameters.Add(key.KeyName, key.Value);
                }

                var settingsModel = new SettingsModel(sectionName, parameters);
                Parameters.Add(settingsModel);
            }
        }
        public static void Save(List<SettingsModel> settings)
        {

            var iniParser = new FileIniDataParser();
            var data = iniParser.ReadFile(IniFilePath);
            var sections = data.Sections;
            foreach(var section in sections)
            {
                var setting = settings.Where(x => x.SectionName == section.SectionName).FirstOrDefault();
                if(setting != null)
                {
                    foreach(var parameter in setting.Parameters)
                    {
                        data.Sections.GetSectionData(section.SectionName).Keys[parameter.Key]=parameter.Value;
                    }
                }
            }
            iniParser.WriteFile(IniFilePath, data);
        }
    }
}
