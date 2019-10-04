using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.API;

namespace SafeApp.Tests
{
    [TestFixture]
    internal class NrsTest
    {
        private const bool SetDefault = true;
        private const bool DirectLink = true;
        private const bool DryRun = false;

        private readonly string _dirName = TestUtils.GetRandomString(5);

        [SetUp]
        public void PrepareTestData()
        {
            Directory.CreateDirectory(_dirName);
            var testFilePath = Path.Combine(_dirName, "index.html");
            File.WriteAllText(testFilePath, TestUtils.GetRandomString(20));
        }

        [Test]
        public async Task ParseUrlTest()
        {
            var session = await TestUtils.CreateTestApp();
            var (xorUrl, _) = await session.Keys.KeysCreatePreloadTestCoinsAsync("1");

            var xorUrlEncoder = await Nrs.ParseUrlAsync(xorUrl);

            // todo: verify that these are actually the expected values
            Assert.AreEqual(0, xorUrlEncoder.ContentType);
            Assert.AreEqual(0, xorUrlEncoder.ContentVersion);
            Assert.AreEqual(0, xorUrlEncoder.DataType);
            Assert.AreEqual(1, xorUrlEncoder.EncodingVersion);
            Assert.AreEqual(string.Empty, xorUrlEncoder.Path);
            Assert.AreEqual(string.Empty, xorUrlEncoder.SubNames);
            Assert.AreEqual(0, xorUrlEncoder.TypeTag);
            Assert.IsNotNull(xorUrlEncoder.Xorname);
            Assert.IsNotEmpty(xorUrlEncoder.Xorname);
        }

        [Test]
        public async Task ParseAndResolveUrlTest()
        {
            var session = await TestUtils.CreateTestApp();
            var (xorUrl, _) = await session.Keys.KeysCreatePreloadTestCoinsAsync("1");

            var api = session.Nrs;
            var (xorUrlEncoder, resolvesAsNrs) = await api.ParseAndResolveUrlAsync(xorUrl);

            // todo: verify that these are actually the expected values
            Assert.AreEqual(0, xorUrlEncoder.ContentType);
            Assert.AreEqual(0, xorUrlEncoder.ContentVersion);
            Assert.AreEqual(0, xorUrlEncoder.DataType);
            Assert.AreEqual(1, xorUrlEncoder.EncodingVersion);
            Assert.AreEqual(string.Empty, xorUrlEncoder.Path);
            Assert.AreEqual(string.Empty, xorUrlEncoder.SubNames);
            Assert.AreEqual(0, xorUrlEncoder.TypeTag);
            Assert.IsNotNull(xorUrlEncoder.Xorname);
            Assert.IsNotEmpty(xorUrlEncoder.Xorname);
            Assert.IsTrue(resolvesAsNrs);
        }

        [Test]
        public async Task CreateNrsMapContainerTest()
        {
            var session = await TestUtils.CreateTestApp();
            var name = TestUtils.GetRandomString(5);

            var link = await CreateFilesContainerAsync(session);

            var api = session.Nrs;
            var (nrsMap, processedEntries, xorUrl) = await api.CreateNrsMapContainerAsync(
                name,
                $"{link}?v=0",
                DirectLink,
                DryRun,
                SetDefault);

            Assert.IsNotNull(nrsMap);
            Assert.IsNotNull(processedEntries);
            Assert.IsNotNull(xorUrl);

            var encoder = await XorEncoder.XorUrlEncoderFromUrl(xorUrl);

            Assert.AreEqual(3, encoder.ContentType);
            Assert.AreEqual(0, encoder.ContentVersion);
            Assert.AreEqual(5, encoder.DataType);
            Assert.AreEqual(1, encoder.EncodingVersion);
            Assert.AreEqual(string.Empty, encoder.Path);
            Assert.AreEqual(string.Empty, encoder.SubNames);
            Assert.AreEqual(1500, encoder.TypeTag);
            Assert.IsNotNull(encoder.Xorname);
            Assert.IsNotEmpty(encoder.Xorname);

            // todo: deserialize and test (not giving correct value of the nrsMap string yet)
            // Assert.IsNotEmpty(nrsMap.SubNamesMap.SubNames);
            // Assert.IsNotNull(nrsMap.Default);
            // nrsMap.SubNamesMap.SubNames.ForEach(s =>
            // {
            //     Assert.IsNotNull(s.SubName);
            //     Assert.IsNotNull(s.SubNameRdf);
            // });
        }

