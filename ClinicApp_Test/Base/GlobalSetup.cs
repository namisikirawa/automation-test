using ClinicApp_Test.Form;
using FlaUI.Core.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicApp_Test.Base
{
    //chỉ mở và đóng ứng dụng một lần cho toàn bộ các test class
    [TestClass]
    public class GlobalSetup
    {
        public static Application app;
        public static UIA3Automation automation;
        public static Window mainWindow;
        public static MainForm mainForm;
        
        private const string appPath = @"D:\SEproject-1&2\ClinicManagement\GUI\bin\Debug\GUI.exe";

        [AssemblyInitialize]
        public static void Init(TestContext context)
        {
            Console.WriteLine("GLOBAL INIT");

            app = Application.Launch(appPath);
            automation = new UIA3Automation();
            mainWindow = app.GetMainWindow(automation);
            Thread.Sleep(1500);

            var loginForm = new LoginForm(mainWindow);
            loginForm.EnterUsername("admin");
            loginForm.EnterPassword("123456");
            loginForm.ClickLogin();
            Thread.Sleep(1500);
            loginForm.CloseMessageBox(automation, app.ProcessId);

            mainForm = new MainForm(mainWindow, automation);
            Thread.Sleep(1000);
            mainWindow = app.GetMainWindow(automation);

            Console.WriteLine("GLOBAL INIT DONE: Logged in and ready");
        }
        [AssemblyCleanup]
        public static void Cleanup()
        {
            Console.WriteLine("GLOBAL CLEANUP");

            automation?.Dispose();
            if (app != null && !app.HasExited)
            {
                app.Close();
            }

            // Kill zombie
            foreach (var p in Process.GetProcessesByName("GUI"))
            {
                p.Kill();
            }
        }
    }
}
