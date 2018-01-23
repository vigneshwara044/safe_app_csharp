using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.Utilities;

namespace SafeApp.Tests {
  [TestFixture]
  internal class NFS {
    [Test]
    public async Task CrudUsingNfsApi() {
      var session = await Utils.CreateTestApp();
      var mDataInfo = await session.MDataInfoActions.RandomPrivateAsync(18000);
      using (var signPubKey = await session.Crypto.AppPubSignKeyAsync())
      using (var entryhandle = await session.MDataEntries.NewAsync())
      using (var permissionHandle = await session.MDataPermissions.NewAsync()) {
        var permissions = new PermissionSet {Read = true, ManagePermissions = true, Insert = true};
        await session.MDataEntries.InsertAsync(entryhandle, "index.html".ToUtfBytes(), "<html><body>Hello</body></html>".ToUtfBytes());
        await session.MDataPermissions.InsertAsync(permissionHandle, signPubKey, ref permissions);
        await session.MData.PutAsync(ref mDataInfo, permissionHandle, NativeHandle.Zero);
      }

      var fileHandle = await session.NFS.FileOpenAsync(mDataInfo, new File(), Misc.NFS.OpenMode.Overwrite);
      var fileContent = Utils.GetRandomString(20).ToUtfBytes();
      await session.NFS.FileWriteAsync(fileHandle, fileContent);
      var createdFile = await session.NFS.FileCloseAsync(fileHandle);
      var fileName = Utils.GetRandomString(10);
      await session.NFS.DirInsertFileAsync(mDataInfo, fileName, createdFile);
      var (file, version) = await session.NFS.DirFetchFileAsync(mDataInfo, fileName);
      fileHandle = await session.NFS.FileOpenAsync(mDataInfo, file, Misc.NFS.OpenMode.Read);
      var fetchedContent = await session.NFS.FileReadAsync(fileHandle, 0, 0);
      Assert.AreEqual(fileContent, fetchedContent);
      fileHandle = await session.NFS.FileOpenAsync(mDataInfo, file, Misc.NFS.OpenMode.Append);
      var appendedContent = Utils.GetRandomString(10).ToUtfBytes();
      await session.NFS.FileWriteAsync(fileHandle, appendedContent);
      file = await session.NFS.FileCloseAsync(fileHandle);
      await session.NFS.DirUpdateFileAsync(mDataInfo, fileName, file, version + 1);
      (file, version) = await session.NFS.DirFetchFileAsync(mDataInfo, fileName);
      fileHandle = await session.NFS.FileOpenAsync(mDataInfo, file, Misc.NFS.OpenMode.Read);
      fetchedContent = await session.NFS.FileReadAsync(fileHandle, 0, 0);
      Assert.AreEqual(fileContent.Count, fetchedContent.Count + appendedContent.Count);
      await session.NFS.DirDeleteFileAsync(mDataInfo, fileName, version);
      Assert.CatchAsync(async () => await session.NFS.DirFetchFileAsync(mDataInfo, fileName));
    }
  }
}
