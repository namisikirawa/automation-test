using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicApp_Test.Form
{
    //trang chủ hiển thị sau khi đăng nhập, có thể điều hướng sang các trang khác
    public class MainForm
    {
        private readonly Window _mainWindow;
        private readonly UIA3Automation _automation;

        public MainForm(Window mainWindow, UIA3Automation automation)
        {
            _mainWindow = mainWindow ?? throw new ArgumentNullException(nameof(mainWindow));
            _automation = automation ?? throw new ArgumentNullException(nameof(automation));
        }

        //điều hướng đến form "Đổi mật khẩu"
        public void OpenChangePasswordForm()
        {
            Keyboard.Press(VirtualKeyShort.LMENU);
            Keyboard.Press(VirtualKeyShort.KEY_D);
            Keyboard.Release(VirtualKeyShort.KEY_D);
            Keyboard.Release(VirtualKeyShort.LMENU);
            Thread.Sleep(1000);
        }

        //điều hướng đến form “Quản lý nhân viên"
        public void OpenEmployeeManagement()
        {
            Keyboard.Press(VirtualKeyShort.LMENU);
            Keyboard.Press(VirtualKeyShort.KEY_N);
            Keyboard.Release(VirtualKeyShort.KEY_N);
            Keyboard.Release(VirtualKeyShort.LMENU);
            Thread.Sleep(1000);
        }
        //điều hướng đến form “Quản lý bệnh nhân"
        public void OpenPaitientManagement()
        {
            Keyboard.Press(VirtualKeyShort.LMENU);
            Keyboard.Press(VirtualKeyShort.KEY_B);
            Keyboard.Release(VirtualKeyShort.KEY_B);
            Keyboard.Release(VirtualKeyShort.LMENU);
            Thread.Sleep(1000);
        }

        //điều hướng đến form “Quản lý vật tư"
        public void OpenSuppliesManagement()
        {
            Keyboard.Press(VirtualKeyShort.LMENU);
            Keyboard.Press(VirtualKeyShort.KEY_V);
            Keyboard.Release(VirtualKeyShort.KEY_V);
            Keyboard.Release(VirtualKeyShort.LMENU);
            Thread.Sleep(1000);
        }

        //logout về form đăng nhập
        public void Logout(UIA3Automation automation, int processId)
        {
            // Nhấn Alt + F4
            Keyboard.Press(VirtualKeyShort.LMENU);
            Keyboard.Press(VirtualKeyShort.F4);
            Keyboard.Release(VirtualKeyShort.F4);
            Keyboard.Release(VirtualKeyShort.LMENU);

            Thread.Sleep(500);

            //chọn yes trên hộp thoại xác nhận
            var desktop = automation.GetDesktop();
            var dialog = desktop.FindFirstChild(cf => cf.ByProcessId(processId)
                .And(cf.ByControlType(ControlType.Window)))?.AsWindow();
            var yesButton = dialog?.FindFirstDescendant(cf => cf.ByText("Yes"))?.AsButton();
            yesButton?.Click();
            Thread.Sleep(1000);
        }
    }
}
