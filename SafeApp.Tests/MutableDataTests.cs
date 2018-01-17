//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using NUnit.Framework;
//using SafeApp.Utilities;
//
//namespace SafeApp.Tests {
//  [TestFixture]
//  internal class MutableDataTests {
//
//    [Test]
//    public async Task RandomPrivateMutableDataUpdateAction() {
//      var session = Utils.RandomSession();
//      const ulong tagType = 15001;
//      var actKey = Utils.GetRandomString(10);
//      var actValue = Utils.GetRandomString(10);
//      var mdInfo = await session.MDataInfoActions.RandomPrivateAsync(tagType);
//      var mDataPermissionSet = new PermissionSet {Insert = true, ManagePermissions = true, Read = true, Delete = false, Update = false};
//      using (var permissionsH = await session.MDataPermissions.NewAsync()) {
//        using (var appSignKeyH = await session.Crypto.AppPubSignKeyAsync()) {
//          await session.MDataPermissions.InsertAsync(permissionsH, appSignKeyH, ref mDataPermissionSet);
//          await session.MData.PutAsync(ref mdInfo, permissionsH, NativeHandle.Zero);
//        }
//      }
//
//      using (var entryActionsH = await session.MDataEntryActions.NewAsync()) {
//        var key = Encoding.ASCII.GetBytes(actKey).ToList();
//        var value = Encoding.ASCII.GetBytes(actValue).ToList();
//        key = await session.MDataInfoActions.EncryptEntryKeyAsync(mdInfo, key);
//        value = await session.MDataInfoActions.EncryptEntryValueAsync(mdInfo, value);
//        await session.MDataEntryActions.InsertAsync(entryActionsH, key, value);
//        await session.MData.MutateEntriesAsync(ref mdInfo, entryActionsH);
//      }
//
//      var keys = await session.MData.ListKeysAsync(ref mdInfo);
//      Assert.AreEqual(1, keys.Count);
//
//      foreach (var key in keys) {
//        var (value, _) = await session.MData.GetValueAsync(mdInfo, key.Val.ToList());
//        var decryptedKey = await session.MDataInfoActions.DecryptAsync(mdInfo, key.Val.ToList());
//        var decryptedValue = await session.MDataInfoActions.DecryptAsync(mdInfo, value.ToList());
//        Assert.AreEqual(actKey, Encoding.ASCII.GetString(decryptedKey.ToArray()));
//        Assert.AreEqual(actValue, Encoding.ASCII.GetString(decryptedValue.ToArray()));
//      }
//
//      await session.MData.SerialisedSizeAsync(ref mdInfo);
//      var serialisedData = await session.MDataInfoActions.SerialiseAsync(mdInfo);
//      mdInfo = await session.MDataInfoActions.DeserialiseAsync(serialisedData);
//
//      keys = await session.MData.ListKeysAsync(ref mdInfo);
//      Assert.AreEqual(1, keys.Count);
//    }
//
//    [Test]
//    public async Task RandomPublicMutableDataInsertAction() {
//      var session = Utils.RandomSession();
//      const ulong tagType = 15010;
//      var actKey = Utils.GetRandomString(10);
//      var actValue = Utils.GetRandomString(10);
//      var mdInfo = await session.MDataInfoActions.RandomPublicAsync(tagType);
//      var mDataPermissionSet = new PermissionSet {Insert = true, ManagePermissions = true, Read = true, Delete = false, Update = false};
//      using (var permissionsH = await session.MDataPermissions.NewAsync()) {
//        using (var appSignKeyH = await session.Crypto.AppPubSignKeyAsync()) {
//          await session.MDataPermissions.InsertAsync(permissionsH, appSignKeyH, ref mDataPermissionSet);
//          await session.MData.PutAsync(ref mdInfo, permissionsH, NativeHandle.Zero);
//        }
//      }
//
//      using (var entryActionsH = await session.MDataEntryActions.NewAsync()) {
//        var key = Encoding.ASCII.GetBytes(actKey).ToList();
//        var value = Encoding.ASCII.GetBytes(actValue).ToList();
//        await session.MDataEntryActions.InsertAsync(entryActionsH, key, value);
//        await session.MData.MutateEntriesAsync(ref mdInfo, entryActionsH);
//      }
//
//      var keys = await session.MData.ListKeysAsync(ref mdInfo);
//      Assert.AreEqual(1, keys.Count);
//
//      foreach (var key in keys) {
//        var (value, _) = await session.MData.GetValueAsync(mdInfo, key.Val.ToList());
//        Assert.AreEqual(actKey, Encoding.ASCII.GetString(key.Val.ToArray()));
//        Assert.AreEqual(actValue, Encoding.ASCII.GetString(value.ToArray()));
//      }
//
//      await session.MData.SerialisedSizeAsync(ref mdInfo);
//      var serialisedData = await session.MDataInfoActions.SerialiseAsync(mdInfo);
//      mdInfo = await session.MDataInfoActions.DeserialiseAsync(serialisedData);
//
//      keys = await session.MData.ListKeysAsync(ref mdInfo);
//      Assert.AreEqual(1, keys.Count);
//    }
//  }
//}
