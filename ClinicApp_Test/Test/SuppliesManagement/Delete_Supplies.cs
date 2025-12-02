

using ClinicApp_Test.Extent;
using System.Diagnostics;

namespace ClinicApp_Test.Test.SuppliesManagement
{
    [TestClass]
    public class Delete_Supplies : BaseTest_Report
    {
        private static SuppliesForm suppliesForm;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            var mainForm = GlobalSetup.mainForm;

            mainForm.OpenSuppliesManagement();
            Thread.Sleep(1000);

            suppliesForm = new SuppliesForm(GlobalSetup.mainWindow);
        }

        [TestMethod]
        public void TestDeleteSupplies()
        {
            var _test = _extent.CreateTest("TC_003: Xóa vật tư thành công");
            _test.AssignCategory("Xóa vật tư");
            try
            {
                var grid = suppliesForm.SuppliesGrid;
                int initialRowCount = grid.Rows.Length;
                ExtentLogger.info(_test, $"Số dòng trước khi xóa: {initialRowCount}");

                ExtentLogger.info(_test, "Xóa nhân viên đầu tiên trong danh sách");
                suppliesForm.DeleteFirstSupply(GlobalSetup.automation, GlobalSetup.app.ProcessId);

                grid = suppliesForm.SuppliesGrid;
                int rowCountAfterDelete = grid.Rows.Length;
                ExtentLogger.info(_test, $"Số dòng sau khi xóa: {rowCountAfterDelete}");

                Assert.AreEqual(initialRowCount - 1, rowCountAfterDelete, "Số dòng không giảm sau khi xóa!");
                ExtentLogger.passHighlight(_test, "Test case pass: Xóa vật tư thành công");
            }
            catch (Exception ex)
            {
                ExtentLogger.failHighlight(_test, $"Test case fail: Xóa vật tư thất bại");
                Assert.Fail($"Lỗi trong quá trình xóa vật tư: {ex.Message}");
            }
        }
    }
}
