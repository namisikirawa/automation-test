using ClinicApp_Test.Extent;
using ClinicApp_Test.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicApp_Test.Test.EmployeeManagement
{
    [TestClass]
    public class Edit_Employee : BaseTest_Report
    {
        private static EmployeeForm employeeForm;
        private const string data = @"D:\Đồ án tốt nghiệp\AutomationTest_Project\ClinicApp_Test\ClinicApp_Test\Data\edit_employee.csv";

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            var mainForm = GlobalSetup.mainForm;
            mainForm.OpenEmployeeManagement();
            Thread.Sleep(1000);

            employeeForm = new EmployeeForm(GlobalSetup.mainWindow);
        }

        public static IEnumerable<object[]> GetEditEmployeeData()
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
                    csv.GetField<string>("tenCanTim"),
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
        [DynamicData(nameof(GetEditEmployeeData), DynamicDataSourceType.Method)]
        public void EditEmployeeTest(string testCaseName, string tenCanTim, string hoTen, string gioiTinh, string ngaySinh, string sdt,
                                     string diaChi, string email, string chucVu, string gioLamViec,
                                     string bangCap, string knLamViec, string expectedMessage)
        {
            var _test = _extent.CreateTest(testCaseName);
            _test.AssignCategory("Sửa nhân viên");
            try
            {
                ExtentLogger.info(_test, $"Tìm nhân viên cần sửa: {tenCanTim}");
                employeeForm.FindEmployee(tenCanTim, "");
                ExtentLogger.info(_test,"Click chuột phải, chọn 'Sửa'");
                employeeForm.EditFirstEmployee(GlobalSetup.automation, GlobalSetup.app.ProcessId);

                ExtentLogger.info(_test, "Mở form sửa nhân viên");
                var editForm = new EditEmployee_Form(GlobalSetup.mainWindow);

                ExtentLogger.info(_test, $"Nhập thông tin mới: " +
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
                editForm.EnterEmployeeInfo(hoTen, gioiTinh, ngaySinh, sdt, diaChi, email, chucVu, gioLamViec, bangCap, knLamViec);

                ExtentLogger.info(_test, "Nhấn nút Lưu");
                var actualMessage = editForm.ClickSaveAndGetMessage(GlobalSetup.automation, GlobalSetup.app.ProcessId);
                ExtentLogger.info(_test, $"Thông báo mong đợi: '{expectedMessage}'");
                ExtentLogger.info(_test, $"Thực tế: '{actualMessage}'");

                Assert.AreEqual(expectedMessage, actualMessage, "Messagebox không đúng như mong đợi");
                ExtentLogger.passHighlight(_test, "Test case pass: Thông báo chính xác");
            }
            catch (Exception ex)
            {
                ExtentLogger.failHighlight(_test, $"Test case fail: Thông báo không khớp");
                Assert.Fail($"Lỗi trong quá trình sửa nhân viên: {ex.Message}");
            }
        }
    }
}
