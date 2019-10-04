using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.Core;

namespace SafeApp.Tests
{
    [TestFixture]
    internal class FilesTest
    {
        [OneTimeSetUp]
        public void Setup() => TestUtils.PrepareTestData();

        [OneTimeTearDown]
        public void TearDown() => TestUtils.RemoveTestData();

        [Test]
        public async Task FilesContainerCreateAndGetTest()
        {
            var session = await TestUtils.CreateTestApp();
            var (xorUrl, processedFiles, filesMap1) = await session.Files.FilesContainerCreateAsync(TestUtils.TestDataDir, null, true, false);
            Assert.NotNull(processedFiles.Files.Find(q => q.FileName.Equals("index.html")));
            Assert.NotNull(xorUrl);
            Assert.NotNull(processedFiles);
            Assert.NotNull(filesMap1);

            var (version, filesMap2) = await session.Files.FilesContainerGetAsync(xorUrl);
            Assert.AreEqual(0, version);
        }

        [Test]
        public async Task FilesContainerSyncTest()
        {
            var session = await TestUtils.CreateTestApp();
            var (xorUrl, processedFiles, filesMap) = await session.Files.FilesContainerCreateAsync(TestUtils.TestDataDir, null, true, false);
            Directory.CreateDirectory($"{TestUtils.TestDataDir}/newDir");
            var testFilePath = Path.Combine($"{TestUtils.TestDataDir}/newDir", "hello.html");
            File.WriteAllText(testFilePath, TestUtils.GetRandomString(20));
            var (version, newProcessedFiles, newFileMap) = await session.Files.FilesContainerSyncAsync($"{TestUtils.TestDataDir}/newDir", xorUrl, true, false, false, false);
            Assert.NotNull(newProcessedFiles.Files.Find(q => q.FileName.Equals("hello.html")));

            Assert.AreEqual(1, version);
            Assert.AreNotEqual(processedFiles, newProcessedFiles);
            Assert.AreNotEqual(filesMap, newFileMap);
        }

        [Test]
        public async Task FilesContainerSyncDryRunTest()
        {
            var session = await TestUtils.CreateTestApp();
            var (xorUrl, processedFiles, filesMap) = await session.Files.FilesContainerCreateAsync(TestUtils.TestDataDir, null, true, false);
            Directory.CreateDirectory($"{TestUtils.TestDataDir}/newDir");
            var testFilePath = Path.Combine($"{TestUtils.TestDataDir}/newDir", "hello.html");
            File.WriteAllText(testFilePath, TestUtils.GetRandomString(20));
            var (version, newProcessedFiles, newFileMap) = await session.Files.FilesContainerSyncAsync($"{TestUtils.TestDataDir}/newDir", xorUrl, true, false, false, true);
            Assert.NotNull(newProcessedFiles.Files.Find(q => q.FileName.Equals("hello.html")));

            Assert.AreEqual(1, version);
            Assert.AreNotEqual(processedFiles, newProcessedFiles);
            Assert.AreNotEqual(filesMap, newFileMap);
        }

        [Test]
        public async Task FilesContainerAddTest()
        {
            var session = await TestUtils.CreateTestApp();
            var (xorUrl, processedFiles, filesMap) = await session.Files.FilesContainerCreateAsync(TestUtils.TestDataDir, null, false, false);
            var testFilePath = Path.Combine(TestUtils.TestDataDir, "hello.html");
            File.WriteAllText(testFilePath, TestUtils.GetRandomString(20));
            var newFileName = $"{xorUrl}/hello.html";
            var (version, newProcessedFiles, newfilesMap) = await session.Files.FilesContainerAddAsync($"{TestUtils.TestDataDir}/hello.html", newFileName, false, false, false);
            Assert.AreEqual(1, version);
            Assert.NotNull(newProcessedFiles.Files.Find(q => q.FileName.Equals("hello.html")));
            Assert.AreNotEqual(processedFiles, newProcessedFiles);
            Assert.AreNotEqual(filesMap, newfilesMap);
        }

        public async Task FilesContainerAddDryRunTest()
        {
            var session = await TestUtils.CreateTestApp();
            var (xorUrl, processedFiles, filesMap) = await session.Files.FilesContainerCreateAsync(TestUtils.TestDataDir, null, false, true);
            var testFilePath = Path.Combine(TestUtils.TestDataDir, "hello.html");
            File.WriteAllText(testFilePath, TestUtils.GetRandomString(20));
            var newFileName = $"{xorUrl}/hello.html";
            var (version, newProcessedFiles, newfilesMap) = await session.Files.FilesContainerAddAsync($"{TestUtils.TestDataDir}/hello.html", newFileName, false, false, true);
            Assert.AreEqual(1, version);
            Assert.NotNull(newProcessedFiles.Files.Find(q => q.FileName.Equals("hello.html")));
            Assert.AreNotEqual(processedFiles, newProcessedFiles);
            Assert.AreNotEqual(filesMap, newfilesMap);
        }

        [Test]
        public async Task FilesContainerAddFromRawTest()
        {
            var session = await TestUtils.CreateTestApp();
            var (xorUrl, processedFiles, filesMap) = await session.Files.FilesContainerCreateAsync(TestUtils.TestDataDir, null, false, false);
            var newFileName = $"{xorUrl}/hello.html";
            var (version, newProcessedFiles, newFilesMap) = await session.Files.FilesContainerAddFromRawAsync(TestUtils.GetRandomString(50).ToUtfBytes(), newFileName, false, false, false);
            Assert.AreEqual(1, version);
            Assert.NotNull(newProcessedFiles.Files.Find(q => q.FileName.Equals("hello.html")));
            Assert.AreNotEqual(processedFiles, newProcessedFiles);
            Assert.AreNotEqual(filesMap, newFilesMap);
        }

        [Test]
        public async Task FilesPutAndGetPublishedImmutableTest()
        {
            var session = await TestUtils.CreateTestApp();
            var data = TestUtils.GetRandomString(20).ToUtfBytes();
            var xorUrl = await session.Files.FilesPutPublishedImmutableAsync(data, "text/plain");
            var newData = await session.Files.FilesGetPublishedImmutableAsync(xorUrl);
            Assert.AreEqual(data, newData);
        }
    }
}
