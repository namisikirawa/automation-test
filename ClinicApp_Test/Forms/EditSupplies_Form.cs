using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicApp_Test.Forms
{
    public class EditSupplies_Form
    {
        private readonly Window _window;

        public EditSupplies_Form(Window window)
        {
            _window = window ?? throw new ArgumentNullException(nameof(window));
        }

        private AutomationElement GroupBox1 => _window.FindFirstDescendant(cf => cf.ByAutomationId("groupBox1"));
        private TextBox TxtTenVatTu => GroupBox1.FindFirstDescendant(cf => cf.ByAutomationId("txtTenvattu"))?.AsTextBox();
        private TextBox TxtSL => GroupBox1.FindFirstDescendant(cf => cf.ByAutomationId("txtSL"))?.AsTextBox();
        private TextBox TxtDVT => GroupBox1.FindFirstDescendant(cf => cf.ByAutomationId("txtDVT"))?.AsTextBox();
        private TextBox TxtDongia => GroupBox1.FindFirstDescendant(cf => cf.ByAutomationId("txtDongia"))?.AsTextBox();
        private TextBox TxtNgaynhap => GroupBox1.FindFirstDescendant(cf => cf.ByAutomationId("txtNgaynhap"))?.AsTextBox();
        private TextBox TxtNgayhethan => GroupBox1.FindFirstDescendant(cf => cf.ByAutomationId("txtNgayhethan"))?.AsTextBox();
        private TextBox TxtNhacungcap => GroupBox1.FindFirstDescendant(cf => cf.ByAutomationId("txtNhacungcap"))?.AsTextBox();


        private Button BtnSave => _window.FindFirstDescendant(cf => cf.ByAutomationId("btn_Save"))?.AsButton();
        private Button BtnCancel => _window.FindFirstDescendant(cf => cf.ByAutomationId("btn_Cancel"))?.AsButton();

        public void EnterSuppliesInfo(
            string tenvattu,
            string sl,
            string dvt,
            string dongia,
            string ngaynhap,
            string ngayhethan,
            string nhacungcap)
        {
            ClearAndEnter(TxtTenVatTu,tenvattu);
            ClearAndEnter(TxtSL,sl);
            ClearAndEnter(TxtDVT,dvt);
            ClearAndEnter(TxtDongia,dongia);
            ClearAndEnter(TxtNgaynhap,ngaynhap);
            ClearAndEnter(TxtNgayhethan,ngayhethan);
            ClearAndEnter(TxtNhacungcap,nhacungcap);
        }
        private void ClearAndEnter(TextBox textbox, string value)
        {
            textbox.Text = "";
            Thread.Sleep(50);        
            textbox.Enter(value);
        }
        public string ClickSaveAndGetMessage(UIA3Automation automation, int processId)
        {
            BtnSave?.Click();
            Thread.Sleep(500);

            var desktop = automation.GetDesktop();

            // --- kiểm tra có dialog xác nhận ko ---
            Window dialog = null;
            for (int i = 0; i < 5; i++)
            {
                dialog = desktop.FindFirstChild(cf => cf.ByProcessId(processId)
                             .And(cf.ByControlType(ControlType.Window)))?.AsWindow();
                if (dialog != null)
                    break;
                Thread.Sleep(200);
            }

            if (dialog == null)
                throw new Exception("Không tìm thấy dialog sau khi nhấn Save.");


            var confirmButton = dialog.FindFirstDescendant(cf => cf.ByText("Yes"))?.AsButton() ??
                                dialog.FindFirstDescendant(cf => cf.ByAutomationId("btn_Confirm"))?.AsButton();
            //nếu có xác nhận, chọn yes
            if (confirmButton != null)
            {
                confirmButton.Click();
                Thread.Sleep(500);
            }

            // --- kiểm tra msgbox ---
            Window msgBox = null;
            for (int i = 0; i < 5; i++)
            {
                msgBox = desktop.FindFirstChild(cf => cf.ByProcessId(processId)
                                .And(cf.ByControlType(ControlType.Window)))?.AsWindow();
                if (msgBox != null && msgBox.Title != dialog.Title)
                    break;
                Thread.Sleep(200);
            }

            if (msgBox == null)
                throw new Exception("Không tìm thấy message box.");

            string messageText = msgBox.FindFirstDescendant(cf => cf.ByControlType(ControlType.Text))
                                ?.AsLabel()?.Text;

            var okButton = msgBox.FindFirstDescendant(cf => cf.ByText("OK"))?.AsButton();
            okButton?.Click();
            Thread.Sleep(300);

            //đóng form edit nếu form còn mở (có nút hủy):
            var cancelBtn = _window.FindFirstDescendant(cf => cf.ByAutomationId("btn_Cancel"))?.AsButton();
            if (cancelBtn != null)
            {
                cancelBtn.Click();
                Thread.Sleep(500);
            }
            Thread.Sleep(500);
            return messageText;
        }
        public void ClickCancel()
        {
            BtnCancel?.Click();
            Thread.Sleep(1000);
        }

    }
}
