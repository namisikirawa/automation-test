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

        //điều hướng đến form "Đổi mật khẩu" (ko có tổ hợp phím)
        public void OpenChangePasswordForm()
        {
            var menuStrip = _mainWindow.FindFirstDescendant(cf => cf.ByControlType(ControlType.MenuBar));
            var systemMenu = menuStrip?.FindFirstDescendant(cf => cf.ByName("Hệ thống"))?.AsMenuItem();

            if (systemMenu == null)
                throw new Exception("Không tìm thấy menu 'Hệ thống'.");

            systemMenu.Click();
            Thread.Sleep(500);

            var accountMenu = systemMenu.FindFirstDescendant(cf => cf.ByName("Quản lý tài khoản"))?.AsMenuItem();

            if (accountMenu == null)
                throw new Exception("Không tìm thấy menu 'Quản lý tài khoản'.");

            accountMenu.Click();
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
        public void OpenSuppliestManagement()
        {
            Keyboard.Press(VirtualKeyShort.LMENU);
            Keyboard.Press(VirtualKeyShort.KEY_V);
            Keyboard.Release(VirtualKeyShort.KEY_V);
            Keyboard.Release(VirtualKeyShort.LMENU);
            Thread.Sleep(1000);
        }
    }
}
