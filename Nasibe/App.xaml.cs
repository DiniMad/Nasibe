using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Nasibe.Views;

namespace Nasibe
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            CheckForRunnigApplication();
            var MainWindow = new MainWindow();
            MainWindow.ShowDialog();
        }
        private static void CheckForRunnigApplication()
        {
            var currentProcesses = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);

            if (currentProcesses.Length <= 1) return;
            var windowRemove = new RemoveWindow
            {
                WindowTitle = "در حال اجرا",
                Caption = "برنامه در حال اجرا است. ",
                InformationIcon = true,
                OneBtn = true,
                Btn2 = "باشه"
            };
            windowRemove.ShowDialog();
            Environment.Exit(0);
        }
    }
}
