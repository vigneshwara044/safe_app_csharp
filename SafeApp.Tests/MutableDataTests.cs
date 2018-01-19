using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.Utilities;

namespace SafeApp.Tests {
  [TestFixture]
  internal class MutableDataTests {
    [Test]
    public async Task RandomPrivateMutableDataUpdateAction() {
      var session = await Utils.CreateTestApp();
      const ulong tagType = 15001;
      var actKey = Utils.GetRandomString(10);
      var actValue = Utils.GetRandomString(10);
      var mdInfo = await session.MDataInfoActions.RandomPrivateAsync(tagType);
      var mDataPermissionSet = new PermissionSet {Insert = true, ManagePermissions = true, Read = true};
      using (var permissionsH = await session.MDataPermissions.NewAsync()) {
        using (var appSignKeyH = await session.Crypto.AppPubSignKeyAsync()) {
          await session.MDataPermissions.InsertAsync(permissionsH, appSignKeyH, ref mDataPermissionSet);
          await session.MData.PutAsync(ref mdInfo, permissionsH, NativeHandle.Zero);
        }
      }

      using (var entryActionsH = await session.MDataEntryActions.NewAsync()) {
        var key = Encoding.ASCII.GetBytes(actKey).ToList();
        var value = Encoding.ASCII.GetBytes(actValue).ToList();
        key = await session.MDataInfoActions.EncryptEntryKeyAsync(mdInfo, key);
        value = await session.MDataInfoActions.EncryptEntryValueAsync(mdInfo, value);
        await session.MDataEntryActions.InsertAsync(entryActionsH, key, value);
        await session.MData.MutateEntriesAsync(ref mdInfo, entryActionsH);
      }

      var keys = await session.MData.ListKeysAsync(ref mdInfo);
      Assert.AreEqual(1, keys.Count);

      foreach (var key in keys) {
        var (value, _) = await session.MData.GetValueAsync(mdInfo, key.Val.ToList());
        var decryptedKey = await session.MDataInfoActions.DecryptAsync(mdInfo, key.Val.ToList());
        var decryptedValue = await session.MDataInfoActions.DecryptAsync(mdInfo, value.ToList());
        Assert.AreEqual(actKey, Encoding.ASCII.GetString(decryptedKey.ToArray()));
        Assert.AreEqual(actValue, Encoding.ASCII.GetString(decryptedValue.ToArray()));
      }

      await session.MData.SerialisedSizeAsync(ref mdInfo);
      var serialisedData = await session.MDataInfoActions.SerialiseAsync(mdInfo);
      mdInfo = await session.MDataInfoActions.DeserialiseAsync(serialisedData);

      keys = await session.MData.ListKeysAsync(ref mdInfo);
      Assert.AreEqual(1, keys.Count);
    }

    [Test]
    public async Task RandomPublicMutableDataInsertAction() {
      var session = await Utils.CreateTestApp();
      const ulong tagType = 15010;
      var actKey = Utils.GetRandomString(10);
      var actValue = Utils.GetRandomString(10);
      var mdInfo = await session.MDataInfoActions.RandomPublicAsync(tagType);
      var mDataPermissionSet = new PermissionSet {Insert = true, ManagePermissions = true, Read = true};
      using (var permissionsH = await session.MDataPermissions.NewAsync()) {
        using (var appSignKeyH = await session.Crypto.AppPubSignKeyAsync()) {
          await session.MDataPermissions.InsertAsync(permissionsH, appSignKeyH, ref mDataPermissionSet);
          await session.MData.PutAsync(ref mdInfo, permissionsH, NativeHandle.Zero);
        }
      }

      using (var entryActionsH = await session.MDataEntryActions.NewAsync()) {
        var key = Encoding.ASCII.GetBytes(actKey).ToList();
        var value = Encoding.ASCII.GetBytes(actValue).ToList();
        await session.MDataEntryActions.InsertAsync(entryActionsH, key, value);
        await session.MData.MutateEntriesAsync(ref mdInfo, entryActionsH);
      }

      var keys = await session.MData.ListKeysAsync(ref mdInfo);
      Assert.AreEqual(1, keys.Count);

      foreach (var key in keys) {
        var (value, _) = await session.MData.GetValueAsync(mdInfo, key.Val.ToList());
        Assert.AreEqual(actKey, Encoding.ASCII.GetString(key.Val.ToArray()));
        Assert.AreEqual(actValue, Encoding.ASCII.GetString(value.ToArray()));
      }

      await session.MData.SerialisedSizeAsync(ref mdInfo);
      var serialisedData = await session.MDataInfoActions.SerialiseAsync(mdInfo);
      mdInfo = await session.MDataInfoActions.DeserialiseAsync(serialisedData);

      keys = await session.MData.ListKeysAsync(ref mdInfo);
      Assert.AreEqual(1, keys.Count);
    }

