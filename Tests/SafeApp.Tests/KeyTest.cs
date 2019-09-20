using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.Core;

namespace SafeApp.Tests
{
    [TestFixture]
    internal class KeyTest
    {
        [Test]
        public async Task GenerateKeyPairTest()
        {
            var session = await TestUtils.CreateTestApp();
            var api = session.Keys;
            var keyPair = await api.GenerateKeyPairAsync();
        }
    }
}
