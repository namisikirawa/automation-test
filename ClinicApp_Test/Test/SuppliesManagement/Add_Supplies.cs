using AventStack.ExtentReports.Gherkin.Model;
using ClinicApp_Test.Extent;
using ClinicApp_Test.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ClinicApp_Test.Test.SuppliesManagement
{
    [TestClass]
    public class Add_Supplies : BaseTest_Report
    {
        private static SuppliesForm suppliesForm;
        private const string data = @"D:\Đồ án tốt nghiệp\AutomationTest_Project\ClinicApp_Test\ClinicApp_Test\Data\add_supplies.csv";

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            var mainForm = GlobalSetup.mainForm;

            mainForm.OpenSuppliesManagement();
            Thread.Sleep(800);

            suppliesForm = new SuppliesForm(GlobalSetup.mainWindow);
        }

        public static IEnumerable<object[]> GetAddSuppliesData()
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
        [DynamicData(nameof(GetAddSuppliesData), DynamicDataSourceType.Method)]
        public void AddSuppliesTest(string testCaseName,
            string tenvattu, string sl, string dvt,
            string dongia, string ngaynhap, string ngayhethan,
            string nhacungcap, string expectedMessage)
        {
            var _test = _extent.CreateTest(testCaseName);
            _test.AssignCategory("Thêm vật tư");
            try
            {
                ExtentLogger.info(_test, "Nhấn nút Thêm vật tư");
                suppliesForm.ClickAddButton();
                Thread.Sleep(500);

                var addForm = new AddSupplies_Form(GlobalSetup.mainWindow);
                ExtentLogger.info(_test, "Nhập thông tin vật tư: " +
                    $"Tên vật tư: {tenvattu}, " +
                    $"Số lượng: {sl}, " +
                    $"Đơn vị tính: {dvt}, " +
                    $"Đơn giá: {dongia}, " +
                    $"Ngày nhập: {ngaynhap}, " +
                    $"Ngày hết hạn: {ngayhethan}, " +
                    $"Nhà cung cấp: {nhacungcap}"
                );

                addForm.EnterSuppliesInfo(tenvattu, sl, dvt, dongia, ngaynhap, ngayhethan, nhacungcap);

                ExtentLogger.info(_test, "Nhấn nút Lưu");
                addForm.ClickSave(GlobalSetup.automation, GlobalSetup.app.ProcessId);
                Thread.Sleep(300);

                string actualMessage = addForm.GetMessageBoxText(GlobalSetup.automation, GlobalSetup.app.ProcessId);
                ExtentLogger.info(_test, $"Mong đợi: '{expectedMessage}'");
                ExtentLogger.info(_test, $"Thực tế: '{actualMessage}'");

                Assert.AreEqual(expectedMessage, actualMessage);
                ExtentLogger.passHighlight(_test, "Test case pass: Thông báo chính xác");
                addForm.CloseMessageBox(GlobalSetup.automation, GlobalSetup.app.ProcessId);
            }
            catch (Exception ex)
            {
                ExtentLogger.failHighlight(_test, $"Test case fail: Thông báo không khớp");
                Assert.Fail($"Lỗi: {ex.Message}");
            }
            finally
            {
                try
                {
                    var addForm = new AddSupplies_Form(GlobalSetup.mainWindow);
                    addForm.ClickCancel();
                    ExtentLogger.info(_test, "Đã đóng form thêm vật tư");
                }
                catch (Exception ex)
                {
                    Assert.Fail($"Lỗi khi đóng form thêm vật tư: {ex.Message}");
                    Assert.Fail($"Lỗi: {ex.Message}");
                }
            }
        }
    }
}
