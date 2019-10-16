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
        public async Task FetchAndInspectDataTypesTest()
        {
            var session = await TestUtils.CreateTestApp();
            var (keyUrl, _) = await session.Keys.KeysCreatePreloadTestCoinsAsync("10");
            ValidateFetchDataTypes(await session.Fetch.FetchAsync(keyUrl));
            ValidateFetchDataTypes(await session.Fetch.InspectAsync(keyUrl));
            var walletUrl = await session.Wallet.WalletCreateAsync();
            ValidateFetchDataTypes(await session.Fetch.FetchAsync(walletUrl));
            ValidateFetchDataTypes(await session.Fetch.InspectAsync(walletUrl));
            var (filesXorUrl, processedFiles, _) = await session.Files.FilesContainerCreateAsync(
                TestUtils.TestDataDir,
                null,
                true,
                false);
            ValidateFetchDataTypes(await session.Fetch.FetchAsync(filesXorUrl));
            ValidateFetchDataTypes(await session.Fetch.InspectAsync(filesXorUrl));
            ValidateFetchDataTypes(await session.Fetch.FetchAsync(processedFiles.Files[0].FileXorUrl));
            ValidateFetchDataTypes(await session.Fetch.InspectAsync(processedFiles.Files[0].FileXorUrl));
            var (_, _, nrsXorUrl) = await session.Nrs.CreateNrsMapContainerAsync(
                TestUtils.GetRandomString(5),
                $"{filesXorUrl}?v=0",
                false,
                false,
                true);
            ValidateFetchDataTypes(await session.Fetch.FetchAsync(nrsXorUrl), expectNrs: true);
            ValidateFetchDataTypes(await session.Fetch.FetchAsync(nrsXorUrl), expectNrs: true);
        }

        public void ValidateFetchDataTypes(ISafeData data, bool expectNrs = false)
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
                        if (expectNrs)
                            Validate.NrsContainerInfo(immutableData.ResolvedFrom);
                        else
                            Validate.EnsureNullNrsContainerInfo(immutableData.ResolvedFrom);
                        break;
                    case SafeDataFetchFailed dataFetchFailed:
                        Assert.IsNotNull(dataFetchFailed.Description);
                        Assert.AreNotEqual(0, dataFetchFailed.Code);
                        Assert.Fail(dataFetchFailed.Description);
                        break;
                }
            }
            else
            {
                Assert.Fail("Fetch data type not available");
            }
        }
    }
}
