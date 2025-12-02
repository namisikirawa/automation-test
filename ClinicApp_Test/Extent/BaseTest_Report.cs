using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicApp_Test.Extent
{
    public class BaseTest_Report : BaseTest
    {
        [TestInitialize]
        public void ReportInit()
        {
            _extent = ExtentManager.GetInstance();
        }

        [TestCleanup]
        public void ReportCleanup()
        {
            ExtentManager.Flush();
        }
        public TestContext TestContext { get; set; }
    }
}
