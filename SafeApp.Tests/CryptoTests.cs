using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.Misc;

namespace SafeApp.Tests {
  [TestFixture]
  internal class CryptoTests {
    private const int EncKeySize = 32;

    [Test]
    public async Task GenerateEncKeyPair() {
      var session = await MockAuthBindings.MockSession.CreateTestApp();
      var encKeyPairTuple = await session.Crypto.EncGenerateKeyPairAsync();
      Assert.NotNull(encKeyPairTuple.Item1);
      Assert.NotNull(encKeyPairTuple.Item2);
      await session.Crypto.EncPubKeyFreeAsync(encKeyPairTuple.Item1);
      await session.Crypto.EncSecretKeyFreeAsync(encKeyPairTuple.Item2);
      session.FreeApp();
    }

    [Test]
    public async Task GetAppPubSignKey() {
      var session = await MockAuthBindings.MockSession.CreateTestApp();
      using (var handle = await session.Crypto.AppPubSignKeyAsync()) {
        Assert.NotNull(handle);
      }
      session.FreeApp();
    }

    [Test]
    public async Task GetPublicEncryptKey() {
      var session = await MockAuthBindings.MockSession.CreateTestApp();
      var encKeyPairTuple = await session.Crypto.EncGenerateKeyPairAsync();
      using (encKeyPairTuple.Item1)
      using (encKeyPairTuple.Item2)
      {
        var rawKey = await session.Crypto.EncPubKeyGetAsync(encKeyPairTuple.Item1);
        Assert.AreEqual(rawKey.Count, EncKeySize);
        var handle = await session.Crypto.EncPubKeyNewAsync(rawKey);
        Assert.NotNull(handle);
        rawKey = await session.Crypto.EncSecretKeyGetAsync(encKeyPairTuple.Item2);
        Assert.AreEqual(rawKey.Count, EncKeySize);
        handle = await session.Crypto.EncSecretKeyNewAsync(rawKey);
        Assert.NotNull(handle);
        session.FreeApp();
      }
    }

    [Test]
    public async Task SealedBoxEncryption() {
      var session = await MockAuthBindings.MockSession.CreateTestApp();
      var encKeyPairTuple = await session.Crypto.EncGenerateKeyPairAsync();
      Assert.NotNull(encKeyPairTuple.Item1);
      Assert.NotNull(encKeyPairTuple.Item2);
      var plainBytes = new byte[1024];
      new Random().NextBytes(plainBytes);
      var cipherBytes = await session.Crypto.EncryptSealedBoxAsync(plainBytes.ToList(), encKeyPairTuple.Item1);
      var decryptedBytes = await session.Crypto.DecryptSealedBoxAsync(cipherBytes, encKeyPairTuple.Item1, encKeyPairTuple.Item2);
      Assert.AreEqual(plainBytes, decryptedBytes);
      await session.Crypto.EncPubKeyFreeAsync(encKeyPairTuple.Item1);
      await session.Crypto.EncSecretKeyFreeAsync(encKeyPairTuple.Item2);
      session.FreeApp();
    }
  }
}
