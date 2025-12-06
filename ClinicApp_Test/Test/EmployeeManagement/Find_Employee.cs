

using ClinicApp_Test.Extent;
using ClinicApp_Test.Form;
using System.Diagnostics;

namespace ClinicApp_Test.Test.EmployeeManagement
{

    [TestClass]
    public class Find_Employee : BaseTest_Report
    {
        private static EmployeeForm employeeForm;
        private const string csvFile = @"D:\Đồ án tốt nghiệp\AutomationTest_Project\ClinicApp_Test\ClinicApp_Test\Data\find_employee.csv";

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            var mainForm = GlobalSetup.mainForm;
            mainForm.OpenEmployeeManagement();
            Thread.Sleep(1000);

            employeeForm = new EmployeeForm(GlobalSetup.mainWindow);
        }

        public static IEnumerable<object[]> GetData()
        {
            using var reader = new StreamReader(csvFile, Encoding.UTF8);
            using var csv = new CsvHelper.CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);

            csv.Read();
            csv.ReadHeader();

            while (csv.Read())
            {
                var testCaseName = csv.GetField<string>("testCaseName");
                var name = csv.GetField<string>("name");
                var phoneNumber = csv.GetField<string>("phoneNumber");
                var expectedRaw = csv.GetField<string>("expectedCellData") ?? string.Empty;
                var expectedCellData = expectedRaw.Split('|', StringSplitOptions.RemoveEmptyEntries)
                                                  .Select(s => s.Trim())
                                                  .ToArray();
                yield return new object[] { testCaseName, name, phoneNumber, expectedCellData };
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(GetData), DynamicDataSourceType.Method)]
         public void TestFindEmployee(string testCaseName, string name, string phoneNumber, string[] expectedCellData)
        {
            var _test = _extent.CreateTest(testCaseName);
            _test.AssignCategory("Tìm kiếm nhân viên");
            try
            {
                ExtentLogger.Info(_test, $"Nhập vào ô tìm kiếm: Tên = '{name}'. Số điện thoại: {phoneNumber}");
                ExtentLogger.Info(_test, $"Nhấn nút Tìm kiếm");
                employeeForm.FindEmployee(name, phoneNumber);
                Thread.Sleep(1000);

                ExtentLogger.Info(_test, $"Dữ liệu mong đợi: {string.Join(", ", expectedCellData)}");
                ExtentLogger.Info(_test, "Dữ liệu thực tế:");
                var check = employeeForm.VerifySearchResults(expectedCellData);

                var allRows = employeeForm.GetAllGridValues();
                foreach (var row in allRows)
                    ExtentLogger.Info(_test, string.Join(" | ", row));

                Assert.IsTrue(check, $"Có ít nhất một dòng không chứa dữ liệu mong đợi: {string.Join(", ", expectedCellData)}");
                ExtentLogger.Pass(_test, "Test case pass");
            }
            catch (Exception ex)
            {
                Assert.Fail($"Lỗi trong quá trình tìm kiếm nhân viên: {ex.Message}");
                ExtentLogger.Fail(_test, $"Test case fail: {ex.Message}");
            }
        }
    }
}
