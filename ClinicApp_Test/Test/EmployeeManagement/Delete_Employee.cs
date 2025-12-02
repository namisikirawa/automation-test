
using ClinicApp_Test.Extent;
using System.Diagnostics;

namespace ClinicApp_Test.Test.EmployeeManagement
{
    [TestClass]
    public class Delete_Employee : BaseTest_Report
    {
        private static EmployeeForm employeeForm;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            var mainForm = GlobalSetup.mainForm;

            mainForm.OpenEmployeeManagement();
            Thread.Sleep(1000);

            employeeForm = new EmployeeForm(GlobalSetup.mainWindow);
        }

        [TestMethod]
        public void TestDeleteEmployee()
        {
            var _test = _extent.CreateTest("TC_001: Xóa nhân viên thành công");
            _test.AssignCategory("Thêm nhân viên");
            try
            {
                var grid = employeeForm.EmployeeGrid;
                int initialRowCount = grid.Rows.Length;
                ExtentLogger.info(_test, $"Số dòng trước khi xóa: {initialRowCount}");

                ExtentLogger.info(_test, "Xóa nhân viên đầu tiên trong danh sách");
                employeeForm.DeleteFirstEmployee(GlobalSetup.automation, GlobalSetup.app.ProcessId);

                grid = employeeForm.EmployeeGrid;
                int rowCountAfterDelete = grid.Rows.Length;
                ExtentLogger.info(_test, $"Số dòng sau khi xóa: {rowCountAfterDelete}");

                Assert.AreEqual(initialRowCount - 1, rowCountAfterDelete, "Số dòng không giảm sau khi xóa!");
                ExtentLogger.passHighlight(_test, "Test case pass: Xóa nhân viên thành công");
            }
            catch (Exception ex)
            {
                ExtentLogger.failHighlight(_test, $"Test case fail: Xóa nhân viên thất bại");
                Assert.Fail($"Lỗi trong quá trình xóa nhân viên: {ex.Message}");
            }
        }
    }
}
