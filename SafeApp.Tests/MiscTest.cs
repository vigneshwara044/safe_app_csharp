using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.MockAuthBindings;
using SafeApp.Utilities;

#if __ANDROID__
using Android.App;
#endif

namespace SafeApp.Tests {
  [TestFixture]
  internal class MiscTest {
    [Test, Ignore("")]
    public async Task AccessAppConatinerTest() {
      var authReq = new AuthReq {
        App = new AppExchangeInfo {Id = Utils.GetRandomString(10), Name = Utils.GetRandomString(10), Vendor = Utils.GetRandomString(10)},
        AppContainer = true,
        Containers = new List<ContainerPermissions>()
      };
      var session = await Utils.CreateTestApp(authReq);
      var mDataInfo = await session.AccessContainer.GetMDataInfoAsync("apps/" + authReq.App.Id);
      var keys = await session.MData.ListKeysAsync(ref mDataInfo);
      Assert.AreEqual(0, keys.Count);
      using (var entriesActionHandle = await session.MDataEntryActions.NewAsync()) {
        var encKey = await session.MDataInfoActions.EncryptEntryKeyAsync(mDataInfo, Utils.GetRandomString(15).ToUtfBytes());
        var encVal = await session.MDataInfoActions.EncryptEntryKeyAsync(mDataInfo, Utils.GetRandomString(25).ToUtfBytes());
        await session.MDataEntryActions.InsertAsync(entriesActionHandle, encKey, encVal);
        await session.MData.MutateEntriesAsync(ref mDataInfo, entriesActionHandle);
      }

      using (var entriesActionHandle = await session.MDataEntryActions.NewAsync())
      using (var entryHandle = await session.MData.ListEntriesAsync(mDataInfo)) {
        keys = await session.MData.ListKeysAsync(ref mDataInfo);
        var value = await session.MDataEntries.GetAsync(entryHandle, keys[0].Val);
        await session.MDataEntryActions.UpdateAsync(entriesActionHandle, keys[0].Val, Utils.GetRandomString(10).ToUtfBytes(), value.Item2);
        await session.MData.MutateEntriesAsync(ref mDataInfo, entriesActionHandle);
      }

      using (var entriesActionHandle = await session.MDataEntryActions.NewAsync())
      using (var entryHandle = await session.MData.ListEntriesAsync(mDataInfo)) {
        keys = await session.MData.ListKeysAsync(ref mDataInfo);
        var value = await session.MDataEntries.GetAsync(entryHandle, keys[0].Val);
        await session.MDataEntryActions.DeleteAsync(entriesActionHandle, keys[0].Val, value.Item2);
        await session.MData.MutateEntriesAsync(ref mDataInfo, entriesActionHandle);
      }
    }

    [Test]
    public void IsMockBuildTest() {
      Assert.AreEqual(true, Authenticator.IsMockBuild());
    }

    [Test, Ignore("")]
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
      using (var reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("SafeApp.Tests.log.toml"))) {
#endif
        using (var writer = new StreamWriter(Path.Combine(configPath, "log.toml"))) {
          writer.Write(reader.ReadToEnd());
          writer.Close();
        }

        reader.Close();
      }

      Assert.DoesNotThrowAsync(async () => await Session.InitLoggingAsync(configPath));
      Assert.ThrowsAsync<Exception>(async () => await Session.DecodeIpcMessageAsync("Some Random Invalid String"));
      Assert.IsFalse(string.IsNullOrEmpty(System.IO.File.ReadAllText(Path.Combine(configPath, "Client.log"))));
    }
  }
}
