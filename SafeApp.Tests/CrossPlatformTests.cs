using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.MockAuthBindings;

namespace SafeApp.Tests {
  [TestFixture]
  internal class CrossPlatformTests {
    [SetUp]
    public void Setup() { }

    [TearDown]
    public void Tear() { }

    [Test]
    public async Task RandomAppCreate() {
      var session = new Session(MockAuthResolver.Current.TestCreateApp());
      using (await session.MDataEntryActions.NewAsync()) { }

      session.Dispose();
    }
  }
}
