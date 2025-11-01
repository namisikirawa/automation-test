

namespace ClinicApp_Test.Test.PaitientManagament
{
    [TestClass]
    public class FindPatient
    {
        private const string appPath = @"D:\SEproject-1&2\ClinicManagement\GUI\bin\Debug\GUI.exe";
        private const string csvFile = @"D:\Đồ án tốt nghiệp\AutomationTest_Project\ClinicApp_Test\ClinicApp_Test\Data\find_patient.csv";

        private static Application app;
        private static UIA3Automation automation;
        private static Window mainWindow;
        private static LoginForm loginForm;
        private static MainForm mainForm;
        private static PatientForm patientForm;

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

            // --- Điều hướng đến quản lý bệnh nhân ---
            mainForm = new MainForm(mainWindow, automation);
            mainForm.OpenPaitientManagement();
            Thread.Sleep(1000);

            // --- Gán lại cửa sổ chính & form bệnh nhân ---
            mainWindow = app.GetMainWindow(automation);
            patientForm = new PatientForm(mainWindow);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetData), DynamicDataSourceType.Method)]
        public void TestFindEmployee(string name, string phoneNumber, string[] expectedCellData)
        {
            // --- Thực hiện tìm kiếm ---
            patientForm.FindPatient(name, phoneNumber);
            Thread.Sleep(1500);

            // --- Lấy dữ liệu sau khi tìm ---
            var allRows = patientForm.GetAllGridValues();

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
                var name = csv.GetField<string>("name");
                var phoneNumber = csv.GetField<string>("phoneNumber");
                var expectedRaw = csv.GetField<string>("expectedCellData") ?? string.Empty;
                var expectedCellData = expectedRaw.Split('|', StringSplitOptions.RemoveEmptyEntries)
                                                  .Select(s => s.Trim())
                                                  .ToArray();
                yield return new object[] { name, phoneNumber, expectedCellData };
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
