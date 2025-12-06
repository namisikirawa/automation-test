using ClinicApp_Test.Extent;
using ClinicApp_Test.Form;
using ClinicApp_Test.Forms;

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
                int beforeCount = patientForm.GetRowCount();

                ExtentLogger.Info(_test, "Nhấn nút 'Thêm mới'");
                patientForm.ClickAddButton();
                Thread.Sleep(500);

                var addForm = new AddPatient_Form(GlobalSetup.mainWindow);
                ExtentLogger.Info(_test, "Nhập thông tin bệnh nhân: " +
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

                ExtentLogger.Info(_test, "Nhấn nút Lưu");
                addForm.ClickSave(GlobalSetup.automation, GlobalSetup.app.ProcessId);
                Thread.Sleep(300);

                string actualMessage = addForm.GetMessageBoxText(GlobalSetup.automation, GlobalSetup.app.ProcessId);
                ExtentLogger.Info(_test, $"Thông báo mong đợi: '{expectedMessage}'");
                ExtentLogger.Info(_test, $"Thực tế: '{actualMessage}'");
                if (expectedMessage != actualMessage)
                {
                    ExtentLogger.Fail(_test, "Test case fail: Thông báo không khớp!");
                    addForm.CloseMessageBox(GlobalSetup.automation, GlobalSetup.app.ProcessId);
                    Assert.Fail("Message box text mismatch!");
                }

                //pass phần kiểm tra hộp thoại: screenshot+ thông báo
                ExtentLogger.Pass(_test, "Thông báo trùng khớp");
                addForm.CloseMessageBox(GlobalSetup.automation, GlobalSetup.app.ProcessId);

                if (expectedMessage == "Thêm bệnh nhân thành công!")
                {
                    //kiểm tra số bệnh nhân có tăng thêm 1 không
                    int afterCount = patientForm.GetRowCount();
                    ExtentLogger.Info(_test, $"Số bệnh nhân trước khi thêm: {beforeCount}");
                    ExtentLogger.Info(_test, $"Số bệnh nhân sau khi thêm: {afterCount}");

                    if (afterCount != beforeCount + 1)
                    {
                        ExtentLogger.Error(_test, "Số lượng bệnh nhân không tăng thêm 1 sau khi thêm!");
                        Assert.Fail("Row count mismatch!");
                    }
                }

                ExtentLogger.PassWithoutScreenshot(_test, "Test case passed!");
            }
            catch (Exception ex)
            {
                ExtentLogger.Fail(_test, $"Test case fail: {ex.Message}");
                Assert.Fail($"Lỗi: {ex.Message}");
            }
            finally
            {
                try
                {
                    var addForm = new AddPatient_Form(GlobalSetup.mainWindow);
                    addForm.ClickCancel();
                    ExtentLogger.Info(_test, "Đã đóng form thêm bệnh nhân");
                }
                catch(Exception ex)
                {
                    Assert.Fail($"Lỗi khi đóng form thêm bệnh nhân: {ex.Message}");
                    ExtentLogger.Error(_test,$"Lỗi: {ex.Message}");
                }
            }
        }
    }
}
