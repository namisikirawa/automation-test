using FlaUI.Core.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicApp_Test.Form
{
    public class EmployeeForm
    {
        private readonly Window _window;

        public EmployeeForm(Window window)
        {
            _window = window ?? throw new ArgumentNullException(nameof(window));
        }

        public DataGridView EmployeeGrid =>
            _window.FindFirstDescendant(cf => cf.ByAutomationId("dgv_Employee"))?.AsDataGridView();
        private AutomationElement GroupBox =>
           _window.FindFirstDescendant(cf => cf.ByAutomationId("groupBox1"));

        private TextBox TxtHoTen =>
            GroupBox.FindFirstDescendant(cf => cf.ByAutomationId("txtHoTen"))?.AsTextBox();

        private TextBox TxtSdt =>
            GroupBox.FindFirstDescendant(cf => cf.ByAutomationId("txtSDT"))?.AsTextBox();

        private Button BtnSearch =>
            GroupBox.FindFirstDescendant(cf => cf.ByAutomationId("btnSearch"))?.AsButton();

        //xóa nhân viên đầu tiên trong gridview
        public void DeleteFirstEmployee(UIA3Automation automation, int processId)
        {
            var grid = EmployeeGrid;
            if (grid == null)
                throw new Exception("Không tìm thấy bảng nhân viên!");

            var firstRow = grid.Rows[0];
            var clickPoint = firstRow.Properties.BoundingRectangle.Value.Center();

            Mouse.MoveTo(clickPoint);
            Mouse.Click(MouseButton.Right);
            Thread.Sleep(500);

            var contextMenu = automation.GetDesktop().FindFirstDescendant(cf => cf.ByControlType(ControlType.Menu));
            var deleteItem = contextMenu.FindFirstDescendant(cf =>
                cf.ByControlType(ControlType.MenuItem).And(cf.ByName("Xóa")))?.AsMenuItem();
            deleteItem?.Click();

            Thread.Sleep(500);

            // xác nhận Yes
            var desktop = automation.GetDesktop();
            var dialog = desktop.FindFirstChild(cf => cf.ByProcessId(processId)
                .And(cf.ByControlType(ControlType.Window)))?.AsWindow();
            var yesButton = dialog?.FindFirstDescendant(cf => cf.ByText("Yes"))?.AsButton();
            yesButton?.Click();

            Thread.Sleep(500);

            // đóng messagebox "Đã xóa thành công"
            var msgBox = desktop.FindFirstChild(cf => cf.ByProcessId(processId)
                .And(cf.ByControlType(ControlType.Window)));
            var okButton = msgBox?.FindFirstDescendant(cf => cf.ByAutomationId("2"))?.AsButton();
            okButton?.Click();

            Thread.Sleep(1000);
        }
        // --- Thao tác tìm nhân viên theo tên hoặc số điện thoại ---
        public void FindEmployee(string name, string phoneNumber)
        {
            if (TxtHoTen == null || TxtSdt == null || BtnSearch == null)
                throw new Exception("Không tìm thấy các control tìm kiếm trên form nhân viên.");

            // Xóa dữ liệu cũ
            TxtHoTen.Text = string.Empty;
            TxtSdt.Text = string.Empty;
            Thread.Sleep(200);

            if (!string.IsNullOrWhiteSpace(name))
                TxtHoTen.Enter(name);

            if (!string.IsNullOrWhiteSpace(phoneNumber))
                TxtSdt.Enter(phoneNumber);

            BtnSearch.Click();
            Thread.Sleep(1000);
        }

        // --- Lấy danh sách dữ liệu theo từng dòng (để so sánh chính xác hơn) ---
        public List<List<string>> GetAllGridValues()
        {
            var dataGrid = EmployeeGrid;
            if (dataGrid == null)
                throw new Exception("Không tìm thấy bảng dữ liệu nhân viên.");

            var allRows = dataGrid.Rows
            .Select(row => row.Cells
                .Select(cell => cell.Value?.ToString()?.Trim() ?? string.Empty)
                .ToList())
            .ToList();

            // Lọc bỏ các dòng mà tất cả các ô đều rỗng hoặc null (dòng cuối cùng trong gridview)
            var filteredRows = allRows
                .Where(row => row.Any(cell =>
                    !string.IsNullOrWhiteSpace(cell) &&
                    !string.Equals(cell, "(null)", StringComparison.OrdinalIgnoreCase)))
                .ToList();

            return filteredRows;
        }
    }
}
