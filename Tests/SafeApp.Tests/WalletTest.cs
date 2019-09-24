using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.Core;

namespace SafeApp.Tests
{
    [TestFixture]
    internal class WalletTest
    {
        [SetUp]
        public async Task Init()
        {
            await InitLogging();
        }

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
            var walletXORURL = await session.Wallet.WalletCreateAsync();
            var (_, keyPair1) = await session.Keys.KeysCreatePreloadTestCoins("123");
            var (_, keyPair2) = await session.Keys.KeysCreatePreloadTestCoins("321");
            await session.Wallet.WalletInsertAsync(walletXORURL, "TestBalance1", true, keyPair1.SK);

            var currentBalance = double.Parse(await session.Wallet.WalletBalanceAsync(walletXORURL));
            Assert.AreEqual(123.00, currentBalance);

            await session.Wallet.WalletInsertAsync(walletXORURL, "TestBalance2", false, keyPair2.SK);
            currentBalance = double.Parse(await session.Wallet.WalletBalanceAsync(walletXORURL));
            Assert.AreEqual(321.00, currentBalance);
        }

        [Test]
        public async Task InsertAndGetTest()
        {
            var session = await TestUtils.CreateTestApp();
            var walletXORURL = await session.Wallet.WalletCreateAsync();
            var (xorurl1, keyPair1) = await session.Keys.KeysCreatePreloadTestCoins("123");
            var (xorurl2, keyPair2) = await session.Keys.KeysCreatePreloadTestCoins("321");

            await session.Wallet.WalletInsertAsync(walletXORURL, "TestBalance1", true, keyPair1.SK);
            await session.Wallet.WalletInsertAsync(walletXORURL, "TestBalance2", false, keyPair2.SK);

            var walletBalances = await session.Wallet.WalletGetAsync(walletXORURL);
            Assert.IsTrue(walletBalances.WalletBalances.Find(q => q.WalletName.Equals("TestBalance1")).IsDefault);
            Assert.AreEqual(walletBalances.WalletBalances.Find(q => q.WalletName.Equals("TestBalance1")).SpendableWalletBalance.Xorurl, xorurl1);
            Assert.AreEqual(walletBalances.WalletBalances.Find(q => q.WalletName.Equals("TestBalance1")).SpendableWalletBalance.Sk, keyPair1.SK);

            Assert.IsTrue(walletBalances.WalletBalances.Find(q => q.WalletName.Equals("TestBalance2")).IsDefault);
            Assert.AreEqual(walletBalances.WalletBalances.Find(q => q.WalletName.Equals("TestBalance2")).SpendableWalletBalance.Xorurl, xorurl1);
            Assert.AreEqual(walletBalances.WalletBalances.Find(q => q.WalletName.Equals("TestBalance2")).SpendableWalletBalance.Sk, keyPair2.SK);
        }

        [Test]
        public async Task WalletInsertAndSetDefaultTest()
        {
            var session = await TestUtils.CreateTestApp();

            var walletXORURL = await session.Wallet.WalletCreateAsync();
            var (xorurl1, keyPair1) = await session.Keys.KeysCreatePreloadTestCoins("123");
            var (xorurl2, keyPair2) = await session.Keys.KeysCreatePreloadTestCoins("321");

            await session.Wallet.WalletInsertAsync(walletXORURL, "TestBalance1", true, keyPair1.SK);
            await session.Wallet.WalletInsertAsync(walletXORURL, "TestBalance2", true, keyPair2.SK);
            var walletBalances = await session.Wallet.WalletGetAsync(walletXORURL);

            Assert.IsTrue(walletBalances.WalletBalances.Find(q => q.WalletName.Equals("TestBalance1")).IsDefault);
            Assert.AreEqual(walletBalances.WalletBalances.Find(q => q.WalletName.Equals("TestBalance1")).SpendableWalletBalance.Xorurl, xorurl1);
            Assert.AreEqual(walletBalances.WalletBalances.Find(q => q.WalletName.Equals("TestBalance1")).SpendableWalletBalance.Sk, keyPair1.SK);

            Assert.IsTrue(walletBalances.WalletBalances.Find(q => q.WalletName.Equals("TestBalance2")).IsDefault);
            Assert.AreEqual(walletBalances.WalletBalances.Find(q => q.WalletName.Equals("TestBalance2")).SpendableWalletBalance.Xorurl, xorurl2);
            Assert.AreEqual(walletBalances.WalletBalances.Find(q => q.WalletName.Equals("TestBalance2")).SpendableWalletBalance.Sk, keyPair2.SK);
        }

        [Test]
        public async Task TransferWithoutDefaultBalanceTest()
        {
            var session = await TestUtils.CreateTestApp();

            // without default balance
            var fromWalletXORURL = await session.Wallet.WalletCreateAsync();

            // with default balance
            var toWalletXORURL = await session.Wallet.WalletCreateAsync();
            var (xorurl, keyPair) = await session.Keys.KeysCreatePreloadTestCoins("123");
            await session.Wallet.WalletInsertAsync(toWalletXORURL, "TestBalance", true, keyPair.SK);

            var ex = Assert.ThrowsAsync<FfiException>(
               async () =>
               {
                    await session.Wallet.WalletTransferAsync(fromWalletXORURL, toWalletXORURL, "123", 0);
                });
            Assert.AreEqual(-203, ex.ErrorCode);

            Assert.ThrowsAsync<FfiException>(
               async () =>
               {
                   await session.Wallet.WalletTransferAsync(toWalletXORURL, fromWalletXORURL, "123", 0);
               });
            Assert.AreEqual(-203, ex.ErrorCode);
        }

