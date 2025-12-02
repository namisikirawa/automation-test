
using ClinicApp_Test.Extent;
using ClinicApp_Test.Forms;


namespace ClinicApp_Test.Test.EmployeeManagement
{

    [TestClass]
    public class Add_Employee : BaseTest_Report
    {
        private static EmployeeForm employeeForm;
        private const string data = @"D:\Đồ án tốt nghiệp\AutomationTest_Project\ClinicApp_Test\ClinicApp_Test\Data\add_employee.csv";

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            var mainForm = GlobalSetup.mainForm;
            mainForm.OpenEmployeeManagement();
            Thread.Sleep(1000);

            employeeForm = new EmployeeForm(GlobalSetup.mainWindow);
        }

        public static IEnumerable<object[]> GetAddEmployeeData()
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
                    csv.GetField<string>("hoTen"),
                    csv.GetField<string>("gioiTinh"),
                    csv.GetField<string>("ngaySinh"),
                    csv.GetField<string>("Sdt"),
                    csv.GetField<string>("diaChi"),
                    csv.GetField<string>("email"),
                    csv.GetField<string>("chucVu"),
                    csv.GetField<string>("gioLamViec"),
                    csv.GetField<string>("bangCap"),
                    csv.GetField<string>("knLamViec"),
                    csv.GetField<string>("expectedMessage")
                };
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(GetAddEmployeeData), DynamicDataSourceType.Method)]
        public void AddEmployeeTest(string testCaseName,string hoTen, string gioiTinh, string ngaySinh, string sdt,
                                    string diaChi, string email, string chucVu,
                                    string gioLamViec, string bangCap, string knLamViec,
                                    string expectedMessage)
        {
            var _test = _extent.CreateTest(testCaseName);
            _test.AssignCategory("Thêm nhân viên");
            try
            {
                ExtentLogger.info(_test, "Nhấn nút Thêm nhân viên");
                employeeForm.ClickAddButton();

                var addForm = new AddEmployee_Form(GlobalSetup.mainWindow);
                ExtentLogger.info(_test, $"Nhập thông tin nhân viên: " +
                    $"Họ tên: {hoTen}, " +
                    $"Giới tính: {gioiTinh}, " +
                    $"Ngày sinh: {ngaySinh}, " +
                    $"SĐT: {sdt}, " +
                    $"Địa chỉ: {diaChi}, " +
                    $"Email: {email}, " +
                    $"Chức vụ: {chucVu}, " +
                    $"Giờ làm việc: {gioLamViec}, " +
                    $"Bằng cấp: {bangCap}, " +
                    $"Kinh nghiệm: {knLamViec}"
                );
                addForm.EnterEmployeeInfo(hoTen, gioiTinh, ngaySinh, sdt, diaChi,email, chucVu, gioLamViec, bangCap, knLamViec);

                ExtentLogger.info(_test, "Nhấn nút Lưu");
                addForm.ClickSave(GlobalSetup.automation, GlobalSetup.app.ProcessId);
                Thread.Sleep(500);

                string actualMessage = addForm.GetMessageBoxText(GlobalSetup.automation, GlobalSetup.app.ProcessId);
                ExtentLogger.info(_test, $"Thông báo mong đợi: '{expectedMessage}'");
                ExtentLogger.info(_test, $"Thực tế: '{actualMessage}'");

                Assert.AreEqual(expectedMessage, actualMessage);
                ExtentLogger.passHighlight(_test, "Test case pass: Thông báo chính xác");
                addForm.CloseMessageBox(GlobalSetup.automation, GlobalSetup.app.ProcessId);
            }
            catch (Exception ex)
            {
                ExtentLogger.failHighlight(_test, $"Test case fail: Thông báo không khớp");
                Assert.Fail($"Lỗi: { ex.Message}");
            }
            finally
            {
                // luôn đóng form
                try
                {
                    var addForm = new AddEmployee_Form(GlobalSetup.mainWindow);
                    addForm.ClickCancel();
                    ExtentLogger.info(_test, "Đã đóng form thêm nhân viên");
                }
                catch (Exception ex)
                {
                    ExtentLogger.fail(_test, "Không thể đóng form thêm nhân viên");
                    Assert.Fail($"Lỗi: {ex.Message}");
                }
            }
        }
    }
}
