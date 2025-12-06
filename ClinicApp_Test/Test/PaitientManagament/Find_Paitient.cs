

using ClinicApp_Test.Extent;
using ClinicApp_Test.Form;
using System.Diagnostics;

namespace ClinicApp_Test.Test.PaitientManagament
{
    [TestClass]
    public class Find_Patient : BaseTest_Report
    {
        private static PatientForm patientForm;
        private const string csvFile =
            @"D:\Đồ án tốt nghiệp\AutomationTest_Project\ClinicApp_Test\ClinicApp_Test\Data\find_patient.csv";

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            // Dùng window & mainForm từ GlobalSetup
            var mainForm = GlobalSetup.mainForm;
            mainForm.OpenPaitientManagement();
            Thread.Sleep(1000);

            patientForm = new PatientForm(GlobalSetup.mainWindow);
        }

        public static IEnumerable<object[]> GetData()
        {
            using var reader = new StreamReader(csvFile, Encoding.UTF8);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

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

                yield return new object[] { testCaseName ,name, phoneNumber, expectedCellData };
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(GetData), DynamicDataSourceType.Method)]
        public void TestFindPatient(string testCaseName, string name, string phoneNumber, string[] expectedCellData)
        {
            var _test = _extent.CreateTest(testCaseName);
            _test.AssignCategory("Tìm kiếm bệnh nhân");
            try
            {
                ExtentLogger.Info(_test, $"Nhập vào ô tìm kiếm: Tên = '{name}', SĐT = '{phoneNumber}'");
                ExtentLogger.Info(_test, $"Nhấn nút Tìm kiếm");
                patientForm.FindPatient(name, phoneNumber);
                Thread.Sleep(1200);

                ExtentLogger.Info(_test, $"Dữ liệu mong đợi: {string.Join(", ", expectedCellData)}");
                ExtentLogger.Info(_test, "Dữ liệu thực tế:");
                var check = patientForm.VerifySearchResults(expectedCellData);

                var allRows = patientForm.GetAllGridValues();
                foreach (var row in allRows)
                    ExtentLogger.Info(_test, string.Join(" | ", row));

                Assert.IsTrue(check,$"Có dòng không chứa dữ liệu mong đợi: {string.Join(", ", expectedCellData)}");
                ExtentLogger.Pass(_test, "Test case pass");
            }
            catch (Exception ex)
            {
                Assert.Fail($"Lỗi trong quá trình test tìm bệnh nhân với tên '{name}' và số điện thoại '{phoneNumber}': {ex.Message}");
                ExtentLogger.Fail(_test, $"Test case fail: {ex.Message}");
            }
        }
    }
}
