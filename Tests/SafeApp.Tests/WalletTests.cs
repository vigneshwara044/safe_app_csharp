using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.Core;

namespace SafeApp.Tests
{
    [TestFixture]
    internal class WalletTest
    {
        [Test]
        public async Task CreateWalletTest()
        {
            var session = await TestUtils.CreateTestApp();
            var wallet = await session.Wallet.WalletCreateAsync();
            var balance = double.Parse(await session.Wallet.WalletBalanceAsync(wallet));
            Assert.AreEqual(0.0, balance);
        }

        [Test]
        public async Task InsertAndBalanceTest()
        {
            var session = await TestUtils.CreateTestApp();
            var walletXorUrl = await session.Wallet.WalletCreateAsync();
            var (_, keyPair1) = await session.Keys.KeysCreatePreloadTestCoinsAsync("123");
            var (_, keyPair2) = await session.Keys.KeysCreatePreloadTestCoinsAsync("321");
            await session.Wallet.WalletInsertAsync(walletXorUrl, "TestBalance1", true, keyPair1.SK);

            var currentBalance = double.Parse(await session.Wallet.WalletBalanceAsync(walletXorUrl));
            Assert.AreEqual(123.00, currentBalance);

            await session.Wallet.WalletInsertAsync(walletXorUrl, "TestBalance2", false, keyPair2.SK);
            currentBalance = double.Parse(await session.Wallet.WalletBalanceAsync(walletXorUrl));
            Assert.AreEqual(444.00, currentBalance);
        }

        [Test]
        public async Task InsertAndGetTest()
        {
            var session = await TestUtils.CreateTestApp();
            var walletXorUrl = await session.Wallet.WalletCreateAsync();
            var (xorUrl1, keyPair1) = await session.Keys.KeysCreatePreloadTestCoinsAsync("123");
            var (xorUrl2, keyPair2) = await session.Keys.KeysCreatePreloadTestCoinsAsync("321");

            await session.Wallet.WalletInsertAsync(walletXorUrl, "TestBalance1", true, keyPair1.SK);
            await session.Wallet.WalletInsertAsync(walletXorUrl, "TestBalance2", false, keyPair2.SK);

            var walletBalances = await session.Wallet.WalletGetAsync(walletXorUrl);
            Assert.IsTrue(walletBalances.WalletBalances.Find(q => q.WalletName.Equals("TestBalance1")).IsDefault);
            Assert.AreEqual(
                walletBalances.WalletBalances.Find(
                q => q.WalletName.Equals("TestBalance1")).Balance.XorUrl, xorUrl1);
            Assert.AreEqual(
                walletBalances.WalletBalances.Find(
                    q => q.WalletName.Equals("TestBalance1")).Balance.Sk, keyPair1.SK);

            Assert.IsFalse(walletBalances.WalletBalances.Find(q => q.WalletName.Equals("TestBalance2")).IsDefault);
            Assert.AreEqual(
                walletBalances.WalletBalances.Find(
                    q => q.WalletName.Equals("TestBalance2")).Balance.XorUrl, xorUrl2);
            Assert.AreEqual(
                walletBalances.WalletBalances.Find(
                    q => q.WalletName.Equals("TestBalance2")).Balance.Sk, keyPair2.SK);
        }

        [Test]
        public async Task WalletInsertAndSetDefaultTest()
        {
            var session = await TestUtils.CreateTestApp();

            var walletXORURL = await session.Wallet.WalletCreateAsync();
            var (xorUrl1, keyPair1) = await session.Keys.KeysCreatePreloadTestCoinsAsync("123");
            var (xorUrl2, keyPair2) = await session.Keys.KeysCreatePreloadTestCoinsAsync("321");

            await session.Wallet.WalletInsertAsync(walletXORURL, "TestBalance1", true, keyPair1.SK);
            await session.Wallet.WalletInsertAsync(walletXORURL, "TestBalance2", true, keyPair2.SK);
            var walletBalances = await session.Wallet.WalletGetAsync(walletXORURL);

            Assert.IsFalse(walletBalances.WalletBalances.Find(q => q.WalletName.Equals("TestBalance1")).IsDefault);
            Assert.AreEqual(
                walletBalances.WalletBalances.Find(
                    q => q.WalletName.Equals("TestBalance1")).Balance.XorUrl, xorUrl1);
            Assert.AreEqual(
                walletBalances.WalletBalances.Find(
                    q => q.WalletName.Equals("TestBalance1")).Balance.Sk, keyPair1.SK);

            Assert.IsTrue(walletBalances.WalletBalances.Find(q => q.WalletName.Equals("TestBalance2")).IsDefault);
            Assert.AreEqual(
                walletBalances.WalletBalances.Find(
                    q => q.WalletName.Equals("TestBalance2")).Balance.XorUrl, xorUrl2);
            Assert.AreEqual(
                walletBalances.WalletBalances.Find(
                    q => q.WalletName.Equals("TestBalance2")).Balance.Sk, keyPair2.SK);
        }

        [Test]
        public async Task TransferWithoutDefaultBalanceTest()
        {
            var session = await TestUtils.CreateTestApp();

            // without default balance
            var fromWalletXorUrl = await session.Wallet.WalletCreateAsync();

            // with default balance
            var toWalletXorUrl = await session.Wallet.WalletCreateAsync();
            var (_, keyPair) = await session.Keys.KeysCreatePreloadTestCoinsAsync("123");
            await session.Wallet.WalletInsertAsync(toWalletXorUrl, "TestBalance", true, keyPair.SK);

            var ex = Assert.ThrowsAsync<FfiException>(
              async () =>
              {
                  await session.Wallet.WalletTransferAsync(fromWalletXorUrl, toWalletXorUrl, "123", 0);
              });
            Assert.AreEqual(-203, ex.ErrorCode);

            Assert.ThrowsAsync<FfiException>(
              async () =>
              {
                  await session.Wallet.WalletTransferAsync(toWalletXorUrl, fromWalletXorUrl, "123", 0);
              });
            Assert.AreEqual(-203, ex.ErrorCode);
        }

