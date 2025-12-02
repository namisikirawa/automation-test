using ClinicApp_Test.Extent;
using ClinicApp_Test.Form;
using System.Diagnostics;


namespace ClinicApp_Test.Test.Login
{
    [TestClass]
    public class Change_Password : BaseTest_Report
    {
        private static ChangePasswordForm changePasswordForm;
        private const string csvFile = @"D:\Đồ án tốt nghiệp\AutomationTest_Project\ClinicApp_Test\ClinicApp_Test\Data\change_password.csv";

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            var mainForm = GlobalSetup.mainForm;
            mainForm.OpenChangePasswordForm();
            Thread.Sleep(1000);

            changePasswordForm = new ChangePasswordForm(GlobalSetup.mainWindow,GlobalSetup.automation);
        }
        public static IEnumerable<object[]> GetChangePasswordData()
        {
            using var reader = new StreamReader(csvFile, Encoding.UTF8);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            csv.Read();
            csv.ReadHeader();

            while (csv.Read())
            {
                yield return new object[]
                {
                    csv.GetField<string>("testCaseName"),
                    csv.GetField<string>("oldpass"),
                    csv.GetField<string>("newpass"),
                    csv.GetField<string>("expectedMessage")
                };
            }
        }
        [DataTestMethod]
        [DynamicData(nameof(GetChangePasswordData), DynamicDataSourceType.Method)]
        public void TestChangePassword(string testCaseName,string oldPass, string newPass, string expectedMsg)
        {
            var _test = _extent.CreateTest(testCaseName);
            _test.AssignCategory("Đổi mật khẩu");
            try
            {
                ExtentLogger.info(_test, $"Nhập mật khẩu cũ: {oldPass}");
                ExtentLogger.info(_test, $"Nhập mật khẩu mới: {newPass}");
                changePasswordForm.EnterOldPassword(oldPass);
                changePasswordForm.EnterNewPassword(newPass);

                ExtentLogger.info(_test, "Nhấn nút Đổi mật khẩu");
                changePasswordForm.ClickChangePassword();

                var actualMsg = changePasswordForm.GetMessageBoxTextAndClose(GlobalSetup.app.ProcessId);
                ExtentLogger.info(_test, $"Thông báo mong đợi: '{expectedMsg}'");
                ExtentLogger.info(_test, $"Thực tế: '{actualMsg}'");

                Assert.AreEqual(expectedMsg, actualMsg, "Thông báo không khớp!");
                ExtentLogger.passHighlight(_test, "Test case pass: Thông báo chính xác");
            }
            catch (Exception ex)
            {
                Assert.Fail("Lỗi trong quá trình test đổi mật khẩu: " + ex.Message);
                ExtentLogger.failHighlight(_test, "Test case fail: Thông báo không khớp");
            }
        }
    }
}
