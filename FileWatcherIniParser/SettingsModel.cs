using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcherIniParser
{
    public class SettingsModel
    {
        public string SectionName { get; set; }

        public Dictionary<string, string> Parameters = new Dictionary<string, string>();

        public SettingsModel()
        {

        }
        public SettingsModel(string sectionName, Dictionary<string, string> parameters)
        {
            this.SectionName = sectionName;
            this.Parameters = parameters;
        }
    }
}
