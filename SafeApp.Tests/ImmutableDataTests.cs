using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.MockAuthBindings;

namespace SafeApp.Tests {
  [TestFixture]
  internal class ImmutableDataTests {
    [Test]
    public async Task WriteAndReadUsingPainCipherOpt() {
      var data = new byte[1024];
      new Random().NextBytes(data);
      var session = new Session(MockAuthResolver.Current.TestCreateApp());
      using (var cipherOptHandle = await session.CipherOpt.NewPlaintextAsync()) {
        var seHandle = await session.IData.NewSelfEncryptorAsync();
        await session.IData.WriteToSelfEncryptorAsync(seHandle, data.ToList());
        var dataMapAddress = await session.IData.CloseSelfEncryptorAsync(seHandle, cipherOptHandle);
        var seReaderHandle = await session.IData.FetchSelfEncryptorAsync(dataMapAddress);
        var len = await session.IData.SizeAsync(seReaderHandle);
        var readData = await session.IData.ReadFromSelfEncryptorAsync(seReaderHandle, 0, len);
        Assert.AreEqual(data, readData);
      }
      session.FreeApp();
    }

    [Test]
    public async Task WriteAndReadUsingSymmetricCipherOpt()
    {
      var data = new byte[1024];
      new Random().NextBytes(data);
      var session = new Session(MockAuthResolver.Current.TestCreateApp());
      using (var cipherOptHandle = await session.CipherOpt.NewSymmetricAsync())
      {
        var seHandle = await session.IData.NewSelfEncryptorAsync();
        await session.IData.WriteToSelfEncryptorAsync(seHandle, data.ToList());
        var dataMapAddress = await session.IData.CloseSelfEncryptorAsync(seHandle, cipherOptHandle);
        var seReaderHandle = await session.IData.FetchSelfEncryptorAsync(dataMapAddress);
        var len = await session.IData.SizeAsync(seReaderHandle);
        var readData = await session.IData.ReadFromSelfEncryptorAsync(seReaderHandle, 0, len);
        Assert.AreEqual(data, readData);
      }
      session.FreeApp();
    }

    [Test]
    public async Task WriteAndReadUsingAsymmetricCipherOpt()
    {
      var data = new byte[1024];
      new Random().NextBytes(data);
      var session = new Session(MockAuthResolver.Current.TestCreateApp());
      var encKeyPair = await session.Crypto.EncGenerateKeyPairAsync();
      using (encKeyPair.Item1)
      using (encKeyPair.Item2)
      using (var cipherOptHandle = await session.CipherOpt.NewAsymmetricAsync(encKeyPair.Item1))
      {
        var seHandle = await session.IData.NewSelfEncryptorAsync();
        await session.IData.WriteToSelfEncryptorAsync(seHandle, data.ToList());
        var dataMapAddress = await session.IData.CloseSelfEncryptorAsync(seHandle, cipherOptHandle);
        var seReaderHandle = await session.IData.FetchSelfEncryptorAsync(dataMapAddress);
        var len = await session.IData.SizeAsync(seReaderHandle);
        var readData = await session.IData.ReadFromSelfEncryptorAsync(seReaderHandle, 0, len);
        Assert.AreEqual(data, readData);
      }
      session.FreeApp();
    }
  }
}
