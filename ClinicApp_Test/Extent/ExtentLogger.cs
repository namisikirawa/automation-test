using AventStack.ExtentReports.MarkupUtils;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicApp_Test.Extent
{
    public static class ExtentLogger
    {
        public static void Success(ExtentTest test, string message) => test.Pass(message);
        public static void Error(ExtentTest test, string message) => test.Fail(message);
        public static void Info(ExtentTest test, string message)
        {
            test.Info(message);
        }
        public static void AttachScreenshotBase64(ExtentTest test)
        {
            try
            {
                // Lấy kích thước màn hình chính
                int width = 1920; // hoặc chỉnh theo màn hình test
                int height = 1080;

                using var bmp = new Bitmap(width, height);
                using var g = Graphics.FromImage(bmp);
                g.CopyFromScreen(0, 0, 0, 0, bmp.Size);

                using var ms = new MemoryStream();
                bmp.Save(ms, ImageFormat.Png);
                string base64 = Convert.ToBase64String(ms.ToArray());

                test.Info("Screenshot:", MediaEntityBuilder.CreateScreenCaptureFromBase64String(base64).Build());
            }
            catch
            {
                test.Warning("Không chụp được screenshot.");
            }
        }

        public static void Pass(ExtentTest test, string message)
        {
            test.Pass(MarkupHelper.CreateLabel(message, ExtentColor.Green));
            AttachScreenshotBase64(test);
        }
        public static void PassWithoutScreenshot(ExtentTest test, string message)
        {
            test.Pass(MarkupHelper.CreateLabel(message, ExtentColor.Green));
        }
        public static void Fail(ExtentTest test, string message)
        {
            test.Fail(MarkupHelper.CreateLabel(message, ExtentColor.Red));
            AttachScreenshotBase64(test);
        }
        public static void FailWithoutScreenshot(ExtentTest test, string message)
        {
            test.Fail(MarkupHelper.CreateLabel(message, ExtentColor.Red));
        }
        
    }
}
