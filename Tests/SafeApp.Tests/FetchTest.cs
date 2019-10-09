using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.Core;

namespace SafeApp.Tests
{
    [TestFixture]
    public class FetchTest
    {
        [OneTimeSetUp]
        public void Setup() => TestUtils.PrepareTestData();

        [OneTimeTearDown]
        public void TearDown() => TestUtils.RemoveTestData();

        [Test]
        public async Task FetchDataTypesTest()
        {
            var session = await TestUtils.CreateTestApp();
            var (keyUrl, _) = await session.Keys.KeysCreatePreloadTestCoinsAsync("10");
            ValidateFetchDataTypes(await session.Fetch.FetchAsync(keyUrl));
            var walletUrl = await session.Wallet.WalletCreateAsync();
            ValidateFetchDataTypes(await session.Fetch.FetchAsync(walletUrl));
            var (filesXorUrl, _, _) = await session.Files.FilesContainerCreateAsync(
                TestUtils.TestDataDir,
                null,
                true,
                false);
            ValidateFetchDataTypes(await session.Fetch.FetchAsync(filesXorUrl));
            var (_, _, nrsXorUrl) = await session.Nrs.CreateNrsMapContainerAsync(
                TestUtils.GetRandomString(5),
                $"{filesXorUrl}?v=0",
                false,
                false,
                true);
            ValidateFetchDataTypes(await session.Fetch.FetchAsync(nrsXorUrl), expectNrs: true);
        }

        public void ValidateFetchDataTypes(ISafeData data, bool expectNrs = false)
        {
            if (data != null)
            {
                switch (data)
                {
                    case SafeKey key:
                        TestUtils.ValidateXorName(key.Xorname);
                        EnsureNullNrsContainerInfo(key.ResolvedFrom);
                        break;
                    case Wallet wallet:
                        TestUtils.ValidateXorName(wallet.Xorname);
                        EnsureNullNrsContainerInfo(wallet.ResolvedFrom);
                        break;
                    case FilesContainer filesContainer:
                        TestUtils.ValidateXorName(filesContainer.Xorname);
                        if (expectNrs)
                            ValidateNrsContainerInfo(filesContainer.ResolvedFrom);
                        else
                            EnsureNullNrsContainerInfo(filesContainer.ResolvedFrom);
                        break;
                    case PublishedImmutableData immutableData:
                        Assert.IsNotNull(immutableData.Data);
                        TestUtils.ValidateXorName(immutableData.Xorname);
                        ValidateNrsContainerInfo(immutableData.ResolvedFrom);
                        break;
                    case SafeDataFetchFailed dataFetchFailed:
                        Assert.IsNotNull(dataFetchFailed.Description);
                        Assert.AreNotEqual(0, dataFetchFailed.Code);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                Assert.Fail("Fetch data type not available");
            }
        }

        void ValidateNrsContainerInfo(NrsMapContainerInfo info)
        {
            Assert.AreNotEqual(0, info.DataType);
            Assert.IsNotNull(info.NrsMap);
            Assert.IsNotNull(info.PublicName);
            Assert.AreNotEqual(0, info.TypeTag);
            Assert.IsNotNull(info.Version);
            Assert.IsNotNull(info.XorUrl);
            TestUtils.ValidateXorName(info.XorName);
        }

        void EnsureNullNrsContainerInfo(NrsMapContainerInfo info)
        {
            Assert.AreEqual(0, info.DataType); // iffy, since 0 is actually a data type
            Assert.IsNull(info.NrsMap);
            Assert.IsNull(info.PublicName);
            Assert.AreEqual(0, info.TypeTag); // is TT=0 used?
            Assert.AreEqual(0, info.Version); // iffy, since v 0 is actually the first version
            Assert.IsNull(info.XorUrl);
            Assert.IsTrue(Enumerable.SequenceEqual(new byte[32], info.XorName));
        }
    }
}
