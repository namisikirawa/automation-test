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
    public class Add_Patient : BaseTest_Report
    {
        private static PatientForm patientForm;
        private const string data = @"D:\Đồ án tốt nghiệp\AutomationTest_Project\ClinicApp_Test\ClinicApp_Test\Data\add_patient.csv";

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            var mainForm = GlobalSetup.mainForm;
            mainForm.OpenPaitientManagement();
            Thread.Sleep(800);

            patientForm = new PatientForm(GlobalSetup.mainWindow);
        }

        public static IEnumerable<object[]> GetAddPatientData()
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
        [DynamicData(nameof(GetAddPatientData), DynamicDataSourceType.Method)]
        public void AddPatientTest(string testCaseName,
            string hoTen, string gioiTinh, string ngaySinh, string sdt,
            string diaChi, string cccd, string email, string chieuCao,
            string canNang, string diUng, string nhomMau,
            string tsBenhAn, string ghiChu, string expectedMessage)
        {
            var _test = _extent.CreateTest(testCaseName);
            _test.AssignCategory("Thêm bệnh nhân");
            try
            {
                ExtentLogger.info(_test, "Nhấn nút Thêm bệnh nhân");
                patientForm.ClickAddButton();
                Thread.Sleep(500);

                var addForm = new AddPatient_Form(GlobalSetup.mainWindow);
                ExtentLogger.info(_test, "Nhập thông tin bệnh nhân: " +
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

                addForm.EnterPatientInfo(
                    hoTen, gioiTinh, ngaySinh, sdt, diaChi, cccd, email,
                    chieuCao, canNang, diUng, nhomMau, tsBenhAn, ghiChu
                );

                ExtentLogger.info(_test, "Nhấn nút Lưu");
                addForm.ClickSave(GlobalSetup.automation, GlobalSetup.app.ProcessId);
                Thread.Sleep(300);

                string actualMessage = addForm.GetMessageBoxText(GlobalSetup.automation, GlobalSetup.app.ProcessId);
                ExtentLogger.info(_test, $"Mong đợi: '{expectedMessage}'");
                ExtentLogger.info(_test, $"Thực tế: '{actualMessage}'");

                Assert.AreEqual(expectedMessage, actualMessage);

                // Capture screenshot khi pass
                string screenshotPath = ExtentLogger.CaptureScreenshot(GlobalSetup.mainWindow, testCaseName + "_Pass");
                ExtentLogger.AttachScreenshot(_test, screenshotPath, "Screenshot khi test pass");

                ExtentLogger.passHighlight(_test, "Test case pass: Thông báo chính xác");
                addForm.CloseMessageBox(GlobalSetup.automation, GlobalSetup.app.ProcessId);
            }
            catch (Exception ex)
            {
                // Capture screenshot khi fail
                string screenshotPath = ExtentLogger.CaptureScreenshot(GlobalSetup.mainWindow, testCaseName + "_Fail");
                ExtentLogger.AttachScreenshot(_test, screenshotPath, "Screenshot khi test fail");

                ExtentLogger.failHighlight(_test, $"Test case fail: Thông báo ko khớp");
                Assert.Fail($"Lỗi: {ex.Message}");
            }
            finally
            {
                try
                {
                    var addForm = new AddPatient_Form(GlobalSetup.mainWindow);
                    addForm.ClickCancel();
                    ExtentLogger.info(_test, "Đã đóng form thêm bệnh nhân");
                }
                catch(Exception ex)
                {
                    Assert.Fail($"Lỗi khi đóng form thêm bệnh nhân: {ex.Message}");
                    Assert.Fail($"Lỗi: {ex.Message}");
                }
            }
        }
    }
}
