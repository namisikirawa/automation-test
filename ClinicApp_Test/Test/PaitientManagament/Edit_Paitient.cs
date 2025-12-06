using ClinicApp_Test.Extent;
using ClinicApp_Test.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicApp_Test.Test.PaitientManagament
{
    [TestClass]
    public class Edit_Patient : BaseTest_Report
    {
        private static PatientForm patientForm;
        private const string data = @"D:\Đồ án tốt nghiệp\AutomationTest_Project\ClinicApp_Test\ClinicApp_Test\Data\edit_patient.csv";

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            var mainForm = GlobalSetup.mainForm;
            mainForm.OpenPaitientManagement();
            Thread.Sleep(1000);

            patientForm = new PatientForm(GlobalSetup.mainWindow);
        }

        public static IEnumerable<object[]> GetEditPatientData()
        {
            using var reader = new StreamReader(data, Encoding.UTF8);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

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
                    csv.GetField<string>("CCCD"),
                    csv.GetField<string>("email"),
                    csv.GetField<string>("chieuCao"),
                    csv.GetField<string>("canNang"),
                    csv.GetField<string>("diUng"),
                    csv.GetField<string>("nhomMau"),
                    csv.GetField<string>("tienSuBenhAn"),
                    csv.GetField<string>("ghiChu"),
                    csv.GetField<string>("expectedMessage")
                };
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(GetEditPatientData), DynamicDataSourceType.Method)]
        public void EditPatientTest(string testCaseName,
            string tenCanTim, string hoTen, string gioiTinh, string ngaySinh,
            string sdt, string diaChi, string cccd, string email,
            string chieuCao, string canNang, string diUng, string nhomMau,
            string tsBenhAn, string ghiChu, string expectedMessage)
        {
            var _test = _extent.CreateTest(testCaseName);
            _test.AssignCategory("Sửa bệnh nhân");
            try
            {
                ExtentLogger.Info(_test, $"Tìm bệnh nhân cần sửa: {tenCanTim}");
                patientForm.FindPatient(tenCanTim, "");
                patientForm.EditFirstPatient(GlobalSetup.automation, GlobalSetup.app.ProcessId);
                ExtentLogger.Info(_test, "Click chuột phải, chọn 'Sửa'");

                ExtentLogger.Info(_test, "Mở form sửa bệnh nhân");
                var editForm = new EditPatient_Form(GlobalSetup.mainWindow);

                ExtentLogger.Info(_test, "Nhập thông tin mới: " +
                    $"Họ tên: {hoTen}, " +
                    $"Giới tính: {gioiTinh}, " +
                    $"Ngày sinh: {ngaySinh}, " +
                    $"SĐT: {sdt}, " +
                    $"Địa chỉ: {diaChi}, " +
                    $"CCCD: {cccd}, " +
                    $"Email: {email}, " +
                    $"Chiều cao: {chieuCao}, " +
                    $"Cân nặng: {canNang}, " +
                    $"Dị ứng: {diUng}, " +
                    $"Nhóm máu: {nhomMau}, " +
                    $"Tiền sử bệnh án: {tsBenhAn}, " +
                    $"Ghi chú: {ghiChu}"
                );

                editForm.EnterPatientInfo(
                    hoTen, gioiTinh, ngaySinh, sdt, diaChi, cccd, email,
                    chieuCao, canNang, diUng, nhomMau, tsBenhAn, ghiChu
                );

                ExtentLogger.Info(_test, "Nhấn nút Lưu");
                var actualMsg = editForm.ClickSaveAndGetMessage(GlobalSetup.automation, GlobalSetup.app.ProcessId);
                ExtentLogger.Info(_test, $"Mong đợi: '{expectedMessage}'");
                ExtentLogger.Info(_test, $"Thực tế: '{actualMsg}'");

                if (expectedMessage != actualMsg)
                {
                    ExtentLogger.FailWithoutScreenshot(_test, "Test case fail: Thông báo không đúng như mong đợi");
                    Assert.Fail("Thông báo không đúng như mong đợi");
                }

                ExtentLogger.PassWithoutScreenshot(_test, "Thông báo đúng như mong đợi");
            }
            catch (Exception ex)
            {
                Assert.Fail("Lỗi trong quá trình test sửa bệnh nhân: " + ex.Message);
                ExtentLogger.Fail(_test, $"Test case fail: {ex.Message}");
            }
        }
    }
}
