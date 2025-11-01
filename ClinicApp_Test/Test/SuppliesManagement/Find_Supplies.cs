
namespace ClinicApp_Test.Test.SuppliesManagement
{
    [TestClass]
    public class FindEmployee
    {
        private const string appPath = @"D:\SEproject-1&2\ClinicManagement\GUI\bin\Debug\GUI.exe";
        private const string csvFile = @"D:\Đồ án tốt nghiệp\AutomationTest_Project\ClinicApp_Test\ClinicApp_Test\Data\find_supplies.csv";

        private static Application app;
        private static UIA3Automation automation;
        private static Window mainWindow;
        private static LoginForm loginForm;
        private static MainForm mainForm;
        private static SuppliesForm suppliesForm;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            app = Application.Launch(appPath);
            automation = new UIA3Automation();
            mainWindow = app.GetMainWindow(automation);

            // --- Đăng nhập ---
            loginForm = new LoginForm(mainWindow);
            loginForm.EnterUsername("phamquynh");
            loginForm.EnterPassword("123456");
            loginForm.ClickLogin();
            Thread.Sleep(2000);
            loginForm.CloseMessageBox(automation, app.ProcessId);
            Thread.Sleep(1000);

            // --- Điều hướng đến quản lý vật tư ---
            mainForm = new MainForm(mainWindow, automation);
            mainForm.OpenSuppliestManagement();
            Thread.Sleep(1000);

            // --- Gán lại cửa sổ chính & form nhân viên ---
            mainWindow = app.GetMainWindow(automation);
            suppliesForm = new SuppliesForm(mainWindow);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetData), DynamicDataSourceType.Method)]
        public void TestFindSupplies(string name, string provider, string[] expectedCellData)
        {
            // --- Thực hiện tìm kiếm ---
            suppliesForm.FindSupplies(name, provider);
            Thread.Sleep(1500);

            // --- Lấy dữ liệu sau khi tìm ---
            var allRows = suppliesForm.GetAllGridValues();

            // --- Kiểm tra có tất cả hàng đều chứa ít nhất 1 trong các dữ liệu mong đợi ---
            bool allMatch = allRows.All(row =>
                row.Any(cell =>
                    expectedCellData.Any(expected =>
                        cell.Contains(expected, StringComparison.OrdinalIgnoreCase))));

            Assert.IsTrue(allMatch,
                $"Có ít nhất một dòng không chứa bất kỳ dữ liệu mong đợi nào trong số: {string.Join(", ", expectedCellData)}");

        }

        public static IEnumerable<object[]> GetData()
        {
            using var reader = new StreamReader(csvFile, Encoding.UTF8);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            csv.Read();
            csv.ReadHeader();

            while (csv.Read())
            {
                var suppliesName = csv.GetField<string>("suppliesName");
                var provider = csv.GetField<string>("provider");
                var expectedRaw = csv.GetField<string>("expectedCellData") ?? string.Empty;
                var expectedCellData = expectedRaw.Split('|', StringSplitOptions.RemoveEmptyEntries)
                                                  .Select(s => s.Trim())
                                                  .ToArray();
                yield return new object[] { suppliesName, provider, expectedCellData };
            }
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
