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

            var (keyUrl, keys) = await session.Keys.KeysCreatePreloadTestCoinsAsync("10");
            ValidateFetchOrInspectDataTypes(await session.Fetch.FetchAsync(keyUrl), true);

            var walletUrl = await session.Wallet.WalletCreateAsync();
            ValidateFetchOrInspectDataTypes(await session.Fetch.FetchAsync(walletUrl), true);

            var (filesXorUrl, processedFiles, _) = await session.Files.FilesContainerCreateAsync(
                TestUtils.TestDataDir,
                null,
                true,
                false);
            ValidateFetchOrInspectDataTypes(await session.Fetch.FetchAsync(filesXorUrl), true);
            ValidateFetchOrInspectDataTypes(await session.Fetch.FetchAsync(processedFiles.Files[0].FileXorUrl), true);

            var (_, _, nrsXorUrl) = await session.Nrs.CreateNrsMapContainerAsync(
                TestUtils.GetRandomString(5),
                $"{filesXorUrl}?v=0",
                false,
                false,
                true);
            ValidateFetchOrInspectDataTypes(await session.Fetch.FetchAsync(nrsXorUrl), true, expectNrs: true);
            ValidateFetchOrInspectDataTypes(await session.Fetch.FetchAsync(nrsXorUrl), true,  expectNrs: true);
        }

        [Test]
        public async Task InspectDataTypesTest()
        {
            var session = await TestUtils.CreateTestApp();
            var (keyUrl, _) = await session.Keys.KeysCreatePreloadTestCoinsAsync("10");
            ValidateFetchOrInspectDataTypes(await session.Fetch.InspectAsync(keyUrl), false);
            var walletUrl = await session.Wallet.WalletCreateAsync();
            ValidateFetchOrInspectDataTypes(await session.Fetch.InspectAsync(walletUrl), false);
            var (filesXorUrl, processedFiles, _) = await session.Files.FilesContainerCreateAsync(
                TestUtils.TestDataDir,
                null,
                true,
                false);
            ValidateFetchOrInspectDataTypes(await session.Fetch.InspectAsync(filesXorUrl), false);
            ValidateFetchOrInspectDataTypes(await session.Fetch.InspectAsync(processedFiles.Files[0].FileXorUrl), false);
            var (_, _, nrsXorUrl) = await session.Nrs.CreateNrsMapContainerAsync(
                TestUtils.GetRandomString(5),
                $"{filesXorUrl}?v=0",
                false,
                false,
                true);
        }

        [Test]
        public async Task NewInspectTest()
        {
            var session = await TestUtils.CreateTestApp();
            var (filesXorUrl, processedFiles, _) = await session.Files.FilesContainerCreateAsync(
                TestUtils.TestDataDir,
                null,
                true,
                false);
            var inspectdData = (PublishedImmutableData)await session.Fetch.InspectAsync(processedFiles.Files[0].FileXorUrl);
            var fetchData = (PublishedImmutableData)await session.Fetch.FetchAsync(processedFiles.Files[0].FileXorUrl);

            Assert.AreEqual(inspectdData.Data, fetchData.Data);

            Assert.AreEqual(inspectdData.XorName, fetchData.XorName);
            Assert.AreEqual(inspectdData.XorUrl, fetchData.XorUrl);
            Assert.AreEqual(inspectdData.ResolvedFrom.Version, fetchData.ResolvedFrom.Version);
            Assert.AreEqual(inspectdData.ResolvedFrom.XorName, fetchData.ResolvedFrom.XorName);
            Assert.AreEqual(inspectdData.ResolvedFrom.DataType, fetchData.ResolvedFrom.DataType);
            Assert.AreEqual(inspectdData.MediaType, fetchData.MediaType);
        }

        public void ValidateFetchOrInspectDataTypes(ISafeData data, bool isFetch, bool expectNrs = false)
        {
            if (data != null)
            {
                switch (data)
                {
                    case SafeKey key:
                        Validate.XorName(key.XorName);
                        Validate.EnsureNullNrsContainerInfo(key.ResolvedFrom);
                        break;
                    case Wallet wallet:
                        Validate.XorName(wallet.XorName);
                        Validate.EnsureNullNrsContainerInfo(wallet.ResolvedFrom);
                        break;
                    case FilesContainer filesContainer:
                        Validate.XorName(filesContainer.XorName);
                        if (expectNrs)
                            Validate.NrsContainerInfo(filesContainer.ResolvedFrom);
                        else
                            Validate.EnsureNullNrsContainerInfo(filesContainer.ResolvedFrom);
                        break;
                    case PublishedImmutableData immutableData:
                        Assert.IsNotNull(immutableData.Data);
                        Validate.XorName(immutableData.XorName);
                        if (isFetch)
                        {
                            Assert.NotZero(immutableData.Data.Length);
                        }
                        else
                        {
                            var hello = immutableData.Data.ToUtfString();
                            Assert.Zero(immutableData.Data.Length);
                        }
                        if (expectNrs)
                            Validate.NrsContainerInfo(immutableData.ResolvedFrom);
                        else
                            Validate.EnsureNullNrsContainerInfo(immutableData.ResolvedFrom);
                        break;
                    case SafeDataFetchFailed dataFetchOrInspectFailed:
                        Assert.IsNotNull(dataFetchOrInspectFailed.Description);
                        Assert.AreNotEqual(0, dataFetchOrInspectFailed.Code);
                        Assert.Fail(dataFetchOrInspectFailed.Description);
                        break;
                }
            }
            else
            {
                if (isFetch)
                    Assert.Fail("Fetch data type not available");
                else
                    Assert.Fail("Inspect data type not available");
            }
        }
    }
}
