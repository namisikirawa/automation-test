using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicApp_Test.Extent
{
    public class BaseTest
    {
        protected static ExtentReports _extent;

        [ClassInitialize(InheritanceBehavior.BeforeEachDerivedClass)]
        public static void InitClass(TestContext context)
        {
            if (_extent == null)
                _extent = ExtentManager.GetInstance();
        }
    }
}
