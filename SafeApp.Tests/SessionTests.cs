using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.MockAuthBindings;

namespace SafeApp.Tests
{
    [TestFixture]
    internal class SessionTests
    {
        [Test]
        public async Task NetworkDisconnectAsync()
        {
            var session = await Utils.CreateTestApp();
            Assert.False(session.IsDisconnected);
            TestUtils.TestSimulateNetworkDisconnectAsync();
        }
    }
}
