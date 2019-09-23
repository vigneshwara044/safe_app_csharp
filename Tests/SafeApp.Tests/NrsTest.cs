using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.API;
using SafeApp.Core;

namespace SafeApp.Tests
{
    [TestFixture]
    internal class NrsTest
    {
        [Test]
        public async Task ParseUrlTest()
        {
            var session = await TestUtils.CreateTestApp();
            var (xorurl, _) = await session.Keys.KeysCreatePreloadTestCoinsAsync("1");

            var xorUrlEncoder = await Nrs.ParseUrlAsync(xorurl);
        }

        [Test]
        public async Task ParseAndResolveUrlTest()
        {
            var session = await TestUtils.CreateTestApp();
            var (xorurl, _) = await session.Keys.KeysCreatePreloadTestCoinsAsync("1");

            var api = session.Nrs;
            var (xorUrlEncoder, resolvesAsNrs) = await api.ParseAndResolveUrlAsync(xorurl);
        }
    }
}
