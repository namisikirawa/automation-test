using ClinicApp_Test.Extent;
using ClinicApp_Test.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicApp_Test.Test.SuppliesManagement
{
    [TestClass]
    public class Edit_Supplies : BaseTest_Report
    {
        private static SuppliesForm suppliesForm;
        private const string data = @"D:\Đồ án tốt nghiệp\AutomationTest_Project\ClinicApp_Test\ClinicApp_Test\Data\edit_supplies.csv";

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            var mainForm = GlobalSetup.mainForm;
            mainForm.OpenSuppliesManagement();
            Thread.Sleep(1000);

            suppliesForm = new SuppliesForm(GlobalSetup.mainWindow);
        }

        public static IEnumerable<object[]> GetEditSuppliesData()
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
                    csv.GetField<string>("tenVatTu"),
                    csv.GetField<string>("SL"),
                    csv.GetField<string>("DVT"),
                    csv.GetField<string>("donGia"),
                    csv.GetField<string>("ngayNhap"),
                    csv.GetField<string>("ngayHetHan"),
                    csv.GetField<string>("nhaCungCap"),
                    csv.GetField<string>("expectedMessage")
                };
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(GetEditSuppliesData), DynamicDataSourceType.Method)]
        public void EditSuppliesTest(string testCaseName,
            string tenCanTim, string tenvattu, string sl, string dvt,
            string dongia, string ngaynhap, string ngayhethan,
            string nhacungcap, string expectedMessage)
        {
            var _test = _extent.CreateTest(testCaseName);
            _test.AssignCategory("Sửa vật tư");
            try
            {
                ExtentLogger.Info(_test, $"Tìm vật tư cần sửa: {tenCanTim}");
                suppliesForm.FindSupplies(tenCanTim, "");
                suppliesForm.EditFirstSupplies(GlobalSetup.automation, GlobalSetup.app.ProcessId);
                ExtentLogger.Info(_test, "Click chuột phải, chọn 'Sửa'");

                ExtentLogger.Info(_test, "Mở form sửa vật tư");
                var editForm = new EditSupplies_Form(GlobalSetup.mainWindow);

                ExtentLogger.Info(_test, "Nhập thông tin mới: " +
                    $"Tên vật tư: {tenvattu}, " +
                    $"Số lượng: {sl}, " +
                    $"Đơn vị tính: {dvt}, " +
                    $"Đơn giá: {dongia}, " +
                    $"Ngày nhập: {ngaynhap}, " +
                    $"Ngày hết hạn: {ngayhethan}, " +
                    $"Nhà cung cấp: {nhacungcap}"
                );

                editForm.EnterSuppliesInfo(tenvattu, sl, dvt, dongia, ngaynhap, ngayhethan, nhacungcap);

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
            catch (AssertFailedException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Assert.Fail("Lỗi trong quá trình test sửa vật tư: " + ex.Message);
                ExtentLogger.Fail(_test, $"Test case fail: {ex.Message}");
            }
        }
    }
}
