using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicApp_Test.Forms
{
    public class AddPatient_Form
    {
        private readonly Window _window;

        public AddPatient_Form(Window window)
        {
            _window = window ?? throw new ArgumentNullException(nameof(window));
        }

        private AutomationElement GroupBox1 => _window.FindFirstDescendant(cf => cf.ByAutomationId("groupBox1"));
        private TextBox TxtHoTen => GroupBox1.FindFirstDescendant(cf => cf.ByAutomationId("txtHoten"))?.AsTextBox();
        private TextBox TxtGioiTinh => GroupBox1.FindFirstDescendant(cf => cf.ByAutomationId("txtGioitinh"))?.AsTextBox();
        private TextBox TxtNgaySinh => GroupBox1.FindFirstDescendant(cf => cf.ByAutomationId("txtNgaysinh"))?.AsTextBox();
        private TextBox TxtSDT => GroupBox1.FindFirstDescendant(cf => cf.ByAutomationId("txtSdt"))?.AsTextBox();
        private TextBox TxtDiaChi => GroupBox1.FindFirstDescendant(cf => cf.ByAutomationId("txtDiachi"))?.AsTextBox();
        private TextBox TxtCCCD => GroupBox1.FindFirstDescendant(cf => cf.ByAutomationId("txtCMND"))?.AsTextBox();
        private TextBox TxtEmail => GroupBox1.FindFirstDescendant(cf => cf.ByAutomationId("txtEmail"))?.AsTextBox();

        private AutomationElement GroupBox2 => _window.FindFirstDescendant(cf => cf.ByAutomationId("groupBox2"));
        private TextBox TxtChieuCao => GroupBox2.FindFirstDescendant(cf => cf.ByAutomationId("txtChieucao"))?.AsTextBox();
        private TextBox TxtCanNang => GroupBox2.FindFirstDescendant(cf => cf.ByAutomationId("txtCannang"))?.AsTextBox();
        private TextBox TxtDiUng => GroupBox2.FindFirstDescendant(cf => cf.ByAutomationId("txtDiung"))?.AsTextBox();
        private TextBox TxtNhomMau => GroupBox2.FindFirstDescendant(cf => cf.ByAutomationId("txtNhommau"))?.AsTextBox();
        private TextBox TxtTSBenhAn => GroupBox2.FindFirstDescendant(cf => cf.ByAutomationId("txtTiensubenhan"))?.AsTextBox();
        private TextBox TxtGhiChu => GroupBox2.FindFirstDescendant(cf => cf.ByAutomationId("txtGhichu"))?.AsTextBox();

        private Button BtnSave => _window.FindFirstDescendant(cf => cf.ByAutomationId("btn_Save"))?.AsButton();
        private Button BtnCancel => _window.FindFirstDescendant(cf => cf.ByAutomationId("btn_Cancel"))?.AsButton();

        public void EnterPatientInfo(
            string hoTen, string gioiTinh, string ngaySinh, string sdt,
            string diaChi, string cccd, string email, string chieuCao,
            string canNang, string diUng, string nhomMau,
            string tsBenhAn, string ghiChu)
        {
            TxtHoTen.Enter(hoTen);
            TxtGioiTinh.Enter(gioiTinh);
            TxtNgaySinh.Enter(ngaySinh);
            TxtSDT.Enter(sdt);
            TxtDiaChi.Enter(diaChi);
            TxtCCCD.Enter(cccd);

            TxtEmail.Enter(email);
            TxtChieuCao.Enter(chieuCao);
            TxtCanNang.Enter(canNang);
            TxtDiUng.Enter(diUng);
            TxtNhomMau.Enter(nhomMau);
            TxtTSBenhAn.Enter(tsBenhAn);
            TxtGhiChu.Enter(ghiChu);
        }

        public void ClickSave(UIA3Automation automation, int processId)
        {
            BtnSave?.Click();
            Thread.Sleep(500);

            var desktop = automation.GetDesktop();
            var dialog = desktop.FindFirstChild(cf => cf.ByProcessId(processId)
                .And(cf.ByControlType(ControlType.Window)))?.AsWindow();
            var yesButton = dialog?.FindFirstDescendant(cf => cf.ByText("Yes"))?.AsButton();
            yesButton?.Click();
        }

        public void ClickCancel()
        {
            BtnCancel?.Click();
            Thread.Sleep(500);
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
