using NUnit.Framework;

namespace LookO2.Importer.Tests.Fixtures
{
    public abstract class GivenWhen
    {
        [OneTimeSetUp]
        public void SetupAsync()
        {
            Given();
            When();
        }

        public virtual void Given() { }

        public abstract void When();
    }
}
