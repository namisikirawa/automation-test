using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ClinicApp_Test.Extent
{
    public static class ExtentManager
    {
        private static ExtentReports _extent;
        public static ExtentReports GetInstance()
        {
            if (_extent == null)
            {
                string reportDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");
                Directory.CreateDirectory(reportDir);
                string reportPath = Path.Combine(reportDir, "ClinicManagementReport.html");

                var spark = new ExtentSparkReporter(reportPath);

                //customize report
                spark.Config.DocumentTitle = "Clinic Management App Automation Test Report";
                spark.Config.ReportName = "Clinic Management App Test Report";
                spark.Config.Theme = AventStack.ExtentReports.Reporter.Config.Theme.Dark;
                spark.Config.Encoding = "utf-8";
                spark.Config.TimeStampFormat = "HH:mm:ss";

                _extent = new ExtentReports();
                _extent.AttachReporter(spark);
            }

            return _extent;
        }

        public static void Flush()
        {
            _extent?.Flush();
        }
    }
}
