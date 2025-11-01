using ClinicApp_Test.Form;


namespace ClinicApp_Test.Test.Login
{
    [TestClass]
    public class Change_Password
    {
        private const string appPath = @"D:\SEproject-1&2\ClinicManagement\GUI\bin\Debug\GUI.exe";
        private const string csvFile = @"D:\Đồ án tốt nghiệp\AutomationTest_Project\ClinicApp_Test\ClinicApp_Test\Data\change_password.csv";

        private static Application app;
        private static UIA3Automation automation;
        private static Window mainWindow;
        private static LoginForm loginForm;
        private static ChangePasswordForm changePasswordForm;

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
            Thread.Sleep(2000);

            // Mở màn hình đổi mật khẩu
            mainWindow = app.GetMainWindow(automation);
            var mainForm = new MainForm(mainWindow, automation);
            mainForm.OpenChangePasswordForm();

            changePasswordForm = new ChangePasswordForm(mainWindow, automation);
        }

        

        public static IEnumerable<object[]> GetData()
        {
            using var reader = new StreamReader(csvFile, Encoding.UTF8);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            csv.Read();
            csv.ReadHeader();

            while (csv.Read())
            {
                yield return new object[]
                {
                    csv.GetField<string>("oldpass"),
                    csv.GetField<string>("newpass"),
                    csv.GetField<string>("expectedMessage")
                };
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(GetData), DynamicDataSourceType.Method)]
        public void TestChangePassword(string oldPass, string newPass, string expectedMsg)
        {
            changePasswordForm.EnterOldPassword(oldPass);
            changePasswordForm.EnterNewPassword(newPass);
            changePasswordForm.ClickChangePassword();

            var actualMsg = changePasswordForm.GetMessageBoxTextAndClose(app.ProcessId);
            Console.WriteLine("MessageBox: " + actualMsg);

            Assert.AreEqual(expectedMsg, actualMsg, "Thông báo không khớp!");
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            if (app != null && !app.HasExited)
            {
                app.Close();

                var desktop = automation.GetDesktop();
                var dialog = desktop.FindFirstChild(cf => cf.ByProcessId(app.ProcessId)
                                                           .And(cf.ByControlType(ControlType.Window)))
                                    ?.AsWindow();
                dialog?.FindFirstDescendant(cf => cf.ByText("Yes"))?.AsButton()?.Click();
            }
            automation?.Dispose();
        }
    }
}
