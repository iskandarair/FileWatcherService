using System;
using FileWatcherIniParser;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;

namespace FileWatcherManager
{
    public partial class FolderWatcherWindows : Form
    {
        private readonly string SUBFOLDER_CONTROL_NAME = "tbxFolderName";
        private readonly string SUBFOLDER_CONTROL_FORMATS = "tbxFolderFormats";

        private readonly string SERVICE_EXE_NAME = "FileWatcherService";
        public FolderWatcherWindows()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            InitFolderWatcher();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "The app watches the specified folder and hourly inspects it for formats to move them to destined subfolders." + 
                Environment.NewLine+ Environment.NewLine +
                "Formats are case in-sensitive should be separapted by comma(,). Specified format files will move to correpsonding sub-folder." +
                Environment.NewLine + Environment.NewLine +
                "After saving changes the Folder Watcher starts inspecting.","About Application" +
                Environment.NewLine + Environment.NewLine +
                "It works as windows service. No need to keep it open"
                );
        }

        private void btnFolder_Click(object sender, EventArgs e)
        {
            fbdFolder.ShowDialog();
            btnFolder.Text = fbdFolder.SelectedPath;
        }
        private void InitFolderWatcher()
        {
            IniData.Initialize();
            btnFolder.Text = IniData.WatchedFolder;

            var parameters = IniData.Parameters.Where(x => x.SectionName.ToLower().Contains("folder")).ToList();
            int i = 0;
            foreach(var parameter in parameters)
            {
                i++;
                var tbxfolderName = this.Controls.Find($"{SUBFOLDER_CONTROL_NAME}{i}", true)[0] as TextBox;
                var tbxfolderFormats = this.Controls.Find($"{SUBFOLDER_CONTROL_FORMATS}{i}", true)[0] as TextBox;
                if (tbxfolderName != null)
                {
                    string folderName = null;
                    parameter.Parameters.TryGetValue(IniData.FOLDER_NAME_KEY, out folderName);
                    tbxfolderName.Text = folderName;
                }
                if(tbxfolderFormats !=null)
                {
                    string folderFormats = null;
                    parameter.Parameters.TryGetValue(IniData.FORMAT_KEY, out folderFormats);
                    tbxfolderFormats.Text = folderFormats;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<SettingsModel> models = new List<SettingsModel>();
            for(int i = 1; i <= 5; i++)
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();

                var tbxfolderName = this.Controls.Find($"{SUBFOLDER_CONTROL_NAME}{i}", true)[0] as TextBox;
                if(tbxfolderName !=null)
                {
                    parameters.Add(IniData.FOLDER_NAME_KEY, tbxfolderName.Text);
                }
                var tbxfolderFormats = this.Controls.Find($"{SUBFOLDER_CONTROL_FORMATS}{i}", true)[0] as TextBox;
                if(tbxfolderFormats !=null)
                {
                    parameters.Add(IniData.FORMAT_KEY, tbxfolderFormats.Text);
                }

                SettingsModel model = new SettingsModel($"Folder{i}", parameters);
                models.Add(model);
            }

            if (!string.IsNullOrEmpty(btnFolder.Text))
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add(IniData.WATCHED_FOLDER_KEY, btnFolder.Text);
                models.Add(new SettingsModel(IniData.GLOBAL_SETTINGS, parameters));
            }
            IniData.Save(models);
            ServiceManager.Start();
        }
    }
}
