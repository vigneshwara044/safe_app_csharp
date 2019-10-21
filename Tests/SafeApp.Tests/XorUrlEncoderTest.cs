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
            var rnd = new Random();
            var xorName = new byte[32];
            rnd.NextBytes(xorName);
            var typeTag = 16000UL;
            var contentType = ContentType.Wallet;
            var dataType = DataType.UnpublishedImmutableData;
            var encodedString = await XorEncoder.EncodeAsync(
                xorName,
                typeTag,
                dataType,
                contentType,
                null,
                null,
                0,
                "base32z");
            Assert.IsNotNull(encodedString);
            Assert.IsTrue(encodedString.StartsWith("safe://", StringComparison.Ordinal));

            var xorUrlEncoder = await XorEncoder.EncodeAsync(
                xorName,
                typeTag,
                dataType,
                contentType,
                null,
                null,
                0);

            Assert.AreEqual(xorName, xorUrlEncoder.XorName);
            Validate.Encoder(xorUrlEncoder, dataType, (ContentType)contentType, typeTag);

            var parsedEncoder = await XorEncoder.XorUrlEncoderFromUrl(encodedString);

            Assert.AreEqual(xorName, parsedEncoder.XorName);
            Validate.Encoder(parsedEncoder, dataType, contentType, typeTag);
            Assert.AreEqual(typeTag, parsedEncoder.TypeTag);
            Assert.AreEqual(contentType, parsedEncoder.ContentType);
            Assert.AreEqual(0, parsedEncoder.ContentVersion);
        }
    }
}
