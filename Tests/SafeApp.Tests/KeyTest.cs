using System.Threading.Tasks;
using NUnit.Framework;

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
            Validate.TransientKeyPair(keyPair);
        }

        [Test]
        public async Task KeysCreatePreloadTestCoinsTest()
        {
            var session = await TestUtils.CreateTestApp();
            var api = session.Keys;
            var (xorUrl, keyPair) = await api.KeysCreatePreloadTestCoinsAsync("1");
            await Validate.PersistedKeyPair(xorUrl, keyPair, api);
        }

        [Test]
        public async Task CreateKeysFromTransientTest()
        {
            var session = await TestUtils.CreateTestApp();
            var api = session.Keys;
            var initialAmount = "2";
            var transferAmount = "1";
            var (_, keyPairSender) = await api.KeysCreatePreloadTestCoinsAsync(initialAmount);

            // transient keys, not persisted on the network
            var keyPairRecipient = await api.GenerateKeyPairAsync();

            // we expect keyPairRecipient to persisted with
            // this call, and newKeyPair to be empty.
            var (xorUrl, newKeyPair) = await api.CreateKeysAsync(
                keyPairSender.SK,
                transferAmount,
                keyPairRecipient.PK);

            Assert.IsNull(newKeyPair.PK);
            Assert.IsNull(newKeyPair.SK);

            await Validate.PersistedKeyPair(xorUrl, keyPairRecipient, api);

            var senderBalance = await api.KeysBalanceFromSkAsync(newKeyPair.SK);
            Validate.IsEqualAmount(transferAmount, senderBalance);

            var recipientBalance = await api.KeysBalanceFromSkAsync(newKeyPair.SK);
            Validate.IsEqualAmount(transferAmount, recipientBalance);
        }

        [Test]
        public async Task CreateKeysTest()
        {
            var session = await TestUtils.CreateTestApp();
            var api = session.Keys;
            var initialAmount = "2";
            var transferAmount = "1";
            var (_, keyPairSender) = await api.KeysCreatePreloadTestCoinsAsync(initialAmount);

            var (xorUrl, newKeyPair) = await api.CreateKeysAsync(
                keyPairSender.SK,
                transferAmount,
                null);

            await Validate.PersistedKeyPair(xorUrl, newKeyPair, api);

            var senderBalance = await api.KeysBalanceFromSkAsync(newKeyPair.SK);
            Validate.IsEqualAmount(transferAmount, senderBalance);

            var recipientBalance = await api.KeysBalanceFromSkAsync(newKeyPair.SK);
            Validate.IsEqualAmount(transferAmount, recipientBalance);
        }

        [Test]
        public async Task KeysBalanceFromSkTest()
        {
            var session = await TestUtils.CreateTestApp();
            var api = session.Keys;
            var amount = "1";
            var (xorurl, keyPair) = await api.KeysCreatePreloadTestCoinsAsync(amount);
            var balance = await api.KeysBalanceFromSkAsync(keyPair.SK);
            Validate.IsEqualAmount(amount, balance);
        }

        [Test]
        public async Task KeysBalanceFromUrlTest()
        {
            var session = await TestUtils.CreateTestApp();
            var api = session.Keys;
            var amount = "1";
            var (xorurl, keyPair) = await api.KeysCreatePreloadTestCoinsAsync(amount);
            var balance = await api.KeysBalanceFromUrlAsync(xorurl, keyPair.SK);
            Validate.IsEqualAmount(amount, balance);
        }

        [Test]
        public async Task ValidateSkForUrlTest()
        {
            var session = await TestUtils.CreateTestApp();
            var api = session.Keys;
            var (xorurl, keyPair) = await api.KeysCreatePreloadTestCoinsAsync("1");
            var publicKey = await api.ValidateSkForUrlAsync(keyPair.SK, xorurl);
            Assert.AreEqual(keyPair.PK, publicKey);
        }

        [Test]
        public async Task KeysTransferTest()
        {
            var amountToSend = "1";
            var initialRecipientBalance = "0.1";
            var expectedRecipientEndBalance = "1.1";
            var expectedSenderEndBalance = "0";

            var txId = 1234UL;

            var session = await TestUtils.CreateTestApp();
            var api = session.Keys;

            var (senderUrl, keyPairSender) = await api.KeysCreatePreloadTestCoinsAsync(amountToSend);
            var (recipientUrl, keyPairRecipient) = await api.KeysCreatePreloadTestCoinsAsync(initialRecipientBalance);
            var resultTxId = await api.KeysTransferAsync(amountToSend, keyPairSender.SK, recipientUrl, txId);

            Assert.AreEqual(txId, resultTxId);

            var senderBalance = await api.KeysBalanceFromUrlAsync(senderUrl, keyPairSender.SK);
            Validate.IsEqualAmount(expectedSenderEndBalance, senderBalance);

            var recipientBalance = await api.KeysBalanceFromUrlAsync(recipientUrl, keyPairRecipient.SK);
            Validate.IsEqualAmount(expectedRecipientEndBalance, recipientBalance);
        }
    }
}
