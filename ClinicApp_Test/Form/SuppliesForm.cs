using FlaUI.Core.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicApp_Test.Form
{
    public class SuppliesForm
    {
        private readonly Window _window;

        public SuppliesForm(Window window)
        {
            _window = window ?? throw new ArgumentNullException(nameof(window));
        }

        public DataGridView SuppliesGrid =>
            _window.FindFirstDescendant(cf => cf.ByAutomationId("dgv_Supplies"))?.AsDataGridView();
        private AutomationElement GroupBox =>
            _window.FindFirstDescendant(cf => cf.ByAutomationId("groupBox1"));

        private TextBox TxtTenvattu =>
            GroupBox.FindFirstDescendant(cf => cf.ByAutomationId("txtTenvattu"))?.AsTextBox();

        private TextBox TxtNhacungcap =>
            GroupBox.FindFirstDescendant(cf => cf.ByAutomationId("txtNhacungcap"))?.AsTextBox();

        private Button BtnSearch =>
            GroupBox.FindFirstDescendant(cf => cf.ByAutomationId("btnSearch"))?.AsButton();

        public void DeleteFirstSupply(UIA3Automation automation, int processId)
        {
            var grid = SuppliesGrid;
            if (grid == null)
                throw new Exception("Không tìm thấy bảng vật tư!");

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

        // --- Thao tác tìm vật tư theo tên hoặc nhà cung cấp ---
        public void FindSupplies(string name, string provider)
        {
            if (TxtTenvattu == null || TxtNhacungcap == null || BtnSearch == null)
                throw new Exception("Không tìm thấy các control tìm kiếm trên form vật tư.");

            // Xóa dữ liệu cũ
            TxtTenvattu.Text = string.Empty;
            TxtNhacungcap.Text = string.Empty;
            Thread.Sleep(200);

            if (!string.IsNullOrWhiteSpace(name))
                TxtTenvattu.Enter(name);

            if (!string.IsNullOrWhiteSpace(provider))
                TxtNhacungcap.Enter(provider);

            BtnSearch.Click();
            Thread.Sleep(1000);
        }

        // --- Lấy danh sách dữ liệu theo từng dòng (để so sánh chính xác hơn) ---
        public List<List<string>> GetAllGridValues()
        {
            var dataGrid = SuppliesGrid;
            if (dataGrid == null)
                throw new Exception("Không tìm thấy bảng dữ liệu vật tư.");

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
