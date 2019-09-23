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
            AppPermissionTransferCoins = true,
            Containers = new List<ContainerPermissions>()
        };

        [SetUp]
        public async Task Init()
        {
            await InitLogging();
        }

        /**
        [Test]
        public async Task CreateAccount()
        {
            var locSec = TestUtils.GetRandomString(10);
            var session = await TestUtils.CreateTestApp(locSec, locSec, _authReq);
        }

        [Test]
        public async Task TryKeyBalanceWithExistingAccount()
        {
            var session = await TestUtils.CreateTestAppForExisting("D4GL6N9QR4", "D4GL6N9QR4", _authReq);
            var api = session.Keys;
            var balance = await api.KeysBalanceFromSkAsync("61223e3275b57a467897a77f73ee67b956db8f757f3d278158c27e92b43e3b1e");
        }
        **/

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
            var (xorurl1, keyPairSender) = await apiSender.KeysCreatePreloadTestCoins("1");

            var sessionRecipient = await GetSessionAsync();
            var apiRecipient = sessionRecipient.Keys;

            var (xorurl2, keyPairRecipient) = await apiRecipient.KeysCreatePreloadTestCoins("0.1");

            var (xorUrl, newKeyPair) = await apiSender.CreateKeysAsync(
                keyPairSender.SK,
                "1",
                keyPairRecipient.PK);
        }

        Task<NewSession> GetSessionAsync()
            => TestUtils.CreateTestApp(_authReq);

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

        async Task InitLogging()
        {
            await Session.SetAdditionalSearchPathAsync(@"C:\Users\oetyng\source\repos\safe_app_csharp\Tests\SafeApp.Tests.Framework\bin\x64\Debug");
            await Session.InitLoggingAsync();
        }
    }
}