        [Test]
        public async Task TransferFromZeroBalanceTest()
        {
            var session = await TestUtils.CreateTestApp();

            var fromWalletXORURL = await session.Wallet.WalletCreateAsync();
            var (xorurl1, keyPair1) = await session.Keys.KeysCreatePreloadTestCoins("0.0");
            await session.Wallet.WalletInsertAsync(fromWalletXORURL, "TestBalance", true, keyPair1.SK);

            var (toXorurl, _) = await session.Keys.KeysCreatePreloadTestCoins("0.5");

            Assert.ThrowsAsync<FfiException>(
               async () =>
               {
                   await session.Wallet.WalletTransferAsync(fromWalletXORURL, toXorurl, "0.5", 0);
               });

            var toWalletXORURL = await session.Wallet.WalletCreateAsync();
            var (xorurl2, keypair2) = await session.Keys.KeysCreatePreloadTestCoins("0.5");
            await session.Wallet.WalletInsertAsync(toWalletXORURL, "NewTestBalance", true, keypair2.SK);

            var ex = Assert.ThrowsAsync<FfiException>(
               async () =>
               {
                   await session.Wallet.WalletTransferAsync(fromWalletXORURL, toWalletXORURL, "0", 0);
               });
            Assert.AreEqual(-301, ex.ErrorCode);
        }

        [Test]
        public async Task TransferDifferentAmounts()
        {
            var session = await TestUtils.CreateTestApp();

            var fromWalletXORURL = await session.Wallet.WalletCreateAsync();
            var (xorurl1, keyPair1) = await session.Keys.KeysCreatePreloadTestCoins("123.321");
            await session.Wallet.WalletInsertAsync(fromWalletXORURL, "TestBalance", true, keyPair1.SK);

            var toWalletXORURL = await session.Wallet.WalletCreateAsync();
            var (xorurl2, keypair2) = await session.Keys.KeysCreatePreloadTestCoins("0.5");
            await session.Wallet.WalletInsertAsync(fromWalletXORURL, "TestBalance", true, keypair2.SK);

            // transfering amount more than current balance
            var ex = Assert.ThrowsAsync<FfiException>(
               async () =>
               {
                   await session.Wallet.WalletTransferAsync(fromWalletXORURL, toWalletXORURL, "321.123", 0);
               });
            Assert.AreEqual(-301, ex.ErrorCode);

            // transfering invalid amount
            ex = Assert.ThrowsAsync<FfiException>(
               async () =>
               {
                   await session.Wallet.WalletTransferAsync(fromWalletXORURL, toWalletXORURL, "0.dgnda", 0);
               });
            Assert.AreEqual(-300, ex.ErrorCode);

            // valid transfer
            await session.Wallet.WalletTransferAsync(fromWalletXORURL, toWalletXORURL, "10", 0);
        }

        [Test]
        public async Task TransferToSafeKey()
        {
            var session = await TestUtils.CreateTestApp();

            var fromWalletXORURL = await session.Wallet.WalletCreateAsync();
            var (_, keyPair1) = await session.Keys.KeysCreatePreloadTestCoins("123.321");
            await session.Wallet.WalletInsertAsync(fromWalletXORURL, "TestBalance", true, keyPair1.SK);

            var (xorurl2, _) = await session.Keys.KeysCreatePreloadTestCoins("10.0");

            // transfer from wallet to key
            await session.Wallet.WalletTransferAsync(fromWalletXORURL, xorurl2, "100.001", 0);
        }

        [Test]
        public async Task TransferFromSafeKey()
        {
            var session = await TestUtils.CreateTestApp();
            var apiKeys = session.Keys;

            var (safekeyXORURL1, _) = await apiKeys.KeysCreatePreloadTestCoins("10");
            var (safekeyXORURL2, _) = await apiKeys.KeysCreatePreloadTestCoins("0");

            var ex = Assert.ThrowsAsync<FfiException>(
               async () =>
               {
                   await session.Wallet.WalletTransferAsync(safekeyXORURL1, safekeyXORURL2, "5", 0);
               });
            Assert.AreEqual(-207, ex.ErrorCode);
        }

        // [Test]
        // public async Task TransferWithNRSURLsTest();
        // To be completed when NRS API is ready.

        [Test]
        public async Task TransferFromUnownedWalletTest()
        {
            var session1 = await TestUtils.CreateTestApp();

            var account1WalletXORURL = await session1.Wallet.WalletCreateAsync();
            var (keyXORURL1, keyPair1) = await session1.Keys.KeysCreatePreloadTestCoins("123.321");
            await session1.Wallet.WalletInsertAsync(account1WalletXORURL, "TestBalance", true, keyPair1.SK);

            var session2 = await TestUtils.CreateTestApp();
            var (keyXORURL2, keyPair2) = await session2.Keys.KeysCreatePreloadTestCoins("123.321");

            var ex = Assert.ThrowsAsync<FfiException>(
               async () =>
               {
                   await session2.Wallet.WalletTransferAsync(account1WalletXORURL, keyXORURL2, "5", 0);
               });
            Assert.AreEqual(-102, ex.ErrorCode);
        }

        async Task InitLogging()
        {
            await Session.SetAdditionalSearchPathAsync(@"/Users/admin/projects/maidsafe/safe_app_csharp/Tests/SafeApp.Tests.Core/bin/Release");
            await Session.InitLoggingAsync();
        }
    }
}
