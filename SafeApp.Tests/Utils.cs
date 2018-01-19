using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.MockAuthBindings;
using SafeApp.Utilities;

namespace SafeApp.Tests {
  internal static class Utils {
    private static readonly Random Random = new Random();

    private static async Task<string> AuthenticateAuthRequest(Authenticator authenticator, string ipcMsg, bool allow) {
      var ipcReq = await authenticator.DecodeIpcMessageAsync(ipcMsg);
      Assert.AreEqual(typeof(AuthIpcReq), ipcReq.GetType());
      var response = await authenticator.EncodeAuthRespAsync(ipcReq as AuthIpcReq, allow);
      authenticator.Dispose();
      return response;
    }

    public static async Task<string> AuthenticateAuthRequest(string ipcMsg, bool allow) {
      var authenticator = await Authenticator.CreateAccountAsync(GetRandomString(10), GetRandomString(10), GetRandomString(5));
      return await AuthenticateAuthRequest(authenticator, ipcMsg, allow);
    }

    public static async Task<string> AuthenticateAuthRequest(string locator, string secret, string ipcMsg, bool allow) {
      var authenticator = await Authenticator.LoginAsync(locator, secret);
      return await AuthenticateAuthRequest(authenticator, ipcMsg, allow);
    }

    public static async Task<string> AuthenticateContainerRequest(string locator, string secret, string ipcMsg, bool allow) {
      using (var authenticator = await Authenticator.LoginAsync(locator, secret)) {
        var ipcReq = await authenticator.DecodeIpcMessageAsync(ipcMsg);
        Assert.AreEqual(typeof(ContainersIpcReq), ipcReq.GetType());
        var response = await authenticator.EncodeContainersRespAsync(ipcReq as ContainersIpcReq, allow);
        return response;
      }
    }

    public static async Task<string> AuthenticateShareMDataRequest(string locator, string secret, string ipcMsg, bool allow) {
      var authenticator = await Authenticator.LoginAsync(locator, secret);
      var ipcReq = await authenticator.DecodeIpcMessageAsync(ipcMsg);
      Assert.AreEqual(typeof(ShareMDataIpcReq), ipcReq.GetType());
      var response = await authenticator.EncodeShareMdataRespAsync(ipcReq as ShareMDataIpcReq, allow);
      authenticator.Dispose();
      return response;
    }

    public static async Task<string> AuthenticateUnregisteredRequest(string ipcMsg) {
      var authenticator = await Authenticator.CreateAccountAsync(GetRandomString(5), GetRandomString(5), "");
      var ipcReq = await authenticator.DecodeIpcMessageAsync(ipcMsg);
      Assert.AreEqual(typeof(UnregisteredIpcReq), ipcReq.GetType());
      var response = await authenticator.EncodeUnregisteredRespAsync(((UnregisteredIpcReq)ipcReq).ReqId, true);
      authenticator.Dispose();
      return response;
    }

    public static Task<Session> CreateTestApp() {
      var locator = GetRandomString(10);
      var secret = GetRandomString(10);
      var authReq = new AuthReq {
        App = new AppExchangeInfo {Id = GetRandomString(10), Name = GetRandomString(5), Scope = null, Vendor = GetRandomString(5)},
        AppContainer = true,
        Containers = new List<ContainerPermissions>()
      };
      return CreateTestApp(locator, secret, authReq);
    }

    public static Task<Session> CreateTestApp(AuthReq authReq) {
      var locator = GetRandomString(10);
      var secret = GetRandomString(10);
      return CreateTestApp(locator, secret, authReq);
    }

    public static async Task<Session> CreateTestApp(string locator, string secret, AuthReq authReq) {
      var authenticator = await Authenticator.CreateAccountAsync(locator, secret, Utils.GetRandomString(5));
      var (_, reqMsg) = await Session.EncodeAuthReqAsync(authReq);
      var ipcReq = await authenticator.DecodeIpcMessageAsync(reqMsg);
      Assert.AreEqual(typeof(AuthIpcReq), ipcReq.GetType());
      var authIpcReq = ipcReq as AuthIpcReq;
      var resMsg = await authenticator.EncodeAuthRespAsync(authIpcReq, true);
      var ipcResponse = await Session.DecodeIpcMessageAsync(resMsg);
      Assert.AreEqual(typeof(AuthIpcMsg), ipcResponse.GetType());
      var authResponse = ipcResponse as AuthIpcMsg;
      Assert.NotNull(authResponse);
      authenticator.Dispose();
      return await Session.AppRegisteredAsync(authReq.App.Id, authResponse.AuthGranted);
    }

    public static string GetRandomString(int length) {
      const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
      return new string(Enumerable.Repeat(chars, length).Select(s => s[Random.Next(s.Length)]).ToArray());
    }

    public static async Task<MDataInfo> PreparePublicDirectory(Session session) {
      var mDataInfo = await session.MDataInfoActions.RandomPublicAsync(16000);
      using (var signPubKey = await session.Crypto.AppPubSignKeyAsync())
      using (var entryhandle = await session.MDataEntries.NewAsync())
      using (var permissionHandle = await session.MDataPermissions.NewAsync()) {
        var metadata = new MetadataResponse {
          Name = "Random Pubic Container",
          Description = "Public container for web files",
          TypeTag = mDataInfo.TypeTag,
          XorName = mDataInfo.Name
        };
        var encMetaData = await session.MData.EncodeMetadata(ref metadata);
        var permissions = new PermissionSet {Read = true, ManagePermissions = true, Insert = true};
        await session.MDataEntries.InsertAsync(entryhandle, AppConstants.MDataMetaDataKey.ToUtfBytes(), encMetaData);
        await session.MDataEntries.InsertAsync(entryhandle, "index.html".ToUtfBytes(), "<html><body>Hello</body></html>".ToUtfBytes());
        await session.MDataPermissions.InsertAsync(permissionHandle, signPubKey, ref permissions);
        await session.MData.PutAsync(ref mDataInfo, permissionHandle, entryhandle);
      }

      return mDataInfo;
    }
  }
}
