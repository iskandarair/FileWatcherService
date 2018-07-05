using System.ServiceProcess;
using System.Threading.Tasks;
using System.Timers;

namespace FileWatcherService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //FileWatcher();
            //StartFileWatcherTimer();
        }
        public void FileWatcher()
        {
            FolderWatcher.Initialize();
            FolderWatcher.MoveFiles();
        }
        public void StartFileWatcherTimer()
        {
            var period = 1000 * 60 * 60  * 2; //two hours
            var timer = new Timer(period);
            timer.Elapsed +=  (sender, e) =>  FileWatcher();
            timer.Start();
        }
        protected override void OnStop()
        {
        }
    }
}
