
using ClinicApp_Test.Extent;
using ClinicApp_Test.Form;
using FlaUI.Core.AutomationElements;

namespace ClinicApp_Test.Test.Login
{
    [TestClass]
    public class Login : BaseTest_Report
    {
        private static LoginForm loginForm;
        private const string data = @"D:\Đồ án tốt nghiệp\AutomationTest_Project\ClinicApp_Test\ClinicApp_Test\Data\login.csv";

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            var window = GlobalSetup.mainWindow;
            MainForm mainForm = GlobalSetup.mainForm;
            mainForm.Logout(GlobalSetup.automation, GlobalSetup.app.ProcessId);

            window = GlobalSetup.app.GetMainWindow(GlobalSetup.automation);
            Thread.Sleep(500);
            // Khởi tạo LoginForm để sử dụng trong test
            loginForm = new LoginForm(window);
        }

        public static IEnumerable<object[]> GetLoginData()
        {
            using var reader = new StreamReader(data, Encoding.UTF8);
            using var csv = new CsvHelper.CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);
            csv.Read();
            csv.ReadHeader();

            while (csv.Read())
            {
                yield return new object[]
                {
                    csv.GetField<string>("testCaseName"),
                    csv.GetField<string>("username"),
                    csv.GetField<string>("password"),
                    csv.GetField<string>("expectedMessage")
                };
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(GetLoginData), DynamicDataSourceType.Method)]
        public void TestLogin(string testCaseName, string username, string password, string expectedMessage)
        {
            var _test = _extent.CreateTest(testCaseName);
            _test.AssignCategory("Đăng nhập");
            try
            {
                ExtentLogger.Info(_test, $"Nhập tài khoản: {username}");
                ExtentLogger.Info(_test, $"Nhập mật khẩu: {password}");
                loginForm.UsernameTextBox.Text = string.Empty;
                loginForm.PasswordTextBox.Text = string.Empty;
                Thread.Sleep(300);
                loginForm.EnterUsername(username);
                loginForm.EnterPassword(password);

                ExtentLogger.Info(_test, "Nhấn nút Đăng nhập");
                loginForm.ClickLogin();
                Thread.Sleep(1000);

                string actualMessage = loginForm.GetMessageBoxText(GlobalSetup.automation, GlobalSetup.app.ProcessId);
                ExtentLogger.Info(_test, $"Thông báo mong đợi: {expectedMessage}");
                ExtentLogger.Info(_test, $"Thực tế: {actualMessage}");

                if(expectedMessage != actualMessage)
                {
                    ExtentLogger.Fail(_test, "Test case fail: Thông báo không khớp");
                    Assert.Fail("Thông báo không khớp");
                }
                ExtentLogger.Pass(_test, "Thông báo trùng khớp");
                loginForm.CloseMessageBox(GlobalSetup.automation, GlobalSetup.app.ProcessId);

                if (actualMessage == "Chúc mừng bạn đã đăng nhập thành công!")
                {
                    loginForm.LogoutIfLoggedIn(GlobalSetup.automation, GlobalSetup.app);
                    // Sau khi đăng xuất, khởi tạo lại LoginForm
                    var window = GlobalSetup.app.GetMainWindow(GlobalSetup.automation);
                    loginForm = new LoginForm(window);
                    ExtentLogger.Info(_test, "Đã đăng xuất");
                    Thread.Sleep(1000);
                }
                ExtentLogger.PassWithoutScreenshot(_test, "Test case pass");
                Thread.Sleep(500);
            }
            catch (AssertFailedException ex)
            {
                ExtentLogger.Fail(_test, "Test case fail: Kết quả không khớp với mong đợi");
                ExtentLogger.Fail(_test, $"Test case fail: {ex.Message}");
            }
        }
        [ClassCleanup(ClassCleanupBehavior.EndOfClass)]
        public static void Cleanup()
        {
            // Đăng nhập lại để giữ trạng thái global
            var loginWindow = GlobalSetup.app.GetMainWindow(GlobalSetup.automation);
            var loginForm = new LoginForm(loginWindow);
            loginForm.EnterUsername("admin");
            loginForm.EnterPassword("123456");
            loginForm.ClickLogin();
            Thread.Sleep(1000);
            loginForm.CloseMessageBox(GlobalSetup.automation, GlobalSetup.app.ProcessId);
            Thread.Sleep(2000);
            // Cập nhật lại MainForm cục bộ
            GlobalSetup.mainWindow = GlobalSetup.app.GetMainWindow(GlobalSetup.automation);
            GlobalSetup.mainForm = new MainForm(GlobalSetup.mainWindow, GlobalSetup.automation);
            Thread.Sleep(2000);
        }
    }
}
