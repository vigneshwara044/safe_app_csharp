using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.API;
using SafeApp.Core;

namespace SafeApp.Tests
{
    [TestFixture]
    internal class FilesTest
    {
        [Test]
        public async Task FilesContainerCreateTest()
        {
            var session = await TestUtils.CreateTestApp();
            var (xorUrl, _, _) =  await session.Files.FilesContainerCreateAsync("../testData", null, false, false);
            Assert.NotNull(xorUrl);
        }

        [Test]
        public async Task FilesContainerGetTest()
        {
            var session = await TestUtils.CreateTestApp();
            var (xorUrl, _, _) = await session.Files.FilesContainerCreateAsync("../testData", null, false, false);
            await session.Files.FilesContainerGetAsync(xorUrl);
            
        }

        [Test]
        public async Task FilesContainerSyncTest()
        {
            var session = await TestUtils.CreateTestApp();
            var (xorUrl, _, _) = await session.Files.FilesContainerCreateAsync("../testData", null, false, false);
            await session.Files.FilesContainerSyncAsync("../testData", xorUrl, true, false, false, false);
        }

        [Test]
        public async Task FilesContainerAddTest()
        {
            var session = await TestUtils.CreateTestApp();
            var (xorUrl, _, _) = await session.Files.FilesContainerCreateAsync("../testData", null, false, false);
            var newFileName = $"{xorUrl}/newFile.md";
            await session.Files.FilesContainerAddAsync("../testData", newFileName, false, false, false);
        }

        [Test]
        public async Task FilesContainerAddFromRawTest()
        {
            var session = await TestUtils.CreateTestApp();
            var (xorUrl, _, _) = await session.Files.FilesContainerCreateAsync("../testData", null, false, false);
            var newFileName = $"{xorUrl}/newFile.md";
            await session.Files.FilesContainerAddFromRawAsync(new List<byte> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 }, newFileName, false, false, false);
        }

        [Test]
        public async Task FilesPutPublishedImmutableTest()
        {
            var session = await TestUtils.CreateTestApp();
        }

        [Test]
        public async Task FilesGetPublishedImmutableTest()
        {
            var session = await TestUtils.CreateTestApp();
        }
    }
}
