﻿using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.API;

namespace SafeApp.Tests
{
    [TestFixture]
    internal class NrsTest
    {
        [Test]
        public async Task ParseUrlTest()
        {
            var session = await TestUtils.CreateTestApp();
            var (xorurl, _) = await session.Keys.KeysCreatePreloadTestCoinsAsync("1");

            var xorUrlEncoder = await Nrs.ParseUrlAsync(xorurl);
        }

        [Test]
        public async Task ParseAndResolveUrlTest()
        {
            var session = await TestUtils.CreateTestApp();
            var (xorurl, _) = await session.Keys.KeysCreatePreloadTestCoinsAsync("1");

            var api = session.Nrs;
            var (xorUrlEncoder, resolvesAsNrs) = await api.ParseAndResolveUrlAsync(xorurl);
        }

        [Test]
        public async Task CreateNrsMapContainerTest()
        {
            var session = await TestUtils.CreateTestApp();
            var name = "someName";

            // todo: put file, use its address as link
            // in the meanwhile put the file through cli.
            var link = "safe://hnyynys9j9sd6ku5wu9pz6uodmwk5446e1ee7u4x5gtbhocmastfcjuiksbnc?v=0";

            var directLink = true;
            var dryRun = false;
            var setDefault = true;

            var api = session.Nrs;
            var (nrsMap, processedEntries, xorUrl) = await api.CreateNrsMapContainerAsync(name, link, directLink, dryRun, setDefault);

            Assert.IsNotNull(nrsMap);
            Assert.IsNotNull(processedEntries);
            Assert.IsNotNull(xorUrl);

            // todo: deserialize and test
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
            var name = "someName";

            // todo: put file, use its address as link
            // in the meanwhile put the file through cli.
            var link = "safe://hnyynys9j9sd6ku5wu9pz6uodmwk5446e1ee7u4x5gtbhocmastfcjuiksbnc?v=0";

            var setDefault = true;
            var directLink = true;
            var dryRun = false;

            var api = session.Nrs;
            var (nrsMap, xorUrl, version) = await api.AddToNrsMapContainerAsync(name, link, setDefault, directLink, dryRun);

            Assert.IsNotNull(nrsMap);
            Assert.IsNotEmpty(xorUrl);

            // todo: deserialize and test
            // nrsMap.SubNamesMap.SubNames.ForEach(s =>
            // {
            //     Assert.IsNotNull(s.SubName);
            //     Assert.IsNotNull(s.SubNameRdf);
            // });

            // todo: validate version
        }

        [Test]
        public async Task RemoveFromNrsMapContainerTest()
        {
            var session = await TestUtils.CreateTestApp();
            var name = "someName";

            var dryRun = false;

            var api = session.Nrs;
            var (nrsMap, xorUrl, version) = await api.RemoveFromNrsMapContainerAsync(name, dryRun);

            Assert.IsNotNull(nrsMap);
            Assert.IsNotEmpty(xorUrl);

            // todo: deserialize and validate
            // Assert.IsNotNull(nrsMap.SubNamesMap);
            // Assert.IsNotEmpty(nrsMap.SubNamesMap.SubNames);
            // Assert.IsNotNull(nrsMap.Default);
            // nrsMap.SubNamesMap.SubNames.ForEach(s =>
            // {
            //     Assert.IsNotNull(s.SubName);
            //     Assert.IsNotNull(s.SubNameRdf);
            // });

            // todo: validate version
        }

        [Test]
        public async Task GetNrsMapContainerTest()
        {
            var session = await TestUtils.CreateTestApp();
            var url = "safe://hnyynys9j9sd6ku5wu9pz6uodmwk5446e1ee7u4x5gtbhocmastfcjuiksbnc";

            var api = session.Nrs;
            var (nrsMap, version) = await api.GetNrsMapContainerAsync(url);

            Assert.IsNotNull(nrsMap);

            // todo: deserialize and validate
            // Assert.IsNotNull(nrsMap.SubNamesMap);
            // Assert.IsNotEmpty(nrsMap.SubNamesMap.SubNames);
            // Assert.IsNotNull(nrsMap.Default);
            // nrsMap.SubNamesMap.SubNames.ForEach(s =>
            // {
            //     Assert.IsNotNull(s.SubName);
            //     Assert.IsNotNull(s.SubNameRdf);
            // });

            // todo: validate version
        }
    }
}
