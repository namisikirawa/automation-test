

namespace ClinicApp_Test.Test.PaitientManagament
{
    [TestClass]
    public class DeletePatient
    {
        private const string appPath = @"D:\SEproject-1&2\ClinicManagement\GUI\bin\Debug\GUI.exe";

        private static Application app;
        private static UIA3Automation automation;
        private static Window mainWindow;
        private static LoginForm loginForm;
        private static MainForm mainForm;
        private static PatientForm patientForm;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            app = Application.Launch(appPath);
            automation = new UIA3Automation();
            mainWindow = app.GetMainWindow(automation);

            // Đăng nhập
            loginForm = new LoginForm(mainWindow);
            loginForm.EnterUsername("phamquynh");
            loginForm.EnterPassword("123456");
            loginForm.ClickLogin();
            Thread.Sleep(2000);
            loginForm.CloseMessageBox(automation, app.ProcessId);
            Thread.Sleep(1000);

            // Điều hướng tới form bệnh nhân
            mainForm = new MainForm(mainWindow, automation);
            mainForm.OpenPaitientManagement();
            Thread.Sleep(1000);

            // Gán lại cửa sổ chính sau khi load form
            mainWindow = app.GetMainWindow(automation);
            patientForm = new PatientForm(mainWindow);
        }

        [TestMethod]
        public void TestDeletePatient()
        {
            var grid = patientForm.PatientGrid;
            int initialRowCount = grid.Rows.Length;

            patientForm.DeleteFirstPatient(automation, app.ProcessId);

            grid = patientForm.PatientGrid;
            int rowCountAfterDelete = grid.Rows.Length;

            Console.WriteLine($"Số dòng sau khi xóa: {rowCountAfterDelete}");
            Assert.AreEqual(initialRowCount - 1, rowCountAfterDelete, "Số dòng không giảm sau khi xóa!");
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            if (app != null && !app.HasExited)
            {
                app.Close();

                var desktop = automation.GetDesktop();
                var dialog = desktop.FindFirstChild(cf => cf.ByProcessId(app.ProcessId)
                                                           .And(cf.ByControlType(ControlType.Window)))?.AsWindow();
                var yesButton = dialog?.FindFirstDescendant(cf => cf.ByText("Yes"))?.AsButton();
                yesButton?.Click();
            }
            automation?.Dispose();
        }
    }
}