    [Test]
    public async Task SharedMutableData() {
      var locator = Utils.GetRandomString(10);
      var secret = Utils.GetRandomString(10);
      var authReq = new AuthReq {
        App = new AppExchangeInfo {Id = "net.maidsafe.sample", Name = "Inbox", Scope = null, Vendor = "MaidSafe.net Ltd"},
        Containers = new List<ContainerPermissions>()
      };
      var session = await Utils.CreateTestApp(locator, secret, authReq);
      var typeTag = 16000;
      var mDataInfo = await session.MDataInfoActions.RandomPrivateAsync((ulong)typeTag);
      using (var permissionsH = await session.MDataPermissions.NewAsync()) {
        using (var appSignKeyH = await session.Crypto.AppPubSignKeyAsync()) {
          var ownerPermission = new PermissionSet {Insert = true, ManagePermissions = true, Read = true};
          await session.MDataPermissions.InsertAsync(permissionsH, appSignKeyH, ref ownerPermission);
          var sharePermissions = new PermissionSet {Insert = true};
          await session.MDataPermissions.InsertAsync(permissionsH, NativeHandle.Zero, ref sharePermissions);
          await session.MData.PutAsync(ref mDataInfo, permissionsH, NativeHandle.Zero);
        }
      }

      using (var entriesHandle = await session.MDataEntryActions.NewAsync()) {
        var key = await session.MDataInfoActions.EncryptEntryKeyAsync(mDataInfo, Utils.GetRandomString(10).ToUtfBytes());
        var value = await session.MDataInfoActions.EncryptEntryValueAsync(mDataInfo, Utils.GetRandomString(10).ToUtfBytes());
        await session.MDataEntryActions.InsertAsync(entriesHandle, key, value);
        await session.MData.MutateEntriesAsync(ref mDataInfo, entriesHandle);
      }

      using (var entriesHandle = await session.MData.ListEntriesAsync(mDataInfo)) {
        var keys = await session.MData.ListKeysAsync(ref mDataInfo);
        foreach (var key in keys) {
          var encKey = await session.MDataEntries.GetAsync(entriesHandle, key.Val);
          await session.MDataInfoActions.DecryptAsync(mDataInfo, encKey.Item1);
        }
      }

      session.Dispose();

      authReq = new AuthReq {
        App = new AppExchangeInfo {Id = "net.maidsafe.share.md", Name = "Share Chat", Vendor = "MaidSafe.net Ltd"},
        AppContainer = false,
        Containers = new List<ContainerPermissions>()
      };
      session = await Utils.CreateTestApp(authReq);
      using (var entriesHandle = await session.MDataEntryActions.NewAsync()) {
        var key = await session.MDataInfoActions.EncryptEntryKeyAsync(mDataInfo, Utils.GetRandomString(10).ToUtfBytes());
        var value = await session.MDataInfoActions.EncryptEntryValueAsync(mDataInfo, Utils.GetRandomString(10).ToUtfBytes());
        await session.MDataEntryActions.InsertAsync(entriesHandle, key, value);
        await session.MData.MutateEntriesAsync(ref mDataInfo, entriesHandle);
      }

      using (var entriesHandle = await session.MData.ListEntriesAsync(mDataInfo)) {
        var keys = await session.MData.ListKeysAsync(ref mDataInfo);
        foreach (var key in keys) {
          var encKey = await session.MDataEntries.GetAsync(entriesHandle, key.Val);
          await session.MDataInfoActions.DecryptAsync(mDataInfo, encKey.Item1);
        }
      }

      using (var entryAction = await session.MDataEntryActions.NewAsync())
      using (var entriesHandle = await session.MData.ListEntriesAsync(mDataInfo))
      {
        var keys = await session.MData.ListKeysAsync(ref mDataInfo);
        foreach (var key in keys)
        {
          var encKey = await session.MDataEntries.GetAsync(entriesHandle, key.Val);
          await session.MDataEntryActions.DeleteAsync(entryAction, key.Val, encKey.Item2);
        }

        Assert.CatchAsync(async () => await session.MData.MutateEntriesAsync(ref mDataInfo, entryAction));
      }

      session.Dispose();
    }

