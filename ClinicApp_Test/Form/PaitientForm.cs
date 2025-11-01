using FlaUI.Core.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicApp_Test.Form
{
    public class PatientForm
    {
        private readonly Window _window;

        public PatientForm(Window window)
        {
            _window = window ?? throw new ArgumentNullException(nameof(window));
        }

        public DataGridView PatientGrid =>
            _window.FindFirstDescendant(cf => cf.ByAutomationId("dgv_Patient"))?.AsDataGridView();
        private AutomationElement PatientManagement =>
           _window.FindFirstDescendant(cf => cf.ByAutomationId("PatientManagement"));

        private AutomationElement GroupBox =>
            PatientManagement?.FindFirstDescendant(cf => cf.ByAutomationId("groupBox1"));

        public TextBox NameTextBox =>
            GroupBox?.FindFirstDescendant(cf => cf.ByAutomationId("txtHoTen"))?.AsTextBox();

        public TextBox PhoneTextBox =>
            GroupBox?.FindFirstDescendant(cf => cf.ByAutomationId("txtSdt"))?.AsTextBox();

        public Button SearchButton =>
            GroupBox?.FindFirstDescendant(cf => cf.ByAutomationId("btnSearch"))?.AsButton();

        public void DeleteFirstPatient(UIA3Automation automation, int processId)
        {
            var grid = PatientGrid;
            if (grid == null)
                throw new Exception("Không tìm thấy bảng bệnh nhân!");

            if (grid.Rows.Length == 0)
                throw new Exception("Không có dữ liệu để xóa!");

            var firstRow = grid.Rows[0];
            var clickPoint = firstRow.Properties.BoundingRectangle.Value.Center();

            Mouse.MoveTo(clickPoint);
            Mouse.Click(MouseButton.Right);
            Thread.Sleep(500);

            var contextMenu = automation.GetDesktop().FindFirstDescendant(cf => cf.ByControlType(ControlType.Menu));
            var deleteItem = contextMenu?.FindFirstDescendant(cf =>
                cf.ByControlType(ControlType.MenuItem).And(cf.ByName("Xóa")))?.AsMenuItem();

            deleteItem?.Click();
            Thread.Sleep(500);

            var desktop = automation.GetDesktop();
            var dialog = desktop.FindFirstChild(cf => cf.ByProcessId(processId)
                .And(cf.ByControlType(ControlType.Window)))?.AsWindow();

            var yesButton = dialog?.FindFirstDescendant(cf => cf.ByText("Yes"))?.AsButton();
            yesButton?.Click();
            Thread.Sleep(500);

            var msgBox = desktop.FindFirstChild(cf => cf.ByProcessId(processId)
                .And(cf.ByControlType(ControlType.Window)));
            var okButton = msgBox?.FindFirstDescendant(cf => cf.ByAutomationId("2"))?.AsButton();
            okButton?.Click();
            Thread.Sleep(1000);
        }

        // ---tìm bệnh nhân theo tên hoặc số điện thoại ---
        public void FindPatient(string name, string phoneNumber)
        {
            if (NameTextBox == null || PhoneTextBox == null || SearchButton == null)
                throw new Exception("Không tìm thấy các control tìm kiếm trên form bệnh nhân.");

            // Xóa dữ liệu cũ
            NameTextBox.Text = string.Empty;
            PhoneTextBox.Text = string.Empty;
            Thread.Sleep(200);

            if (!string.IsNullOrWhiteSpace(name))
                NameTextBox.Enter(name);

            if (!string.IsNullOrWhiteSpace(phoneNumber))
                PhoneTextBox.Enter(phoneNumber);

            SearchButton.Click();
            Thread.Sleep(1000);
        }

        // --- Lấy danh sách dữ liệu theo từng dòng (để so sánh chính xác hơn) ---
        public List<List<string>> GetAllGridValues()
        {
            var dataGrid = PatientGrid;
            if (dataGrid == null)
                throw new Exception("Không tìm thấy bảng dữ liệu bệnh nhân.");

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
