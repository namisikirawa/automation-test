// EditPatient_Form.cs
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.Core.Tools;
using System;
using System.Threading;

namespace ClinicApp_Test.Forms
{
    public class EditPatient_Form
    {
        private readonly Window _window;

        public EditPatient_Form(Window window)
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
        private TextBox TxtCMND => GroupBox1.FindFirstDescendant(cf => cf.ByAutomationId("txtCMND"))?.AsTextBox();

        private AutomationElement GroupBox2 => _window.FindFirstDescendant(cf => cf.ByAutomationId("groupBox2"));
        private TextBox TxtChieuCao => GroupBox2.FindFirstDescendant(cf => cf.ByAutomationId("txtChieucao"))?.AsTextBox();        
        private TextBox TxtCanNang => GroupBox2.FindFirstDescendant(cf => cf.ByAutomationId("txtCannang"))?.AsTextBox();
        private TextBox TxtDiUng => GroupBox2.FindFirstDescendant(cf => cf.ByAutomationId("txtDiung"))?.AsTextBox();
        private TextBox TxtNhomMau => GroupBox2.FindFirstDescendant(cf => cf.ByAutomationId("txtNhommau"))?.AsTextBox();

        private TextBox TxtTienSuBenhAn => GroupBox2.FindFirstDescendant(cf => cf.ByAutomationId("txtTiensubenhan"))?.AsTextBox();
        private TextBox TxtGhiChu => GroupBox2.FindFirstDescendant(cf => cf.ByAutomationId("txtGhichu"))?.AsTextBox();


        private Button BtnSave => _window.FindFirstDescendant(cf => cf.ByAutomationId("btn_Save"))?.AsButton();
        private Button BtnCancel => _window.FindFirstDescendant(cf => cf.ByAutomationId("btn_Cancel"))?.AsButton();

        public void EnterPatientInfo(string hoTen, string gioiTinh, string ngaySinh, string sdt, string diaChi, string email,
                                     string cmnd, string chieucao, string cannang, string diung, string nhommau, string tiensubenhan, string ghichu)
        {
            ClearAndEnter(TxtHoTen, hoTen);
            ClearAndEnter(TxtGioiTinh, gioiTinh);
            ClearAndEnter(TxtNgaySinh, ngaySinh);
            ClearAndEnter(TxtSDT, sdt);
            ClearAndEnter(TxtDiaChi, diaChi);
            ClearAndEnter(TxtEmail, email);
            ClearAndEnter(TxtCMND, cmnd);
            ClearAndEnter(TxtChieuCao, chieucao);
            ClearAndEnter(TxtCanNang, cannang);
            ClearAndEnter(TxtDiUng, diung);
            ClearAndEnter(TxtNhomMau, nhommau);
            ClearAndEnter(TxtTienSuBenhAn, tiensubenhan);
            ClearAndEnter(TxtGhiChu, ghichu);
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
            Window dialog = null;
            for (int i = 0; i < 5; i++)
            {
                dialog = desktop.FindFirstChild(cf => cf.ByProcessId(processId)
                             .And(cf.ByControlType(ControlType.Window)))?.AsWindow();
                if (dialog != null)
                    break;
                Thread.Sleep(200);
            }

            if (dialog != null)
            {
                var confirmButton = dialog.FindFirstDescendant(cf => cf.ByText("Yes"))?.AsButton() ??
                                    dialog.FindFirstDescendant(cf => cf.ByAutomationId("btn_Confirm"))?.AsButton();
                confirmButton?.Click();
                Thread.Sleep(500);
            }

            Window msgBox = null;
            for (int i = 0; i < 5; i++)
            {
                msgBox = desktop.FindFirstChild(cf => cf.ByProcessId(processId)
                                .And(cf.ByControlType(ControlType.Window)))?.AsWindow();
                if (msgBox != null && msgBox.Title != dialog?.Title)
                    break;
                Thread.Sleep(200);
            }

            if (msgBox == null)
                throw new Exception("Không tìm thấy message box.");

            string messageText = msgBox.FindFirstDescendant(cf => cf.ByControlType(ControlType.Text))
                                ?.AsLabel()?.Text;

            msgBox.FindFirstDescendant(cf => cf.ByText("OK"))?.AsButton()?.Click();
            Thread.Sleep(300);

            BtnCancel?.Click();
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
