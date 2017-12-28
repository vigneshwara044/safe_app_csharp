using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.MData;
using SafeApp.Misc;
using SafeApp.Utilities;

namespace SafeApp.Tests {
  [TestFixture]
  internal class MutableDataTests {
    [Test]
    public async Task RandomPrivateMutableDataUpdateAction() {
      var session = await MockAuthBindings.MockSession.CreateTestApp();
      const ulong tagType = 15001;
      const string actKey = "sample_key";
      const string actValue = "sample_value";
      var mdInfo = await session.MDataInfoActions.RandomPrivateAsync(tagType);
      var mDataPermissionSet = new PermissionSet() {Insert = true, ManagePermissions = true, Read = true, Delete = false, Update = false};
      using (var permissionsH = await session.MDataPermissions.NewAsync())
      {
        using (var appSignKeyH = await session.Crypto.AppPubSignKeyAsync())
        {
          await session.MDataPermissions.InsertAsync(permissionsH, appSignKeyH, ref mDataPermissionSet);
          await session.MData.PutAsync(ref mdInfo, permissionsH, NativeHandle.Zero);
        }
      }

      using (var entryActionsH = await session.MDataEntryActions.NewAsync())
      {
        var key = Encoding.Default.GetBytes(actKey).ToList();
        var value = Encoding.Default.GetBytes(actValue).ToList();
        key = await session.MDataInfoActions.EncryptEntryKeyAsync(mdInfo, key);
        value = await session.MDataInfoActions.EncryptEntryValueAsync(mdInfo, value);
        await session.MDataEntryActions.InsertAsync(entryActionsH, key, value);
        await session.MData.MutateEntriesAsync(ref mdInfo, entryActionsH);
      }

      var keys = await session.MData.ListKeysAsync(mdInfo);
      Assert.AreEqual(1, keys.Count);

      foreach (var key in keys) {
        var (value, version) = await session.MData.GetValueAsync(mdInfo, key.Val.ToList());
        var decryptedKey = await session.MDataInfoActions.DecryptAsync(mdInfo, key.Val.ToList());
        var decryptedValue = await session.MDataInfoActions.DecryptAsync(mdInfo, value.ToList());
        Assert.AreEqual(actKey, Encoding.Default.GetString(decryptedKey.ToArray()));
        Assert.AreEqual(actValue, Encoding.Default.GetString(decryptedValue.ToArray()));
      }
    }

    [Test]
    public async Task RandomPublicMutableDataInsertAction() {
      var session = await MockAuthBindings.MockSession.CreateTestApp();
      const ulong tagType = 15001;
      const string actKey = "sample_key";
      const string actValue = "sample_value";
      var mdInfo = await session.MDataInfoActions.RandomPublicAsync(tagType);
      var mDataPermissionSet = new PermissionSet() { Insert = true, ManagePermissions = true, Read = true, Delete = false, Update = false };
      using (var permissionsH = await session.MDataPermissions.NewAsync())
      {
        using (var appSignKeyH = await session.Crypto.AppPubSignKeyAsync())
        {
          await session.MDataPermissions.InsertAsync(permissionsH, appSignKeyH, ref mDataPermissionSet);
          await session.MData.PutAsync(ref mdInfo, permissionsH, NativeHandle.Zero);
        }
      }

      using (var entryActionsH = await session.MDataEntryActions.NewAsync())
      {
        var key = Encoding.Default.GetBytes(actKey).ToList();
        var value = Encoding.Default.GetBytes(actValue).ToList();
        await session.MDataEntryActions.InsertAsync(entryActionsH, key, value);
        await session.MData.MutateEntriesAsync(ref mdInfo, entryActionsH);
      }

      var keys = await session.MData.ListKeysAsync(mdInfo);
      Assert.AreEqual(1, keys.Count);

      foreach (var key in keys)
      {
        var (value, version) = await session.MData.GetValueAsync(mdInfo, key.Val.ToList());
        Assert.AreEqual(actKey, Encoding.Default.GetString(key.Val.ToArray()));
        Assert.AreEqual(actValue, Encoding.Default.GetString(value.ToArray()));
      }
    }
  }
}
