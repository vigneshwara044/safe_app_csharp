using System;
using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.MData;
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
      await session.MDataEntryActions.NewAsync();
    }
  }
}
