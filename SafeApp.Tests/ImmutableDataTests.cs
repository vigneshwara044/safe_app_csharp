using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.Misc;

namespace SafeApp.Tests {
  [TestFixture]
  internal class ImmutableDataTests {
    [Test]
    public async Task WriteAndReadUsingPainText() {
      var data = new byte[1024];
      new Random().NextBytes(data);
      var session = await MockAuthBindings.MockSession.CreateTestApp();
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
  }
}