        [Test]
        public async Task TransferFromZeroBalanceTest()
        {
            var session = await TestUtils.CreateTestApp();

            var fromWalletXorUrl = await session.Wallet.WalletCreateAsync();
            var (_, keyPair1) = await session.Keys.KeysCreatePreloadTestCoinsAsync("0.0");
            await session.Wallet.WalletInsertAsync(fromWalletXorUrl, "TestBalance", true, keyPair1.SK);

            var (toXorUrl, _) = await session.Keys.KeysCreatePreloadTestCoinsAsync("0.5");

            Assert.ThrowsAsync<FfiException>(
              async () =>
              {
                  await session.Wallet.WalletTransferAsync(fromWalletXorUrl, toXorUrl, "0.5", 0);
              });

            var toWalletXorUrl = await session.Wallet.WalletCreateAsync();
            var (xorurl2, keypair2) = await session.Keys.KeysCreatePreloadTestCoinsAsync("0.5");
            await session.Wallet.WalletInsertAsync(toWalletXorUrl, "NewTestBalance", true, keypair2.SK);

            var ex = Assert.ThrowsAsync<FfiException>(
              async () =>
              {
                  await session.Wallet.WalletTransferAsync(fromWalletXorUrl, toWalletXorUrl, "0", 0);
              });
            Assert.AreEqual(-300, ex.ErrorCode);
        }

        [Test]
        public async Task TransferDifferentAmountsTest()
        {
            var session = await TestUtils.CreateTestApp();

            var fromWalletXorUrl = await session.Wallet.WalletCreateAsync();
            var (_, keyPair1) = await session.Keys.KeysCreatePreloadTestCoinsAsync("123.321");
            await session.Wallet.WalletInsertAsync(fromWalletXorUrl, "TestBalance1", true, keyPair1.SK);

            var (toXorUrl, keypair2) = await session.Keys.KeysCreatePreloadTestCoinsAsync("0.5");
            await session.Wallet.WalletInsertAsync(fromWalletXorUrl, "TestBalance2", false, keypair2.SK);

            // transfering amount more than current balance
            var ex = Assert.ThrowsAsync<FfiException>(
               async () =>
               {
                   await session.Wallet.WalletTransferAsync(fromWalletXorUrl, toXorUrl, "321.123", 0);
               });
            Assert.AreEqual(-301, ex.ErrorCode);

            // transfering invalid amount
            ex = Assert.ThrowsAsync<FfiException>(
               async () =>
               {
                   await session.Wallet.WalletTransferAsync(fromWalletXorUrl, toXorUrl, "0.dgnda", 0);
               });
            Assert.AreEqual(-300, ex.ErrorCode);

            // valid transfer
            await session.Wallet.WalletTransferAsync(fromWalletXorUrl, toXorUrl, "10", 0);
        }

        [Test]
        public async Task TransferToSafeKeyTest()
        {
            var session = await TestUtils.CreateTestApp();

            var fromWalletXorUrl = await session.Wallet.WalletCreateAsync();
            var (_, keyPair1) = await session.Keys.KeysCreatePreloadTestCoinsAsync("123.321");
            await session.Wallet.WalletInsertAsync(fromWalletXorUrl, "TestBalance", true, keyPair1.SK);

            var (xorUrl2, _) = await session.Keys.KeysCreatePreloadTestCoinsAsync("10.0");

            // transfer from wallet to key
            await session.Wallet.WalletTransferAsync(fromWalletXorUrl, xorUrl2, "100.001", 0);
        }

        [Test]
        public async Task TransferFromSafeKeyTest()
        {
            var session = await TestUtils.CreateTestApp();
            var apiKeys = session.Keys;

            var (safekeyXorUrl1, _) = await apiKeys.KeysCreatePreloadTestCoinsAsync("10");
            var (safekeyXorUrl2, _) = await apiKeys.KeysCreatePreloadTestCoinsAsync("0");

            var ex = Assert.ThrowsAsync<FfiException>(
              async () =>
              {
                  await session.Wallet.WalletTransferAsync(safekeyXorUrl1, safekeyXorUrl2, "5", 0);
              });
            Assert.AreEqual(-207, ex.ErrorCode);
        }

        [Test]
        public async Task TransferFromUnownedWalletTest()
        {
            var session1 = await TestUtils.CreateTestApp();

            var account1WalletXORURL = await session1.Wallet.WalletCreateAsync();
            var (keyXorUrl1, keyPair1) = await session1.Keys.KeysCreatePreloadTestCoinsAsync("123.321");
            await session1.Wallet.WalletInsertAsync(account1WalletXORURL, "TestBalance", true, keyPair1.SK);

            var session2 = await TestUtils.CreateTestApp();
            var (keyXorUrl, keyPair2) = await session2.Keys.KeysCreatePreloadTestCoinsAsync("123.321");

            var ex = Assert.ThrowsAsync<FfiException>(
              async () =>
              {
                  await session2.Wallet.WalletTransferAsync(account1WalletXORURL, keyXorUrl, "5", 0);
              });
            Assert.AreEqual(-102, ex.ErrorCode);
        }
    }
}
