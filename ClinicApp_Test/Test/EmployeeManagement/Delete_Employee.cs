
namespace ClinicApp_Test.Test.EmployeeManagement
{
    [TestClass]
    public class DeleteEmployee
    {
        private const string appPath = @"D:\SEproject-1&2\ClinicManagement\GUI\bin\Debug\GUI.exe";

        private static Application app;
        private static UIA3Automation automation;
        private static Window mainWindow;
        private static LoginForm loginForm;
        private static MainForm mainForm;
        private static EmployeeForm employeeForm;

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

            // Điều hướng sang form nhân viên qua MainForm
            mainForm = new MainForm(mainWindow, automation);
            mainForm.OpenEmployeeManagement();
            Thread.Sleep(1000);

            // Lấy window hiện tại để thao tác
            mainWindow = app.GetMainWindow(automation);
            employeeForm = new EmployeeForm(mainWindow);
        }

        [TestMethod]
        public void TestDeleteEmployee()
        {
            var grid = employeeForm.EmployeeGrid;
            int initialRowCount = grid.Rows.Length;

            employeeForm.DeleteFirstEmployee(automation, app.ProcessId);

            grid = employeeForm.EmployeeGrid;
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