    [Test, Ignore("")]
    public async Task AddRemoveUserPermission() {
      var locator = Utils.GetRandomString(10);
      var secret = Utils.GetRandomString(10);
      var authReq = new AuthReq {
        App = new AppExchangeInfo { Id = "net.maidsafe.mdata.permission", Name = "CMS", Vendor = "MaidSafe.net Ltd"},
        AppContainer = true,
        Containers = new List<ContainerPermissions>()
      };
      var cmsApp = await Utils.CreateTestApp(locator, secret, authReq);
      var mDataInfo = await Utils.PreparePublicDirectory(cmsApp);
      authReq.App.Name = "Hosting";
      authReq.App.Id = "net.maidsafe.mdata.host";
      var ipcMsg = await Session.EncodeAuthReqAsync(authReq);
      var response = await Utils.AuthenticateAuthRequest(locator, secret, ipcMsg.Item2, true);
      var decodedResponse = await Session.DecodeIpcMessageAsync(response) as AuthIpcMsg;
      Assert.NotNull(decodedResponse);
      var hostingApp = await Session.AppRegisteredAsync(authReq.App.Id, decodedResponse.AuthGranted);
      var ipcReq = await Session.EncodeShareMDataRequestAsync(new ShareMDataReq {
        App = authReq.App,
        MData = new List<ShareMData> {
          new ShareMData {
            Name = mDataInfo.Name,
            TypeTag = mDataInfo.TypeTag,
            Perms = new PermissionSet {  Insert = true, Read = true }
          }
        }
      });
      await Utils.AuthenticateShareMDataRequest(locator, secret, ipcReq.Item2, true);
      using (var entryhandle = await hostingApp.MData.ListEntriesAsync(mDataInfo))
      {
        await hostingApp.MDataEntryActions.InsertAsync(entryhandle, "default.html".ToUtfBytes(), "<html><body>Hello Default</body></html>".ToUtfBytes());
        await hostingApp.MData.MutateEntriesAsync(ref mDataInfo, entryhandle);
      }

      var version = await cmsApp.MData.GetVersionAsync(ref mDataInfo);
      using (var permissionHandle = await cmsApp.MData.ListPermissionsAsync(mDataInfo)) {
        var userPermissions = await cmsApp.MDataPermissions.ListAsync(permissionHandle);
        Assert.AreEqual(userPermissions.Count, await cmsApp.MDataPermissions.LenAsync(permissionHandle));
        var hostingAppPubSignKey = await hostingApp.Crypto.AppPubSignKeyAsync();
        await cmsApp.MData.DelUserPermissionsAsync(ref mDataInfo, hostingAppPubSignKey, version);
      }

      using (var entryhandle = await hostingApp.MData.ListEntriesAsync(mDataInfo))
      {
        await hostingApp.MDataEntryActions.InsertAsync(entryhandle, "home.html".ToUtfBytes(), "<html><body>Hello Home!</body></html>".ToUtfBytes());
        await hostingApp.MData.MutateEntriesAsync(ref mDataInfo, entryhandle);
      }
      cmsApp.Dispose();
      hostingApp.Dispose();
    }
  }
}
