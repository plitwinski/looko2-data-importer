using Moq;

namespace LookO2.Importer.Tests.Fixtures
{
    public abstract class BaseFixture<TMock>
        where TMock : class
    {
        public Mock<TMock> Mock { get; }

        public BaseFixture(bool callUnderylingMethods = false)
        {
            Mock = new Mock<TMock>() { CallBase = callUnderylingMethods };
        }

        public TMock Create()
          => Mock.Object;
    }
}
