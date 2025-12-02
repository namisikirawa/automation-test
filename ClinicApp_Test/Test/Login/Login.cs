
using ClinicApp_Test.Extent;
using ClinicApp_Test.Form;

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

            ExtentLogger.info(_test, $"Nhập Username: {username}, Password: {password}");
            loginForm.UsernameTextBox.Text = string.Empty;
            loginForm.PasswordTextBox.Text = string.Empty;
            Thread.Sleep(300);

            loginForm.EnterUsername(username);
            loginForm.EnterPassword(password);
            ExtentLogger.info(_test, "Nhấn nút Đăng nhập");
            loginForm.ClickLogin();
            Thread.Sleep(1000);

            string actualMessage = loginForm.GetMessageBoxText(GlobalSetup.automation, GlobalSetup.app.ProcessId);
            ExtentLogger.info(_test, $"Thông báo mong đợi: {expectedMessage}");
            ExtentLogger.info(_test, $"Thực tế: {actualMessage}");
            try
            {
                Assert.AreEqual(expectedMessage, actualMessage);
                ExtentLogger.passHighlight(_test, "Test case pass: Kết quả khớp với mong đợi");
            }
            catch (AssertFailedException ex)
            {
                ExtentLogger.failHighlight(_test, "Test case fail: Kết quả không khớp với mong đợi");
                throw ex;
            }
            finally
            {
                loginForm.CloseMessageBox(GlobalSetup.automation, GlobalSetup.app.ProcessId);
                Thread.Sleep(500);
            }

            if (expectedMessage == "Chúc mừng bạn đã đăng nhập thành công!")
            {
                ExtentLogger.info(_test, "Đăng xuất sau khi đăng nhập thành công để trả về trạng thái ban đầu");
                loginForm.LogoutIfLoggedIn(GlobalSetup.automation, GlobalSetup.app);
                Thread.Sleep(1000);
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
