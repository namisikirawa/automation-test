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
                ExtentLogger.Info(_test, $"Tìm nhân viên cần sửa: {tenCanTim}");
                employeeForm.FindEmployee(tenCanTim, "");

                ExtentLogger.Info(_test,"Click chuột phải, chọn 'Sửa'");
                employeeForm.EditFirstEmployee(GlobalSetup.automation, GlobalSetup.app.ProcessId);

                ExtentLogger.Info(_test, "Mở form sửa nhân viên");
                var editForm = new EditEmployee_Form(GlobalSetup.mainWindow);

                ExtentLogger.Info(_test, $"Nhập thông tin mới:" +
                    $"");
                editForm.EnterEmployeeInfo(hoTen, gioiTinh, ngaySinh, sdt, diaChi, email, chucVu, gioLamViec, bangCap, knLamViec);

                ExtentLogger.Info(_test,"Nhấn nút 'Lưu'");
                var actualMessage = editForm.ClickSaveAndGetMessage(GlobalSetup.automation, GlobalSetup.app.ProcessId);
                ExtentLogger.Info(_test, $"Thông báo mong đợi: '{expectedMessage}'");
                ExtentLogger.Info(_test, $"Thực tế: '{actualMessage}'");

                if(expectedMessage != actualMessage)
                {
                    ExtentLogger.FailWithoutScreenshot(_test, "Test case fail: Thông báo không đúng như mong đợi");
                    Assert.Fail("Thông báo không đúng như mong đợi");
                }

                ExtentLogger.PassWithoutScreenshot(_test, "Thông báo đúng như mong đợi");
            }
            catch (Exception ex)
            {
                ExtentLogger.Fail(_test, $"Test case fail: {ex.Message}");
                Assert.Fail($"Sửa nhân viên thất bại: {ex.Message}");
            }
        }
    }
}
