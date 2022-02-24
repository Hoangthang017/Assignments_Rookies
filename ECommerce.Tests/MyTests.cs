using ECommerce.Tests;
using Xunit;

namespace Tests
{
    public class MyTests : IClassFixture<InjectionFixture>
    {
        private readonly InjectionFixture injection;

        public MyTests(InjectionFixture injection)
        {
            this.injection = injection;
        }

        [Fact]
        public void SomeTest()
        {
            // TODO: add test code.
        }
    }
}