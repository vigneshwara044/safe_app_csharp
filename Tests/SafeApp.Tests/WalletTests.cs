using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.Core;

namespace SafeApp.Tests
{
    [TestFixture]
    internal class WalletTest
    {
        private const string NAME1 = "TestBalance1";
        private const string NAME2 = "TestBalance2";

        [Test]
        public async Task CreateWalletTest()
        {
            var (_, api, _) = await GetAPIsAsync();

            var wallet = await api.WalletCreateAsync();
            var balance = await api.WalletBalanceAsync(wallet);
            Validate.IsEqualAmount("0.0", balance);
        }

        [Test]
        public async Task InsertAndBalanceTest()
        {
            var (_, api, keysApi) = await GetAPIsAsync();

            var walletXorUrl = await api.WalletCreateAsync();
            var keyPair_1_Balance = "123";
            var keyPair_2_Balance = "321";
            var expectedEndBalance = "444";
            var (_, keyPair1) = await keysApi.KeysCreatePreloadTestCoinsAsync(keyPair_1_Balance);
            var (_, keyPair2) = await keysApi.KeysCreatePreloadTestCoinsAsync(keyPair_2_Balance);
            await api.WalletInsertAsync(walletXorUrl, NAME1, setDefault: true, keyPair1.SK);

            var currentBalance = await api.WalletBalanceAsync(walletXorUrl);
            Validate.IsEqualAmount(keyPair_1_Balance, currentBalance);

            await api.WalletInsertAsync(walletXorUrl, "TestBalance2", setDefault: false, keyPair2.SK);
            currentBalance = await api.WalletBalanceAsync(walletXorUrl);
            Validate.IsEqualAmount(expectedEndBalance, currentBalance);
        }

        [Test]
        public async Task InsertAndGetTest()
        {
            var (session, api, keysApi) = await GetAPIsAsync();

            var walletXorUrl = await api.WalletCreateAsync();
            var (xorUrl1, keyPair1) = await keysApi.KeysCreatePreloadTestCoinsAsync("123");
            var (xorUrl2, keyPair2) = await keysApi.KeysCreatePreloadTestCoinsAsync("321");

            await api.WalletInsertAsync(walletXorUrl, NAME1, setDefault: true, keyPair1.SK);
            await api.WalletInsertAsync(walletXorUrl, NAME2, setDefault: false, keyPair2.SK);

            var walletBalances = await api.WalletGetAsync(walletXorUrl);
            Assert.IsTrue(walletBalances.WalletBalances.Find(q => q.WalletName.Equals(NAME1)).IsDefault);
            Assert.AreEqual(Find(walletBalances, NAME1).XorUrl, xorUrl1);
            Assert.AreEqual(Find(walletBalances, NAME1).Sk, keyPair1.SK);

            Assert.IsFalse(walletBalances.WalletBalances.Find(q => q.WalletName.Equals(NAME2)).IsDefault);
            Assert.AreEqual(Find(walletBalances, NAME2).XorUrl, xorUrl2);
            Assert.AreEqual(Find(walletBalances, NAME2).Sk, keyPair2.SK);
        }

        [Test]
        public async Task WalletInsertAndSetDefaultTest()
        {
            var (session, api, keysApi) = await GetAPIsAsync();

            var walletXorUrl = await api.WalletCreateAsync();
            var (xorUrl1, keyPair1) = await keysApi.KeysCreatePreloadTestCoinsAsync("123");
            var (xorUrl2, keyPair2) = await keysApi.KeysCreatePreloadTestCoinsAsync("321");

            await api.WalletInsertAsync(walletXorUrl, NAME1, setDefault: true, keyPair1.SK);
            await api.WalletInsertAsync(walletXorUrl, NAME2, setDefault: true, keyPair2.SK);
            var walletBalances = await api.WalletGetAsync(walletXorUrl);

            Assert.IsFalse(walletBalances.WalletBalances.Find(q => q.WalletName.Equals(NAME1)).IsDefault);
            Assert.AreEqual(Find(walletBalances, NAME1).XorUrl, xorUrl1);
            Assert.AreEqual(Find(walletBalances, NAME1).Sk, keyPair1.SK);

            Assert.IsTrue(walletBalances.WalletBalances.Find(q => q.WalletName.Equals(NAME2)).IsDefault);
            Assert.AreEqual(Find(walletBalances, NAME2).XorUrl, xorUrl2);
            Assert.AreEqual(Find(walletBalances, NAME2).Sk, keyPair2.SK);
        }

        WalletSpendableBalance Find(WalletSpendableBalances list, string name)
            => list.WalletBalances.Find(q => q.WalletName.Equals(name)).Balance;

        [Test]
        public async Task TransferWithoutDefaultBalanceTest()
        {
            var (session, api, keysApi) = await GetAPIsAsync();

            // without default balance
            var fromWalletXorUrl = await api.WalletCreateAsync();

            // with default balance
            var toWalletXorUrl = await api.WalletCreateAsync();
            var (_, keyPair) = await keysApi.KeysCreatePreloadTestCoinsAsync("123");
            await api.WalletInsertAsync(toWalletXorUrl, "TestBalance", setDefault: true, keyPair.SK);

            AssertThrows(-203, () => api.WalletTransferAsync(fromWalletXorUrl, toWalletXorUrl, "123", 0));
            AssertThrows(-203, () => api.WalletTransferAsync(toWalletXorUrl, fromWalletXorUrl, "123", 0));
        }

