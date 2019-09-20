using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.Core;

namespace SafeApp.Tests
{
    [TestFixture]
    internal class KeyTest
    {
        private static AuthReq _authReq = new AuthReq
        {
            App = new AppExchangeInfo { Id = "net.maidsafe.test", Name = "TestApp", Scope = null, Vendor = "MaidSafe.net Ltd." },
            AppContainer = true,
            Containers = new List<ContainerPermissions>()
        };

        [Test]
        public async Task KeyBalance()
        {
            var session = await TestUtils.CreateTestAppForExisting("AHX4IPAT9S", "AHX4IPAT9S", _authReq);
            var api = session.Keys;
            var balance = await api.KeysBalanceFromSkAsync("ef8ad4147ff5151605e782ad155b1d5faa15e77f4d375ae0f5893362cfaaeb60");
        }

        [Test]
        public async Task CreateAccount()
        {
            var locSec = TestUtils.GetRandomString(10);
            var session = await TestUtils.CreateTestApp(locSec, locSec, new AuthReq
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
        }

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
