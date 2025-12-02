using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicApp_Test.Forms
{
    public class EditEmployee_Form
    {
        private readonly Window _window;

        public EditEmployee_Form(Window window)
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
            ClearAndEnter(TxtHoTen, hoTen);
            ClearAndEnter(TxtGioiTinh, gioiTinh);
            ClearAndEnter(TxtNgaySinh, ngaySinh);
            ClearAndEnter(TxtSDT, sdt);
            ClearAndEnter(TxtDiaChi, diaChi);
            ClearAndEnter(TxtEmail, email);
            ClearAndEnter(TxtChucVu, chucVu);
            ClearAndEnter(TxtGioLamViec, gioLamViec);
            ClearAndEnter(TxtBangCap, bangCap);
            ClearAndEnter(TxtKinhNghiem, kinhNghiem);
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

            //đóng form edit nếu form còn mở (có nút hủy)
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
