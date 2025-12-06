

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
            var _test = _extent.CreateTest("TC_039: Xóa vật tư thành công");
            _test.AssignCategory("Xóa vật tư");
            try
            {
                var grid = suppliesForm.SuppliesGrid;
                int initialRowCount = grid.Rows.Length;
                ExtentLogger.Info(_test, $"Số vật tư trước khi xóa: {initialRowCount}");

                ExtentLogger.Info(_test, "Xóa vật tư đầu tiên trong danh sách");
                suppliesForm.DeleteFirstSupply(GlobalSetup.automation, GlobalSetup.app.ProcessId);

                grid = suppliesForm.SuppliesGrid;
                int rowCountAfterDelete = grid.Rows.Length;
                ExtentLogger.Info(_test, $"Số vật tư sau khi xóa: {rowCountAfterDelete}");

                Assert.AreEqual(initialRowCount - 1, rowCountAfterDelete, "Số vật tư không giảm sau khi xóa!");
                ExtentLogger.Pass(_test, "Test case pass");
            }
            catch (Exception ex)
            {
                ExtentLogger.Fail(_test, $"Test case fail: {ex.Message}");
                Assert.Fail($"Lỗi trong quá trình xóa vật tư: {ex.Message}");
            }
        }
    }
}
