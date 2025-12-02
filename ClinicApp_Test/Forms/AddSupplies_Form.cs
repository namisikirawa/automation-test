using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicApp_Test.Forms
{
    public class AddSupplies_Form
    {
        private readonly Window _window;

        public AddSupplies_Form(Window window)
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
            TxtTenVatTu.Enter(tenvattu);
            TxtSL.Enter(sl);
            TxtDVT.Enter(dvt);
            TxtDongia.Enter(dongia);
            TxtNgaynhap.Enter(ngaynhap);
            TxtNgayhethan.Enter(ngayhethan);
            TxtNhacungcap.Enter(nhacungcap);
        }


        public void ClickSave(UIA3Automation automation, int processId)
        {
            BtnSave?.Click();
            Thread.Sleep(1000);

            // xác nhận Yes
            var desktop = automation.GetDesktop();
            var dialog = desktop.FindFirstChild(cf => cf.ByProcessId(processId)
                .And(cf.ByControlType(ControlType.Window)))?.AsWindow();
            var yesButton = dialog?.FindFirstDescendant(cf => cf.ByText("Yes"))?.AsButton();
            yesButton?.Click();
        }
        public void ClickCancel()
        {
            BtnCancel?.Click();
            Thread.Sleep(1000);
        }
        public string GetMessageBoxText(AutomationBase automation, int processId)
        {
            var desktop = automation.GetDesktop();
            var msgBox = desktop.FindFirstChild(cf => cf.ByProcessId(processId).And(cf.ByControlType(ControlType.Window)));
            var contentText = msgBox?.FindFirstDescendant(cf => cf.ByControlType(ControlType.Text))?.AsLabel();
            return contentText?.Text;
        }

        public void CloseMessageBox(AutomationBase automation, int processId)
        {
            var desktop = automation.GetDesktop();
            var msgBox = desktop.FindFirstChild(cf => cf.ByProcessId(processId).And(cf.ByControlType(ControlType.Window)));
            var okButton = msgBox?.FindFirstDescendant(cf => cf.ByAutomationId("2"))?.AsButton();
            okButton?.Invoke();
        }
    }
}
