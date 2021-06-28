using NUnit.Framework;

namespace CSharpNUnitSpecFlowWebDriver
{
    [SetUpFixture]
    public static class NamespaceSetup
    {
        [OneTimeSetUp]
        public static void ExecuteForCreatingReportsNamespace()
        {
            Reporter.StartReporter();
        }
    }
}
