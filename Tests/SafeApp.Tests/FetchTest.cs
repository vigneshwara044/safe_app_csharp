using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.Core;

namespace SafeApp.Tests
{
    [TestFixture]
    public class FetchTest
    {
        [Test]
        public async Task FetchDataTypesTest()
        {
            var session = await TestUtils.CreateTestApp();
            var (keyUrl, _) = await session.Keys.KeysCreatePreloadTestCoinsAsync("10");
            ValidateFetchDataTypes(await session.Fetch.FetchAsync(keyUrl));
            var walletUrl = await session.Wallet.WalletCreateAsync();
            ValidateFetchDataTypes(await session.Fetch.FetchAsync(walletUrl));
        }

        public void ValidateFetchDataTypes(ISafeData data)
        {
            if (data != null)
            {
                switch (data)
                {
                    case SafeKey key:
                        Assert.IsNotNull(key.Xorname);
                        Assert.IsNotNull(key.ResolvedFrom);
                        break;
                    case Wallet wallet:
                        Assert.IsNotNull(wallet.Xorname);
                        Assert.IsNotNull(wallet.ResolvedFrom);
                        break;
                    case FilesContainer filesContainer:
                        Assert.IsNotNull(filesContainer.ResolvedFrom);
                        Assert.IsNotNull(filesContainer.Xorname);
                        break;
                    case PublishedImmutableData immutableData:
                        Assert.IsNotNull(immutableData.Xorname);
                        Assert.IsNotNull(immutableData.Data);
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
    }
}
