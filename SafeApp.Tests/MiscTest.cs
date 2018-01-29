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

// ReSharper disable AccessToDisposedClosure

namespace SafeApp.Tests {
  [TestFixture]
  internal class MiscTest {
    [Test]
    public async Task AppConatinerTest() {
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
      Assert.That(Authenticator.IsMockBuild(), Is.True);
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

      Assert.That(async () => await Session.InitLoggingAsync(configPath), Throws.Nothing);
      Assert.That(async () => await Session.DecodeIpcMessageAsync("Some Random Invalid String"), Throws.TypeOf<IpcMsgException>());
      using (var fs = new FileStream(Path.Combine(configPath, "Client.log"), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
      using (var sr = new StreamReader(fs, Encoding.Default))
      {
        Assert.That(string.IsNullOrEmpty(sr.ReadToEnd()), Is.False);
      }
    }

    [Test]
    public async Task AppScopeTest() {
      var authReq = new AuthReq {
        App = new AppExchangeInfo {Id = "net.maidsafe.scope.test", Name = "SampleTest", Scope = "Web", Vendor = "MaidSafe.net Ltd"},
        AppContainer = true,
        Containers = new List<ContainerPermissions>()
      };
      var session = await Utils.CreateTestApp(authReq);
      var mdInfoWeb = await session.AccessContainer.GetMDataInfoAsync("apps/" + authReq.App.Id);
      var keys = await session.MData.ListKeysAsync(mdInfoWeb);
      Assert.That(0, Is.EqualTo(keys.Count));
      session.Dispose();
      authReq.App.Scope = "mobile";
      session = await Utils.CreateTestApp(authReq);
      var mdInfoMobile = await session.AccessContainer.GetMDataInfoAsync("apps/" + authReq.App.Id);
      Assert.That(mdInfoMobile.Name, Is.Not.EqualTo(mdInfoWeb.Name));
      session.Dispose();
    }

    [Test]
    public async Task AccountOverflowTest() {
      var session = await Utils.CreateTestApp();
      var mdInfo = await session.MDataInfoActions.RandomPublicAsync(16000);
      var accountInfo = await session.GetAccountInfoAsyc();
      Assert.That(987, Is.EqualTo(accountInfo.MutationsAvailable));
      Assert.That(13, Is.EqualTo(accountInfo.MutationsDone));
      using (var permissionsHandle = await session.MDataPermissions.NewAsync())
      using (var userHandle = await session.Crypto.AppPubSignKeyAsync()) {
        await session.MDataPermissions.InsertAsync(permissionsHandle, userHandle, new PermissionSet { Insert = true, Delete = true });
        await session.MData.PutAsync(mdInfo, permissionsHandle, NativeHandle.Zero);
        for (var i = 0; i < (long)accountInfo.MutationsAvailable - 1; i++) {
          var entryHandle = await session.MDataEntryActions.NewAsync();
          await session.MDataEntryActions.InsertAsync(entryHandle, Utils.GetRandomData(10).ToList(), Utils.GetRandomData(15).ToList());
          await session.MData.MutateEntriesAsync(mdInfo, entryHandle);
          entryHandle.Dispose();
        }
      }
      accountInfo = await session.GetAccountInfoAsyc();
      Assert.That(1000, Is.EqualTo(accountInfo.MutationsDone));
      Assert.That(0, Is.EqualTo(accountInfo.MutationsAvailable));
      using (var entryActionHandle = await session.MDataEntryActions.NewAsync()) {
        await session.MDataEntryActions.InsertAsync(entryActionHandle, Utils.GetRandomData(10).ToList(), Utils.GetRandomData(15).ToList());
        Assert.That(async () => {
          await session.MData.MutateEntriesAsync(mdInfo, entryActionHandle);
        }, Throws.TypeOf<FfiException>());
      }
      session.Dispose();
    }
  }
}
