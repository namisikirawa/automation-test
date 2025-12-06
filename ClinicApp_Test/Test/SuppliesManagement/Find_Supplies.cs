
using ClinicApp_Test.Extent;
using System.Diagnostics;

namespace ClinicApp_Test.Test.SuppliesManagement
{
    [TestClass]
    public class Find_Supplies : BaseTest_Report
    {
        private static SuppliesForm suppliesForm;
        private const string csvFile =
            @"D:\Đồ án tốt nghiệp\AutomationTest_Project\ClinicApp_Test\ClinicApp_Test\Data\find_supplies.csv";

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            // Dùng mainForm từ GlobalSetup
            var mainForm = GlobalSetup.mainForm;
            mainForm.OpenSuppliesManagement();
            Thread.Sleep(1000);

            suppliesForm = new SuppliesForm(GlobalSetup.mainWindow);
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
                var suppliesName = csv.GetField<string>("suppliesName");
                var provider = csv.GetField<string>("provider");

                var expectedRaw = csv.GetField<string>("expectedCellData") ?? string.Empty;
                var expectedCellData = expectedRaw.Split('|', StringSplitOptions.RemoveEmptyEntries)
                                                  .Select(s => s.Trim())
                                                  .ToArray();

                yield return new object[] { testCaseName, suppliesName, provider, expectedCellData };
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(GetData), DynamicDataSourceType.Method)]
        public void TestFindSupplies(string testCaseName, string name, string provider, string[] expectedCellData)
        {
            var _test = _extent.CreateTest(testCaseName);
            _test.AssignCategory("Tìm kiếm vật tư");
            try
            {
                ExtentLogger.Info(_test, $"Nhập vào ô tìm kiếm: Tên = '{name}', Nhà cung cấp = '{provider}'");
                ExtentLogger.Info(_test, $"Nhấn nút Tìm kiếm");
                suppliesForm.FindSupplies(name, provider);
                Thread.Sleep(1200);

                ExtentLogger.Info(_test, $"Dữ liệu mong đợi: {string.Join(", ", expectedCellData)}");
                ExtentLogger.Info(_test, "Dữ liệu thực tế:");
                var check = suppliesForm.VerifySearchResults(expectedCellData);

                var allRows = suppliesForm.GetAllGridValues();
                foreach (var row in allRows)
                    ExtentLogger.Info(_test, string.Join(" | ", row));

                Assert.IsTrue(check, $"Có dòng không chứa dữ liệu mong đợi: {string.Join(", ", expectedCellData)}");
                ExtentLogger.Pass(_test, "Test case pass");
            }
            catch (Exception ex)
            {
                ExtentLogger.Fail(_test, $"Test case fail: {ex.Message}");
                Assert.Fail($"TestFindSupplies failed with exception: {ex.Message}");
            }
        }
    }
}
