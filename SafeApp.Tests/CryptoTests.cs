using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.Utilities;

namespace SafeApp.Tests {
  [TestFixture]
  internal class CryptoTests {
    [Test]
    public async Task EncryptKeyPair() {
      var session = await Utils.CreateTestApp();
      var encKeyPairTuple = await session.Crypto.EncGenerateKeyPairAsync();
      using (encKeyPairTuple.Item1)
      using (encKeyPairTuple.Item2) {
        var rawKey = await session.Crypto.EncPubKeyGetAsync(encKeyPairTuple.Item1);
        Assert.AreEqual(rawKey.Length, AppConstants.AsymPublicKeyLen);
        using (var handle = await session.Crypto.EncPubKeyNewAsync(rawKey)) {
          Assert.NotNull(handle);
        }

        rawKey = await session.Crypto.EncSecretKeyGetAsync(encKeyPairTuple.Item2);
        Assert.AreEqual(rawKey.Length, AppConstants.AsymSecretKeyLen);
        using (var handle = await session.Crypto.EncSecretKeyNewAsync(rawKey)) {
          Assert.NotNull(handle);
        }
      }
      session.Dispose();
    }

    [Test]
    public async Task SignKeyPair()
    {
      var session = await Utils.CreateTestApp();
      var signKeyPair = await session.Crypto.SignGenerateKeyPairAsync();
      using (signKeyPair.Item1)
      using (signKeyPair.Item2)
      {
        var rawKey = await session.Crypto.SignPubKeyGetAsync(signKeyPair.Item1);
        Assert.AreEqual(rawKey.Length, AppConstants.SignPublicKeyLen);
        using (var handle = await session.Crypto.SignPubKeyNewAsync(rawKey))
        {
          Assert.NotNull(handle);
        }

        rawKey = await session.Crypto.SignSecKeyGetAsync(signKeyPair.Item2);
        Assert.AreEqual(rawKey.Length, AppConstants.SignSecretKeyLen);
        using (var handle = await session.Crypto.SignSecKeyNewAsync(rawKey))
        {
          Assert.NotNull(handle);
        }
      }
      session.Dispose();
    }

    [Test]
    public async Task SealedBoxEncryption() {
      var session = await Utils.CreateTestApp();
      var encKeyPairTuple = await session.Crypto.EncGenerateKeyPairAsync();
      using (encKeyPairTuple.Item1)
      using (encKeyPairTuple.Item2) {
        var plainBytes = Utils.GetRandomString(100).ToUtfBytes();
        var cipherBytes = await session.Crypto.EncryptSealedBoxAsync(plainBytes, encKeyPairTuple.Item1);
        var decryptedBytes = await session.Crypto.DecryptSealedBoxAsync(cipherBytes, encKeyPairTuple.Item1, encKeyPairTuple.Item2);
        Assert.AreEqual(plainBytes, decryptedBytes);
        Assert.CatchAsync(async () => await session.Crypto.DecryptSealedBoxAsync(Utils.GetRandomString(10).ToUtfBytes(), encKeyPairTuple.Item1, encKeyPairTuple.Item2));
        cipherBytes = await session.Crypto.EncryptSealedBoxAsync(new List<byte>(), encKeyPairTuple.Item1);
        await session.Crypto.DecryptSealedBoxAsync(cipherBytes, encKeyPairTuple.Item1, encKeyPairTuple.Item2);
      }

      session.Dispose();
    }

    [Test]
    public async Task SignAndVerify() {
      var session = await Utils.CreateTestApp();
      var signKeyPairTuple = await session.Crypto.SignGenerateKeyPairAsync();
      using (signKeyPairTuple.Item1)
      using (signKeyPairTuple.Item2) {
        var plainBytes = Utils.GetRandomString(200).ToUtfBytes();
        var signedData = await session.Crypto.SignAsync(plainBytes, signKeyPairTuple.Item2);
        var verifiedData = await session.Crypto.VerifyAsync(signedData, signKeyPairTuple.Item1);
        Assert.AreEqual(plainBytes, verifiedData);
        Assert.CatchAsync(async () => await session.Crypto.VerifyAsync(Utils.GetRandomString(20).ToUtfBytes(), signKeyPairTuple.Item1));
      }

      session.Dispose();
    }

    [Test]
    public async Task SymmetricEncryption() {
      var session = await Utils.CreateTestApp();
      var encKeyPairTuple = await session.Crypto.EncGenerateKeyPairAsync();
      using (encKeyPairTuple.Item1)
      using (encKeyPairTuple.Item2) {
        var plainBytes = Utils.GetRandomString(200).ToUtfBytes();
        var cipherBytes = await session.Crypto.EncryptAsync(plainBytes.ToList(), encKeyPairTuple.Item1, encKeyPairTuple.Item2);
        var decryptedBytes = await session.Crypto.DecryptAsync(cipherBytes, encKeyPairTuple.Item1, encKeyPairTuple.Item2);
        Assert.AreEqual(plainBytes, decryptedBytes);
        Assert.CatchAsync(async () => await session.Crypto.DecryptAsync(Utils.GetRandomString(20).ToUtfBytes(), encKeyPairTuple.Item1, encKeyPairTuple.Item2));
        Assert.CatchAsync(async () => await session.Crypto.DecryptAsync(cipherBytes, encKeyPairTuple.Item1, encKeyPairTuple.Item1));
      }

      session.Dispose();
    }
  }
}
