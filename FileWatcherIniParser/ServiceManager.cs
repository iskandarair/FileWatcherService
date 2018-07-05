using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcherIniParser
{
    public static class ServiceManager
    {
        private static readonly string SERVICE_EXE_NAME = "FileWatcherService.exe";
        private static readonly string SERVICE_NAME = "Folder Watcher Service";
        private static void InstallService()
        {
            System.Configuration.Install.AssemblyInstaller Installer = new System.Configuration.Install.AssemblyInstaller(SERVICE_EXE_NAME,new string[] { });
            Installer.UseNewContext = true;
            Installer.Install(null);
            Installer.Commit(null);
        }
        public static void UninstallService()
        {
            System.Configuration.Install.AssemblyInstaller Installer = new System.Configuration.Install.AssemblyInstaller(SERVICE_EXE_NAME, new string[] { });
            Installer.UseNewContext = true;
            Installer.Uninstall(null);
        }
        private static void StartService()
        {
            UninstallService();
            try
            {
                ServiceController service = new ServiceController(SERVICE_NAME);
                var status = service.Status;
            }
            catch (Exception ex)
            {
                InstallService();
                ServiceController service = new ServiceController(SERVICE_NAME);
                //here might be an error
                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running);
            }
            finally
            {
                ServiceController service = new ServiceController(SERVICE_NAME);
                if (service.Status == ServiceControllerStatus.Running)
                {
                    service.Stop();
                    service.WaitForStatus(ServiceControllerStatus.Stopped);
                    service.Start();
                }
            }
        }
        public static void Start()
        {
            StartService();
        }
    }
}