        [Test]
        public async Task TransferFromZeroBalanceTest()
        {
            var (session, api, keysApi) = await GetAPIsAsync();

            var fromWalletXorUrl = await api.WalletCreateAsync();
            var (_, keyPair1) = await keysApi.KeysCreatePreloadTestCoinsAsync("0.0");
            await api.WalletInsertAsync(fromWalletXorUrl, "TestBalance", setDefault: true, keyPair1.SK);

            var (toXorUrl, _) = await keysApi.KeysCreatePreloadTestCoinsAsync("0.5");

            AssertThrows(-301, () => api.WalletTransferAsync(fromWalletXorUrl, toXorUrl, "0.5", 0));

            var toWalletXorUrl = await api.WalletCreateAsync();
            var (xorurl2, keypair2) = await keysApi.KeysCreatePreloadTestCoinsAsync("0.5");
            await api.WalletInsertAsync(toWalletXorUrl, "NewTestBalance", setDefault: true, keypair2.SK);

            AssertThrows(-300, () => api.WalletTransferAsync(fromWalletXorUrl, toWalletXorUrl, "0", 0));
        }

        [Test]
        public async Task TransferDifferentAmountsTest()
        {
            var (session, api, keysApi) = await GetAPIsAsync();

            var fromWalletXorUrl = await api.WalletCreateAsync();
            var (_, keyPair1) = await keysApi.KeysCreatePreloadTestCoinsAsync("123.321");
            await api.WalletInsertAsync(fromWalletXorUrl, NAME1, setDefault: true, keyPair1.SK);

            var (toXorUrl, keypair2) = await keysApi.KeysCreatePreloadTestCoinsAsync("0.5");
            await api.WalletInsertAsync(fromWalletXorUrl, "TestBalance2", setDefault: false, keypair2.SK);

            // transfering amount more than current balance
            AssertThrows(-301, () => api.WalletTransferAsync(fromWalletXorUrl, toXorUrl, "321.123", 0));

            // transfering invalid amount
            AssertThrows(-300, () => api.WalletTransferAsync(fromWalletXorUrl, toXorUrl, "0.dgnda", 0));

            // valid transfer
            await api.WalletTransferAsync(fromWalletXorUrl, toXorUrl, "10", 0);
        }

        [Test]
        public async Task TransferToSafeKeyTest()
        {
            var (_, api, keysApi) = await GetAPIsAsync();

            var fromWalletXorUrl = await api.WalletCreateAsync();
            var (_, keyPair1) = await keysApi.KeysCreatePreloadTestCoinsAsync("123.321");
            await api.WalletInsertAsync(fromWalletXorUrl, "TestBalance", setDefault: true, keyPair1.SK);

            var (xorUrl2, _) = await keysApi.KeysCreatePreloadTestCoinsAsync("10.0");

            // transfer from wallet to key
            await api.WalletTransferAsync(fromWalletXorUrl, xorUrl2, "100.001", 0);
        }

        [Test]
        public async Task TransferFromSafeKeyTest()
        {
            var (session, api, keysApi) = await GetAPIsAsync();

            var (safekeyXorUrl1, _) = await keysApi.KeysCreatePreloadTestCoinsAsync("10");
            var (safekeyXorUrl2, _) = await keysApi.KeysCreatePreloadTestCoinsAsync("0");

            AssertThrows(-207, () => api.WalletTransferAsync(safekeyXorUrl1, safekeyXorUrl2, "5", 0));
        }

        [Test]
        public async Task TransferFromUnownedWalletTest()
        {
            var (_, api1, keysApi1) = await GetAPIsAsync();

            var account1WalletXORURL = await api1.WalletCreateAsync();
            var (keyXorUrl1, keyPair1) = await keysApi1.KeysCreatePreloadTestCoinsAsync("123.321");
            await api1.WalletInsertAsync(account1WalletXORURL, "TestBalance", setDefault: true, keyPair1.SK);

            var (_, api2, keysApi2) = await GetAPIsAsync();
            var (keyXorUrl, keyPair2) = await keysApi2.KeysCreatePreloadTestCoinsAsync("123.321");

            AssertThrows(-102, () => api2.WalletTransferAsync(account1WalletXORURL, keyXorUrl, "5", 0));
        }

        async Task<(Session, API.Wallet, API.Keys)> GetAPIsAsync()
        {
            var session = await TestUtils.CreateTestApp();
            return (session, session.Wallet, session.Keys);
        }

        void AssertThrows(int errorCode, AsyncTestDelegate func)
        {
            var ex = Assert.ThrowsAsync<FfiException>(func);
            Assert.AreEqual(errorCode, ex.ErrorCode);
        }
    }
}
