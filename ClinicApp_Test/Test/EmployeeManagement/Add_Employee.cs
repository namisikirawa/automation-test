
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
                //số nhân viên trước khi thêm
                int beforeCount = employeeForm.GetRowCount();

                ExtentLogger.Info(_test, "Nhấn nút 'Thêm mới'");
                employeeForm.ClickAddButton();

                var addForm = new AddEmployee_Form(GlobalSetup.mainWindow);
                ExtentLogger.Info(_test, $"Nhập thông tin nhân viên: " +
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

                ExtentLogger.Info(_test, "Nhấn nút Lưu");
                addForm.ClickSave(GlobalSetup.automation, GlobalSetup.app.ProcessId);
                Thread.Sleep(500);

                string actualMessage = addForm.GetMessageBoxText(GlobalSetup.automation, GlobalSetup.app.ProcessId);

                ExtentLogger.Info(_test, $"Thông báo mong đợi: '{expectedMessage}'");
                ExtentLogger.Info(_test, $"Thực tế: '{actualMessage}'");
                if(expectedMessage!= actualMessage) {
                    ExtentLogger.Fail(_test, "Test case fail: Thông báo không khớp!");
                    addForm.CloseMessageBox(GlobalSetup.automation, GlobalSetup.app.ProcessId);
                    Assert.Fail("Message box text mismatch!");
                }

                //pass phần kiểm tra hộp thoại: screenshot+ thông báo
                ExtentLogger.Pass(_test, "Thông báo trùng khớp");
                addForm.CloseMessageBox(GlobalSetup.automation, GlobalSetup.app.ProcessId);

                if (expectedMessage=="Thêm nhân viên thành công!")
                {
                    //kiểm tra số nhân viên có tăng thêm 1 không
                    int afterCount = employeeForm.GetRowCount();
                    ExtentLogger.Info(_test, $"Số nhân viên trước khi thêm: {beforeCount}");
                    ExtentLogger.Info(_test, $"Số nhân viên sau khi thêm: {afterCount}");

                    if (afterCount != beforeCount + 1)
                    {
                        ExtentLogger.Error(_test, "Số lượng nhân viên không tăng thêm 1 sau khi thêm!");
                        Assert.Fail("Row count mismatch!");
                    }
                }

                ExtentLogger.PassWithoutScreenshot(_test, "Test case passed");
            }
            catch (Exception ex)
            {
                ExtentLogger.Fail(_test, $"Test case fail: {ex.Message}");
                Assert.Fail(ex.Message);
            }
            finally
            {
                // luôn đóng form
                try
                {
                    var addForm = new AddEmployee_Form(GlobalSetup.mainWindow);
                    addForm.ClickCancel();
                    ExtentLogger.Info(_test, "Đã đóng form thêm nhân viên");
                }
                catch (Exception ex)
                {
                    ExtentLogger.Error(_test, "Lỗi khi đóng form thêm nhân viên");
                    Assert.Fail($"Lỗi: {ex.Message}");
                }
            }
        }
    }
}
