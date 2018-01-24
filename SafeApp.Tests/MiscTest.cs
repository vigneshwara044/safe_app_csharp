using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.MockAuthBindings;
using SafeApp.Utilities;

#if __ANDROID__ || __IOS__
using System;
#endif
#if __ANDROID__
using Android.App;
#endif

namespace SafeApp.Tests {
  [TestFixture]
  internal class MiscTest {
    [Test]
    public async Task AccessAppConatinerTest() {
      var authReq = new AuthReq {
        App = new AppExchangeInfo {Id = Utils.GetRandomString(10), Name = Utils.GetRandomString(10), Vendor = Utils.GetRandomString(10)},
        AppContainer = true,
        Containers = new List<ContainerPermissions>()
      };
      var session = await Utils.CreateTestApp(authReq);
      var mDataInfo = await session.AccessContainer.GetMDataInfoAsync("apps/" + authReq.App.Id);
      var perms = await session.MData.ListUserPermissionsAsync(mDataInfo, await session.Crypto.AppPubSignKeyAsync());
      Assert.IsTrue(perms.Insert);
      Assert.IsTrue(perms.Update);
      Assert.IsTrue(perms.Delete);
      Assert.IsTrue(perms.Read);
      Assert.IsTrue(perms.ManagePermissions);
      var keys = await session.MData.ListKeysAsync(mDataInfo);
      Assert.AreEqual(0, keys.Count);
      using (var entriesActionHandle = await session.MDataEntryActions.NewAsync()) {
        var encKey = await session.MDataInfoActions.EncryptEntryKeyAsync(mDataInfo, Utils.GetRandomData(15).ToList());
        var encVal = await session.MDataInfoActions.EncryptEntryKeyAsync(mDataInfo, Utils.GetRandomData(25).ToList());
        await session.MDataEntryActions.InsertAsync(entriesActionHandle, encKey, encVal);
        await session.MData.MutateEntriesAsync(mDataInfo, entriesActionHandle);
      }

      using (var entriesActionHandle = await session.MDataEntryActions.NewAsync())
      using (var entryHandle = await session.MData.ListEntriesAsync(mDataInfo)) {
        keys = await session.MData.ListKeysAsync(mDataInfo);
        var value = await session.MDataEntries.GetAsync(entryHandle, keys[0].Val);
        await session.MDataEntryActions.UpdateAsync(entriesActionHandle, keys[0].Val, Utils.GetRandomData(10).ToList(), value.Item2 + 1);
        await session.MData.MutateEntriesAsync(mDataInfo, entriesActionHandle);
      }

      using (var entriesActionHandle = await session.MDataEntryActions.NewAsync())
      using (var entryHandle = await session.MData.ListEntriesAsync(mDataInfo)) {
        keys = await session.MData.ListKeysAsync(mDataInfo);
        var value = await session.MDataEntries.GetAsync(entryHandle, keys[0].Val);
        await session.MDataEntryActions.DeleteAsync(entriesActionHandle, keys[0].Val, value.Item2 + 1);
        await session.MData.MutateEntriesAsync(mDataInfo, entriesActionHandle);
      }
    }

    [Test]
    public void IsMockBuildTest() {
      Assert.AreEqual(true, Authenticator.IsMockBuild());
    }

    [Test]
    public void RustLoggerTest() {
#if __IOS__
      var configPath = Environment.GetFolderPath(Environment.SpecialFolder.Resources);
      using (var reader = new StreamReader(Path.Combine(".", "log.toml"))) {
#elif __ANDROID__
      var configPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
      using (var reader = new StreamReader(Application.Context.Assets.Open("log.toml"))) {
#else
      var configPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
      Directory.CreateDirectory(configPath);
      var srcPath = Path.Combine(Directory.GetParent(typeof(MiscTest).Assembly.Location).FullName, "log.toml");
      using (var reader = new StreamReader(srcPath)) {
#endif
        using (var writer = new StreamWriter(Path.Combine(configPath, "log.toml"))) {
          writer.Write(reader.ReadToEnd());
          writer.Close();
        }

        reader.Close();
      }

      Assert.DoesNotThrowAsync(async () => await Session.InitLoggingAsync(configPath));
      Assert.ThrowsAsync<IpcMsgException>(async () => await Session.DecodeIpcMessageAsync("Some Random Invalid String"));
      using (var fs = new FileStream(Path.Combine(configPath, "Client.log"), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
      using (var sr = new StreamReader(fs, Encoding.Default))
      {
        Assert.IsFalse(string.IsNullOrEmpty(sr.ReadToEnd()));
      }
    }
  }
}
