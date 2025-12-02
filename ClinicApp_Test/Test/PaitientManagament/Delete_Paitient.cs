

using ClinicApp_Test.Extent;
using System.Diagnostics;

namespace ClinicApp_Test.Test.PaitientManagament
{
    [TestClass]
    public class Delete_Patient : BaseTest_Report
    {
        private static PatientForm patientForm;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            var mainForm = GlobalSetup.mainForm;

            mainForm.OpenPaitientManagement();
            Thread.Sleep(1000);

            patientForm = new PatientForm(GlobalSetup.mainWindow);
        }

        [TestMethod]
        public void TestDeletePatient()
        {
            var _test = _extent.CreateTest("TC_002: Xóa bệnh nhân thành công");
            _test.AssignCategory("Xóa bệnh nhân");
            try
            {
                var grid = patientForm.PatientGrid;
                int initialRowCount = grid.Rows.Length;
                ExtentLogger.info(_test, $"Số dòng trước khi xóa: {initialRowCount}");

                ExtentLogger.info(_test, "Xóa nhân viên đầu tiên trong danh sách");
                patientForm.DeleteFirstPatient(GlobalSetup.automation, GlobalSetup.app.ProcessId);

                grid = patientForm.PatientGrid;
                int rowCountAfterDelete = grid.Rows.Length;
                ExtentLogger.info(_test, $"Số dòng sau khi xóa: {rowCountAfterDelete}");

                Assert.AreEqual(initialRowCount - 1, rowCountAfterDelete,"Số dòng không giảm sau khi xóa!");
                ExtentLogger.passHighlight(_test, "Test case pass: Xóa bệnh nhân thành công");
            }
            catch (Exception ex)
            {
                ExtentLogger.failHighlight(_test, $"Test case fail: Xóa bệnh nhân thất bại");
                Assert.Fail($"Lỗi trong quá trình xóa bệnh nhân: {ex.Message}");
            }
        }
    }
}
