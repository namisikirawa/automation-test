using System;
using System.IO;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;

public class Program
{
    static void Main()
    {
        // Lưu report trên Desktop
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "extentReport.html");
        Directory.CreateDirectory(Path.GetDirectoryName(path));

        var htmlReporter = new ExtentHtmlReporter(path);
        var extent = new ExtentReports();
        extent.AttachReporter(htmlReporter);

        var test = extent.CreateTest("Demo Test");
        test.Pass("Test passed");

        extent.Flush();

        // Mở file nếu tồn tại
        if (File.Exists(path))
        {
            Console.WriteLine("Report tồn tại tại: " + path);
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = path,
                UseShellExecute = true
            });
        }
        else
        {
            Console.WriteLine("Report chưa tạo được: " + path);
        }

        Console.WriteLine("Nhấn Enter để thoát...");
        Console.ReadLine();
    }
}