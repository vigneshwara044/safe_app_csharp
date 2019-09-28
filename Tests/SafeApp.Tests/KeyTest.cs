using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.API;
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
            var (_, keyPairSender) = await apiSender.KeysCreatePreloadTestCoinsAsync("10");

            var sessionRecipient = await GetSessionAsync();
            var apiRecipient = sessionRecipient.Keys;
            var (_, keyPairRecipient) = await apiRecipient.KeysCreatePreloadTestCoinsAsync("10");

            var (xorUrl, newKeyPair) = await apiSender.CreateKeysAsync(
                keyPairSender.SK,
                "1",
                null);

            await ValidateResults(xorUrl, newKeyPair, apiSender);
        }

        Task<Session> GetSessionAsync()
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

            var (xorUrl, keyPair) = await api.KeysCreatePreloadTestCoinsAsync("1");

            await ValidateResults(xorUrl, keyPair, api);
        }

        [Test]
        public async Task KeysBalanceFromSkTest()
        {
            var session = await TestUtils.CreateTestApp();
            var api = session.Keys;
            var (xorurl, keyPair) = await api.KeysCreatePreloadTestCoinsAsync("1");

            var balance = await api.KeysBalanceFromSkAsync(keyPair.SK);
        }

        [Test]
        public async Task KeysBalanceFromUrlTest()
        {
            var session = await TestUtils.CreateTestApp();
            var api = session.Keys;
            var (xorurl, keyPair) = await api.KeysCreatePreloadTestCoinsAsync("1");

            var balance = await api.KeysBalanceFromUrlAsync(xorurl, keyPair.SK);
        }

        [Test]
        public async Task ValidateSkForUrlTest()
        {
            var session = await TestUtils.CreateTestApp();
            var api = session.Keys;
            var (xorurl, keyPair) = await api.KeysCreatePreloadTestCoinsAsync("1");

            var publicKey = await api.ValidateSkForUrlAsync(keyPair.SK, xorurl);
        }

        [Test]
        public async Task KeysTransferTest()
        {
            var amountToSend = "1";
            var txId = 1234UL;

            var sessionSender = await GetSessionAsync();
            var apiSender = sessionSender.Keys;
            var (_, keyPairSender) = await apiSender.KeysCreatePreloadTestCoinsAsync(amountToSend);

            var sessionRecipient = await GetSessionAsync();
            var apiRecipient = sessionRecipient.Keys;
            var (recipientUrl, keyPairRecipient) = await apiRecipient.KeysCreatePreloadTestCoinsAsync("0.1");

            var resultTxId = await apiSender.KeysTransferAsync("1", keyPairSender.SK, recipientUrl, txId);

            Assert.AreEqual(txId, resultTxId);
        }

        async Task ValidateResults(string xorUrl, BlsKeyPair keyPair, Keys api)
        {
            Assert.IsNotNull(xorUrl);
            Assert.IsNotNull(keyPair.PK);
            Assert.IsNotNull(keyPair.SK);
            Assert.AreNotSame(string.Empty, keyPair.PK);
            Assert.AreNotSame(string.Empty, keyPair.SK);

            var publicKey = await api.ValidateSkForUrlAsync(keyPair.SK, xorUrl);

            Assert.AreEqual(keyPair.PK, publicKey);
        }
    }
}
