using FlaUI.Core.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicApp_Test.Forms
{
    public class AddEmployee_Form
    {
        private readonly Window _window;

        public AddEmployee_Form(Window window)
        {
            _window = window ?? throw new ArgumentNullException(nameof(window));
        }

        private AutomationElement GroupBox1 => _window.FindFirstDescendant(cf => cf.ByAutomationId("groupBox1"));
        private TextBox TxtHoTen => GroupBox1.FindFirstDescendant(cf => cf.ByAutomationId("txtHoten"))?.AsTextBox();
        private TextBox TxtGioiTinh => GroupBox1.FindFirstDescendant(cf => cf.ByAutomationId("txtGioitinh"))?.AsTextBox();
        private TextBox TxtNgaySinh => GroupBox1.FindFirstDescendant(cf => cf.ByAutomationId("txtNgaysinh"))?.AsTextBox();
        private TextBox TxtSDT => GroupBox1.FindFirstDescendant(cf => cf.ByAutomationId("txtSdt"))?.AsTextBox();
        private TextBox TxtDiaChi => GroupBox1.FindFirstDescendant(cf => cf.ByAutomationId("txtDiachi"))?.AsTextBox();
        private TextBox TxtEmail => GroupBox1.FindFirstDescendant(cf => cf.ByAutomationId("txtEmail"))?.AsTextBox();

        private AutomationElement GroupBox2 => _window.FindFirstDescendant(cf => cf.ByAutomationId("groupBox2"));
        private TextBox TxtChucVu => GroupBox2.FindFirstDescendant(cf => cf.ByAutomationId("txtChucvu"))?.AsTextBox();
        private TextBox TxtGioLamViec => GroupBox2.FindFirstDescendant(cf => cf.ByAutomationId("txtGiolamviec"))?.AsTextBox();
        private TextBox TxtBangCap => GroupBox2.FindFirstDescendant(cf => cf.ByAutomationId("txtBangcap"))?.AsTextBox();
        private TextBox TxtKinhNghiem => GroupBox2.FindFirstDescendant(cf => cf.ByAutomationId("txtKNLamviec"))?.AsTextBox();

        // --- Nút Lưu / Hủy ---
        private Button BtnSave => _window.FindFirstDescendant(cf => cf.ByAutomationId("btn_Save"))?.AsButton();
        private Button BtnCancel => _window.FindFirstDescendant(cf => cf.ByAutomationId("btn_Cancel"))?.AsButton();

        public void EnterEmployeeInfo(
            string hoTen,
            string gioiTinh,
            string ngaySinh,
            string sdt,
            string diaChi,
            string email,
            string chucVu,
            string gioLamViec,
            string bangCap,
            string kinhNghiem)
        {
            TxtHoTen.Enter(hoTen);
            TxtGioiTinh.Enter(gioiTinh);
            TxtNgaySinh.Enter(ngaySinh);
            TxtSDT.Enter(sdt);
            TxtDiaChi.Enter(diaChi);
            TxtEmail.Enter(email);
            TxtChucVu.Enter(chucVu);
            TxtGioLamViec.Enter(gioLamViec);
            TxtBangCap.Enter(bangCap);
            TxtKinhNghiem.Enter(kinhNghiem);
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
