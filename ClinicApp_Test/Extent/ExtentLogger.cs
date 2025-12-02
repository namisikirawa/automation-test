using AventStack.ExtentReports.MarkupUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicApp_Test.Extent
{
    public static class ExtentLogger
    {
        public static void pass(ExtentTest test, string message)
        {
            test.Pass(message);
        }
        public static void fail(ExtentTest test, string message)
        {
            test.Fail(message);
        }
        public static void info(ExtentTest test, string message)
        {
            test.Info(message);
        }


        public static void passHighlight(ExtentTest test, string message)
        {
            test.Pass(MarkupHelper.CreateLabel(message, ExtentColor.Green));
        }
        public static void failHighlight(ExtentTest test, string message)
        {
            test.Fail(MarkupHelper.CreateLabel(message, ExtentColor.Red));
        }
        /*-----------------------Screenshot Methods*/
        public static string CaptureScreenshot(AutomationElement element, string fileNamePrefix = "Screenshot")
        {
            try
            {
                var bmp = element.Capture();
                string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Screenshots");
                Directory.CreateDirectory(dir);
                string filePath = Path.Combine(dir, $"{fileNamePrefix}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                bmp.Save(filePath, ImageFormat.Png);
                return filePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Capture screenshot failed: {ex.Message}");
                return null;
            }
        }

        public static void AttachScreenshot(ExtentTest test, string screenshotPath, string message = "")
        {
            if (!string.IsNullOrEmpty(screenshotPath) && File.Exists(screenshotPath))
            {
                test.AddScreenCaptureFromPath(screenshotPath, message);
            }
        }
    }
}
