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

        [Test]
        public async Task CreateKeysTest()
        {
            var sessionSender = await GetSessionAsync();
            var apiSender = sessionSender.Keys;
            var keyPairSender = await apiSender.GenerateKeyPairAsync();

            var sessionRecipient = await GetSessionAsync();
            var apiRecipient = sessionRecipient.Keys;
            var keyPairRecipient = await apiRecipient.GenerateKeyPairAsync();

            var (xorUrl, newKeyPair) = await apiSender.CreateKeysAsync(
                keyPairSender.SK,
                "1",
                keyPairRecipient.PK);
        }

        Task<NewSession> GetSessionAsync()
            => TestUtils.CreateTestApp(new AuthReq
            {
                App = new AppExchangeInfo
                {
                    Id = "net.maidsafe.test",
                    Name = "TestApp",
                    Scope = null,
                    Vendor = "MaidSafe.net Ltd."
                },
                AppContainer = true,
                Containers = new List<ContainerPermissions>()
            });

        [Test]
        public async Task KeysCreatePreloadTestCoinsTest()
        {
            var session = await TestUtils.CreateTestApp();
            var api = session.Keys;

            var (xorurl, keyPair) = await api.KeysCreatePreloadTestCoins("1");
        }

        [Test]
        public async Task KeysBalanceFromSkTest()
        {
            var session = await TestUtils.CreateTestApp();
            var api = session.Keys;
            var (xorurl, keyPair) = await api.KeysCreatePreloadTestCoins("1");

            var balance = await api.KeysBalanceFromSkAsync(keyPair.SK);
        }
    }
}
