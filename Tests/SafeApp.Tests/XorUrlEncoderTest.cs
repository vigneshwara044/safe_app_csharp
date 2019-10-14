using System;
using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.API;
using SafeApp.Core;

namespace SafeApp.Tests
{
    [TestFixture]
    public class XorUrlEncoderTest
    {
        [Test]
        public async Task EncodeStringTestAsync()
        {
            Random rnd = new Random();
            var xorName = new byte[32];
            rnd.NextBytes(xorName);
            var contentType = ContentType.Wallet;
            var dataType = DataType.UnpublishedImmutableData;
            var encodedString = await XorEncoder.EncodeAsync(
                xorName,
                16000,
                dataType,
                contentType,
                null,
                null,
                0,
                "base32z");
            Assert.IsNotNull(encodedString);
            Assert.IsTrue(encodedString.StartsWith("safe://", StringComparison.Ordinal));

            var xorEncoder = await XorEncoder.EncodeAsync(
                xorName,
                16000,
                dataType,
                contentType,
                null,
                null,
                0);

            Assert.AreEqual(xorName, xorEncoder.XorName);
            Assert.AreNotEqual(default(XorUrlEncoder), xorEncoder);
            Assert.AreNotEqual(0, xorEncoder.TypeTag);

            var encoder = await XorEncoder.XorUrlEncoderFromUrl(encodedString);

            Assert.AreEqual(xorName, encoder.XorName);
            Assert.AreNotEqual(default(XorUrlEncoder), encoder);
            Assert.AreEqual(16000, encoder.TypeTag);
            Assert.AreEqual(dataType, encoder.DataType);
            Assert.AreEqual(contentType, encoder.ContentType);
            Assert.AreEqual(0, encoder.ContentVersion);
        }
    }
}
