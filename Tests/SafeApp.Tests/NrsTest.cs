using System;
using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.API;
using SafeApp.Core;

namespace SafeApp.Tests
{
    [TestFixture]
    internal class NrsTest
    {
        private const bool SetDefault = true;
        private const bool DirectLink = true;
        private const bool DryRun = false;

        [OneTimeSetUp]
        public void Setup() => TestUtils.PrepareTestData();

        [OneTimeTearDown]
        public void TearDown() => TestUtils.RemoveTestData();

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
            Assert.AreEqual("[]", xorUrlEncoder.SubNames);
            Assert.AreEqual(0, xorUrlEncoder.TypeTag);
            TestUtils.ValidateXorName(xorUrlEncoder.Xorname);
        }

        [Test]
        public async Task ParseAndResolveUrlTest()
        {
            var session = await TestUtils.CreateTestApp();

            var xorUrl = await CreateFilesContainerAsync(session);

            var api = session.Nrs;
            var (_, _, nrsMapXorUrl) = await api.CreateNrsMapContainerAsync(
                TestUtils.GetRandomString(5),
                $"{xorUrl}?v=0",
                false,
                DryRun,
                SetDefault);
            var (xorUrlEncoder, resolvedFrom) = await api.ParseAndResolveUrlAsync(nrsMapXorUrl);

            ValidateEncoder(xorUrlEncoder, ContentType.FilesContainer, 1100);
            ValidateEncoder(resolvedFrom, ContentType.NrsMapContainer, 1500);
        }

        [Test]
        public async Task CreateNrsMapContainerTest()
        {
            var session = await TestUtils.CreateTestApp();
            var name = TestUtils.GetRandomString(5);

            var link = await CreateFilesContainerAsync(session);

            var api = session.Nrs;
            var (nrsMapRaw, processedEntries, xorUrl) = await api.CreateNrsMapContainerAsync(
                name,
                $"{link}?v=0",
                false,
                DryRun,
                SetDefault);

            Assert.IsNotNull(nrsMapRaw);
            Assert.IsNotNull(processedEntries);
            Assert.IsNotNull(xorUrl);

            await ValidateXorUrlAsync(xorUrl, ContentType.NrsMapContainer, 1500);
            await ValidateRawNrsMapAsync(nrsMapRaw);
        }

        [Test]
        public async Task AddToNrsMapContainerTest()
        {
            var session = await TestUtils.CreateTestApp();
            var name = TestUtils.GetRandomString(5);
            var xorUrlResult = await CreateNrsMapContainerXorUrlAsync(session, name);

            var link = await CreateFilesContainerAsync(session);

            var (nrsMapRaw, xorUrl, version) = await session.Nrs.AddToNrsMapContainerAsync(
                name,
                $"{link}?v=0",
                SetDefault,
                DirectLink,
                DryRun);

            Assert.IsNotNull(nrsMapRaw);
            Assert.IsNotEmpty(xorUrl);
            Assert.AreEqual(1, version);

            await ValidateXorUrlAsync(xorUrl, ContentType.NrsMapContainer, 1500);
            await ValidateRawNrsMapAsync(nrsMapRaw);
        }

        [Test]
        public async Task RemoveFromNrsMapContainerTest()
        {
            var session = await TestUtils.CreateTestApp();
            var name = TestUtils.GetRandomString(5);
            var xorUrlResult = await CreateNrsMapContainerXorUrlAsync(session, name);

            var (nrsMapRaw, xorUrl, version) = await session.Nrs.RemoveFromNrsMapContainerAsync(name, DryRun);

            Assert.IsNotNull(nrsMapRaw);
            Assert.IsNotEmpty(xorUrl);
            Assert.AreEqual(1, version);

            await ValidateXorUrlAsync(xorUrl, ContentType.NrsMapContainer, 1500);
            Assert.AreEqual("{\"sub_names_map\":{},\"default\":\"NotSet\"}", nrsMapRaw);
        }

        [Test]
        public async Task GetNrsMapContainerTest()
        {
            var session = await TestUtils.CreateTestApp();
            var name = TestUtils.GetRandomString(5);
            var xorUrl = await CreateNrsMapContainerXorUrlAsync(session, name);

            var api = session.Nrs;
            var (nrsMapRaw, version) = await api.GetNrsMapContainerAsync(xorUrl);

            Assert.IsNotNull(nrsMapRaw);
            Assert.AreEqual(0, version);

            await ValidateXorUrlAsync(xorUrl, ContentType.NrsMapContainer, 1500);
            await ValidateRawNrsMapAsync(nrsMapRaw);
        }

        async Task ValidateRawNrsMapAsync(string nrsMapRaw)
        {
            var nrsMap = Serialization.Deserialize<NrsMap>(nrsMapRaw);

            Assert.IsNull(nrsMap.SubNamesMap);
            /**
            foreach (var entry in nrsMap.SubNamesMap.Values)
            {
                Assert.IsNotNull(entry.SubName);
                Assert.IsNotNull(entry.SubNameRdf);
            }
            **/
            Assert.IsNotNull(nrsMap.Default);
            Assert.AreEqual(1, nrsMap.Default.Count);
            foreach (var rdf in nrsMap.Default.Values)
            {
                Assert.AreNotEqual(default(DateTime), rdf.Created);
                Assert.AreNotEqual(default(DateTime), rdf.Modified);
                Assert.IsNotNull(rdf.Link);
                await ValidateXorUrlAsync(rdf.Link, ContentType.FilesContainer, 1100);
            }
        }

        async Task ValidateXorUrlAsync(string xorUrl, ContentType expectedContentType, int expectedTypeTag)
        {
            var encoder = await XorEncoder.XorUrlEncoderFromUrl(xorUrl);
            ValidateEncoder(encoder, expectedContentType, expectedTypeTag);
        }

        enum ContentType
        {
            Raw,
            Wallet,
            FilesContainer,
            NrsMapContainer,
        }

        void ValidateEncoder(XorUrlEncoder encoder, ContentType expectedContentType, int expectedTypeTag)
        {
            Assert.AreEqual((ushort)expectedContentType, encoder.ContentType);
            Assert.AreEqual(0, encoder.ContentVersion);
            Assert.AreEqual(5, encoder.DataType);
            Assert.AreEqual(1, encoder.EncodingVersion);

            // todo: these need to be validated once they contain the correct values

            /**
            Assert.AreEqual(string.Empty, encoder.Path);
            Assert.AreEqual(string.Empty, encoder.SubNames);
            **/
            Assert.AreEqual(expectedTypeTag, encoder.TypeTag);
            TestUtils.ValidateXorName(encoder.Xorname);
        }

        async Task<string> CreateFilesContainerAsync(Session session)
        {
            var (xorUrl, _, _) = await session.Files.FilesContainerCreateAsync(
                TestUtils.TestDataDir,
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
