
namespace ClinicApp_Test.Test.Login
{
    [TestClass]
    public class Login
    {
        private const string appPath= @"D:\SEproject-1&2\ClinicManagement\GUI\bin\Debug\GUI.exe";
        private const string data = @"D:\\Đồ án tốt nghiệp\\AutomationTest_Project\\ClinicApp_Test\\ClinicApp_Test\\Data\\login.csv";

        private static Application app;
        private static UIA3Automation automation;
        private static Window window;
        private static LoginForm loginForm;

        [ClassInitialize]//khởi tạo TestClass để chạy tất cả testcase
        public static void Setup(TestContext context)
        {
            app = Application.Launch(appPath);
            automation = new UIA3Automation();
            window = app.GetMainWindow(automation);

            // Đợi giao diện khởi động ổn định
            Thread.Sleep(1500);

            loginForm = new LoginForm(window);
        }
        //đọc dữ liệu từ file csv
        public static IEnumerable<object[]> GetLoginData()
        {
            using var reader = new StreamReader(data, Encoding.UTF8);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            csv.Read();
            csv.ReadHeader();

            while (csv.Read())
            {
                var username = csv.GetField<string>("username");
                var password = csv.GetField<string>("password");
                var expectedMessage = csv.GetField<string>("expectedMessage");
                yield return new object[] { username, password, expectedMessage };
            }
        }
        //thực hiện các thao tác test
        [DataTestMethod,TestCategory("Login")]
        [DynamicData(nameof(GetLoginData), DynamicDataSourceType.Method)]
        public void TestLogin(string username, string password, string expectedMessage)
        {
            // Làm mới textbox trước khi nhập
            loginForm.UsernameTextBox.Text = string.Empty;
            loginForm.PasswordTextBox.Text = string.Empty;
            Thread.Sleep(300);

            loginForm.EnterUsername(username);
            loginForm.EnterPassword(password);
            loginForm.ClickLogin();

            Thread.Sleep(1500);

            string actualMessage = loginForm.GetMessageBoxText(automation, app.ProcessId);
            Console.WriteLine("MessageBox content: " + actualMessage);

            try
            {
                Assert.AreEqual(expectedMessage, actualMessage, "Nội dung MessageBox không đúng");
            }
            finally
            {
                loginForm.CloseMessageBox(automation, app.ProcessId);
                Thread.Sleep(500);
            }

            // Nếu đăng nhập thành công → Đăng xuất
            if (expectedMessage == "Chúc mừng bạn đã đăng nhập thành công!")
            {
                loginForm.LogoutIfLoggedIn(automation, app);
                Thread.Sleep(1000);

                var mainWindow = app.GetAllTopLevelWindows(automation)
                                    .FirstOrDefault(w => w.Title == "Đăng nhập");

                Assert.IsNotNull(mainWindow, "Không tìm thấy form 'Đăng nhập' sau khi đăng xuất");
                Console.WriteLine("Logout thành công, quay về form đăng nhập.");

                //lấy lại form login mới sau khi đăng xuất (form login cũ đã bị đóng và trở thành null)
                window = app.GetMainWindow(automation);
                loginForm = new LoginForm(window);
            }
        }
        //dọn dẹp bộ nhớ sau khi test
        [ClassCleanup]
        public static void Teardown()
        {
            if (app != null && !app.HasExited)
            {
                app.Close();
                Console.WriteLine("Application closed after all tests.");
            }

            automation?.Dispose();
        }
    }

}
