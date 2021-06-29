using NUnit.Framework;
using System.Collections;
using TechTalk.SpecFlow;

namespace CSharpNUnitSpecFlowWebDriver.Utilities
{
    [Binding, Parallelizable]
    class CrossBrowserData
    {
        public static IEnumerable LatestConfigurations
        {
            get
            {
                yield return new TestFixtureData("Chrome");
                yield return new TestFixtureData("Firefox");
                yield return new TestFixtureData("Edge");
            }
        }

      
    }

}