        [Test]
        public async Task AddToNrsMapContainerTest()
        {
            var session = await TestUtils.CreateTestApp();
            var name = TestUtils.GetRandomString(5);
            var xorUrlResult = await CreateNrsMapContainerXorUrlAsync(session, name);

            var link = await CreateFilesContainerAsync(session);

            var (nrsMap, xorUrl, version) = await session.Nrs.AddToNrsMapContainerAsync(
                name,
                $"{link}?v=0",
                SetDefault,
                DirectLink,
                DryRun);

            Assert.IsNotNull(nrsMap);
            Assert.IsNotEmpty(xorUrl);
            Assert.AreEqual(1, version);

            var encoder = await XorEncoder.XorUrlEncoderFromUrl(xorUrl);

            Assert.AreEqual(3, encoder.ContentType);
            Assert.AreEqual(0, encoder.ContentVersion);
            Assert.AreEqual(5, encoder.DataType);
            Assert.AreEqual(1, encoder.EncodingVersion);
            Assert.AreEqual(string.Empty, encoder.Path);
            Assert.AreEqual(string.Empty, encoder.SubNames);
            Assert.AreEqual(1500, encoder.TypeTag);
            Assert.IsNotNull(encoder.Xorname);
            Assert.IsNotEmpty(encoder.Xorname);

            // todo: deserialize and test (not giving correct value of the nrsMap string yet)
            // Assert.IsNotEmpty(nrsMap.SubNamesMap.SubNames);
            // Assert.IsNotNull(nrsMap.Default);
            // nrsMap.SubNamesMap.SubNames.ForEach(s =>
            // {
            //     Assert.IsNotNull(s.SubName);
            //     Assert.IsNotNull(s.SubNameRdf);
            // });
        }

        [Test]
        public async Task RemoveFromNrsMapContainerTest()
        {
            var session = await TestUtils.CreateTestApp();
            var name = TestUtils.GetRandomString(5);
            var xorUrlResult = await CreateNrsMapContainerXorUrlAsync(session, name);

            var (nrsMap, xorUrl, version) = await session.Nrs.RemoveFromNrsMapContainerAsync(name, DryRun);

            Assert.IsNotNull(nrsMap);
            Assert.IsNotEmpty(xorUrl);
            Assert.AreEqual(1, version);

            var encoder = await XorEncoder.XorUrlEncoderFromUrl(xorUrl);

            Assert.AreEqual(3, encoder.ContentType);
            Assert.AreEqual(0, encoder.ContentVersion);
            Assert.AreEqual(5, encoder.DataType);
            Assert.AreEqual(1, encoder.EncodingVersion);
            Assert.AreEqual(string.Empty, encoder.Path);
            Assert.AreEqual(string.Empty, encoder.SubNames);
            Assert.AreEqual(1500, encoder.TypeTag);
            Assert.IsNotNull(encoder.Xorname);
            Assert.IsNotEmpty(encoder.Xorname);

            // todo: deserialize and test (not giving correct value of the nrsMap string yet)
            // Assert.IsNotNull(nrsMap.SubNamesMap);
            // Assert.IsNotEmpty(nrsMap.SubNamesMap.SubNames);
            // Assert.IsNotNull(nrsMap.Default);
            // nrsMap.SubNamesMap.SubNames.ForEach(s =>
            // {
            //     Assert.IsNotNull(s.SubName);
            //     Assert.IsNotNull(s.SubNameRdf);
            // });
        }

        [Test]
        public async Task GetNrsMapContainerTest()
        {
            var session = await TestUtils.CreateTestApp();
            var name = TestUtils.GetRandomString(5);
            var xorUrl = await CreateNrsMapContainerXorUrlAsync(session, name);

            var api = session.Nrs;
            var (nrsMap, version) = await api.GetNrsMapContainerAsync(xorUrl);

            Assert.IsNotNull(nrsMap);
            Assert.AreEqual(0, version);

            var encoder = await XorEncoder.XorUrlEncoderFromUrl(xorUrl);

            Assert.AreEqual(3, encoder.ContentType);
            Assert.AreEqual(0, encoder.ContentVersion);
            Assert.AreEqual(5, encoder.DataType);
            Assert.AreEqual(1, encoder.EncodingVersion);
            Assert.AreEqual(string.Empty, encoder.Path);
            Assert.AreEqual(string.Empty, encoder.SubNames);
            Assert.AreEqual(1500, encoder.TypeTag);
            Assert.IsNotNull(encoder.Xorname);
            Assert.IsNotEmpty(encoder.Xorname);

            // todo: deserialize and test (not giving correct value of the nrsMap string yet)
            // Assert.IsNotNull(nrsMap.SubNamesMap);
            // Assert.IsNotEmpty(nrsMap.SubNamesMap.SubNames);
            // Assert.IsNotNull(nrsMap.Default);
            // nrsMap.SubNamesMap.SubNames.ForEach(s =>
            // {
            //     Assert.IsNotNull(s.SubName);
            //     Assert.IsNotNull(s.SubNameRdf);
            // });
        }

        async Task<string> CreateFilesContainerAsync(Session session)
        {
            var (xorUrl, _, _) = await session.Files.FilesContainerCreateAsync(
                _dirName,
                null,
                true,
                false);

            return xorUrl;
        }

        async Task<string> CreateNrsMapContainerXorUrlAsync(Session session, string name)
        {
            var link = await CreateFilesContainerAsync(session);

            var api = session.Nrs;
            var (_, _, xorUrl) = await api.CreateNrsMapContainerAsync(
                name,
                $"{link}?v=0",
                DirectLink,
                DryRun,
                SetDefault);

            return xorUrl;
        }
    }
}
