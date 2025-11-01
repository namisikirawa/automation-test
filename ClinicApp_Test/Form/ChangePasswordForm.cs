
namespace ClinicApp_Test.Form
{
    public class ChangePasswordForm
    {
        private readonly Window _mainWindow;
        private readonly UIA3Automation _automation;

        public ChangePasswordForm(Window mainWindow, UIA3Automation automation)
        {
            _mainWindow = mainWindow ?? throw new ArgumentNullException(nameof(mainWindow));
            _automation = automation ?? throw new ArgumentNullException(nameof(automation));
        }

        private AutomationElement AccountPanel =>
            _mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("AccountManagement"));

        private AutomationElement GroupBox =>
            AccountPanel?.FindFirstDescendant(cf => cf.ByAutomationId("groupBox1"));

        private TextBox OldPasswordTextBox =>
            GroupBox?.FindFirstDescendant(cf => cf.ByAutomationId("txtOldPassword"))?.AsTextBox();

        private TextBox NewPasswordTextBox =>
            GroupBox?.FindFirstDescendant(cf => cf.ByAutomationId("txtNewPassword"))?.AsTextBox();

        private Button ChangePasswordButton =>
            GroupBox?.FindFirstDescendant(cf => cf.ByAutomationId("btn_ChangePassword"))?.AsButton();

        public void EnterOldPassword(string oldPass)
        {
            OldPasswordTextBox.Text = oldPass;
        }

        public void EnterNewPassword(string newPass)
        {
            NewPasswordTextBox.Text = newPass;
        }

        public void ClickChangePassword()
        {
            ChangePasswordButton?.Click();
        }

        public string GetMessageBoxTextAndClose(int processId)
        {
            var desktop = _automation.GetDesktop();
            var msgBox = desktop.FindFirstChild(cf => cf.ByProcessId(processId)
                                                       .And(cf.ByControlType(ControlType.Window)))?.AsWindow();

            var label = msgBox?.FindFirstDescendant(cf => cf.ByControlType(ControlType.Text))?.AsLabel();
            var message = label?.Text;

            var okButton = msgBox?.FindFirstDescendant(cf => cf.ByAutomationId("2"))?.AsButton();
            okButton?.Click();

            Thread.Sleep(1000);
            return message;
        }
    }
}
