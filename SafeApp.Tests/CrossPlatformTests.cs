using System.Threading.Tasks;
using NUnit.Framework;

namespace SafeApp.Tests {
  [TestFixture]
  internal class CrossPlatformTests {
    [SetUp]
    public void Setup() { }

    [TearDown]
    public void Tear() { }

    [Test]
    public async Task RandomAppCreate() {
      var session = Utils.RandomSession();
      using (await session.MDataEntryActions.NewAsync()) { }

      session.Dispose();
    }
  }
}
