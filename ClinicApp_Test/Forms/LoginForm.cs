
namespace ClinicApp_Test.Form
{
    public class LoginForm
    {
        private readonly Window _window;

        public LoginForm(Window window)
        {
            _window = window ?? throw new ArgumentNullException(nameof(window));
        }

        public TextBox UsernameTextBox => _window.FindFirstDescendant(cf => cf.ByAutomationId("txtTaikhoan"))?.AsTextBox();
        public TextBox PasswordTextBox => _window.FindFirstDescendant(cf => cf.ByAutomationId("txtMatkhau"))?.AsTextBox();
        public Button LoginButton => _window.FindFirstDescendant(cf => cf.ByAutomationId("btn_login"))?.AsButton();

        public void EnterUsername(string username) => UsernameTextBox.Text = username;
        public void EnterPassword(string password) => PasswordTextBox.Text = password;

        public void ClickLogin() => LoginButton.Click();

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

        public void LogoutIfLoggedIn(AutomationBase automation, Application app)
        {
            Keyboard.Press(VirtualKeyShort.LMENU);
            Keyboard.Press(VirtualKeyShort.F4);
            Keyboard.Release(VirtualKeyShort.F4);
            Keyboard.Release(VirtualKeyShort.LMENU);
            Thread.Sleep(1000);

            var desktop = automation.GetDesktop();
            var logoutMsgBox = desktop.FindFirstChild(cf => cf.ByProcessId(app.ProcessId)
                                                  .And(cf.ByControlType(ControlType.Window)));
            var btnYes = logoutMsgBox?.FindFirstDescendant(cf => cf.ByText("Yes"))?.AsButton();
            btnYes?.Click();
        }
    }
}
