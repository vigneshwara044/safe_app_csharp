using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.MockAuthBindings;
using SafeApp.Utilities;

namespace SafeApp.Tests {
  [TestFixture]
  internal class AuthTest {
    [Test]
    public async Task ConnectAsRegisteredAppTest() {
      var authReq = new AuthReq {
        App = new AppExchangeInfo {Id = "net.maidsafe.test", Name = "TestApp", Scope = null, Vendor = "MaidSafe.net Ltd."},
        AppContainer = true,
        Containers = new List<ContainerPermissions>()
      };

      using (var session = await Utils.CreateTestApp(authReq)) {
        using (await session.Crypto.AppPubSignKeyAsync()) { }
      }

      authReq.AppContainer = false;
      using (var session = await Utils.CreateTestApp(authReq)) {
        using (await session.Crypto.AppPubSignKeyAsync()) { }
      }

      authReq.Containers = null;
      using (var session = await Utils.CreateTestApp(authReq)) {
        using (await session.Crypto.AppPubSignKeyAsync()) { }
      }

      authReq.Containers = new List<ContainerPermissions> {
        new ContainerPermissions {
          ContName = "_public",
          Access = new PermissionSet {Read = true, Insert = true, Delete = false, ManagePermissions = false, Update = false}
        },
        new ContainerPermissions {
          ContName = "_videos",
          Access = new PermissionSet {Read = true, Insert = true, Delete = false, ManagePermissions = false, Update = false}
        },
        new ContainerPermissions {
          ContName = "_publicNames",
          Access = new PermissionSet {Read = true, Insert = true, Delete = false, ManagePermissions = false, Update = false}
        },
        new ContainerPermissions {
          ContName = "_documents",
          Access = new PermissionSet {Read = true, Insert = true, Delete = false, ManagePermissions = false, Update = false}
        },
        new ContainerPermissions {
          ContName = "_music",
          Access = new PermissionSet {Read = true, Insert = true, Delete = false, ManagePermissions = false, Update = false}
        }
        // TODO - Enable once fixed in authenticator
//        new ContainerPermissions()
//        {
//          ContName = "_pictures",
//          Access = new PermissionSet() { Read = true, Insert = true, Delete = false, ManagePermissions = false, Update = false }
//        }
      };

      using (var session = await Utils.CreateTestApp(authReq)) {
        using (await session.Crypto.AppPubSignKeyAsync()) { }
      }

      var authRequest = await Session.EncodeAuthReqAsync(authReq);
      var response = await Utils.AuthenticateAuthRequest(authRequest.Item2, false);
      Assert.CatchAsync(async () => await Session.DecodeIpcMessageAsync(response));

      authReq.Containers =
        new List<ContainerPermissions> {new ContainerPermissions {ContName = "someConatiner", Access = new PermissionSet()}};

      Assert.CatchAsync(async () => await Utils.CreateTestApp(authReq));
      // TODO - Fix Should throw an error
      //      authReq.App = new AppExchangeInfo { Id = "", Name = "", Scope = "", Vendor = "" };
      //      Assert.CatchAsync(async () => { await Utils.CreateTestApp(authReq); });
      // TODO - Fix should throw an error. It is crashing with unhandled exception
//            authReq.App = new AppExchangeInfo();
//            Assert.CatchAsync(async () => { await Utils.CreateTestApp(authReq); });
    }

    [Test]
    public async Task ContainerRequestTest() {
      var authReq = new AuthReq {
        App = new AppExchangeInfo {Id = "net.maidsafe.test", Name = "TestApp", Scope = null, Vendor = "MaidSafe.net Ltd."},
        AppContainer = true,
        Containers = new List<ContainerPermissions>()
      };
      var locator = Utils.GetRandomString(10);
      var secret = Utils.GetRandomString(10);
      var session = await Utils.CreateTestApp(locator, secret, authReq);
      Assert.CatchAsync(async () => await session.AccessContainer.GetMDataInfoAsync("_public"));
      var containerRequest = new ContainersReq {
        App = authReq.App,
        Containers = new List<ContainerPermissions> {
          new ContainerPermissions {ContName = "_public", Access = new PermissionSet {Read = true}}
        }
      };
      var (reqId, msg) = await Session.EncodeContainerRequestAsync(containerRequest);
      var responseMsg = await Utils.AuthenticateContainerRequest(locator, secret, msg, true);
      var ipcMsg = await Session.DecodeIpcMessageAsync(responseMsg);
      Assert.AreEqual(typeof(ContainersIpcMsg), ipcMsg.GetType());
      var containerResponse = ipcMsg as ContainersIpcMsg;
      Assert.AreEqual(reqId, containerResponse?.ReqId);
      await session.AccessContainer.RefreshAccessInfoAsync();
      var mDataInfo = await session.AccessContainer.GetMDataInfoAsync("_public");
      Assert.AreEqual(15000, mDataInfo.TypeTag);
      session.Dispose();
      containerRequest = new ContainersReq
      {
        App = authReq.App,
        Containers = new List<ContainerPermissions> {
          new ContainerPermissions {ContName = "_videos", Access = new PermissionSet {Read = true}}
        }
      };
      (_, msg) = await Session.EncodeContainerRequestAsync(containerRequest);
      responseMsg = await Utils.AuthenticateContainerRequest(locator, secret, msg, false);
      Assert.CatchAsync<IpcMsgException>(async() => await Session.DecodeIpcMessageAsync(responseMsg));
    }

    [Test, Ignore("")]
    public async Task ShareMDataAuthTest() {
      var locator = Utils.GetRandomString(10);
      var secret = Utils.GetRandomString(20);
      var typeTag = 150001;
      var authReq = new AuthReq {
        App = new AppExchangeInfo {Id = "net.maidsafe.test", Name = "TestApp", Scope = null, Vendor = "MaidSafe.net Ltd."},
        AppContainer = true,
        Containers = new List<ContainerPermissions>()
      };
      var session = await Utils.CreateTestApp(locator, secret, authReq);
      var mdInfo = await session.MDataInfoActions.RandomPrivateAsync((ulong)typeTag);
      var actKey = Utils.GetRandomString(10).ToUtfBytes();
      var actValue = Utils.GetRandomString(10).ToUtfBytes();
      using (var userSignKeyHandle = await session.Crypto.AppPubSignKeyAsync())
      using (var permissionsHandle = await session.MDataPermissions.NewAsync()) {
        var permissionSet = new PermissionSet {Read = true, Insert = true, Delete = false, Update = false, ManagePermissions = false};
        await session.MDataPermissions.InsertAsync(permissionsHandle, userSignKeyHandle, ref permissionSet);
        using (var entriesHandle = await session.MDataEntries.NewAsync()) {
          var key = await session.MDataInfoActions.EncryptEntryKeyAsync(mdInfo, actKey);
          var value = await session.MDataInfoActions.EncryptEntryKeyAsync(mdInfo, actValue);
          await session.MDataEntries.InsertAsync(entriesHandle, key, value);
          await session.MData.PutAsync(ref mdInfo, permissionsHandle, entriesHandle);
        }
      }

      session.Dispose();
      authReq.App = new AppExchangeInfo {Id = "net.maidsafe.test.app", Name = "Test App", Scope = null, Vendor = "MaidSafe.net Ltd."};
      var msg = await Session.EncodeAuthReqAsync(authReq);
      var authresponse = await Utils.AuthenticateAuthRequest(locator, secret, msg.Item2, true);
      var authGranted = await Session.DecodeIpcMessageAsync(authresponse) as AuthIpcMsg;
      session = await Session.AppRegisteredAsync(authReq.App.Id, authGranted.AuthGranted);
      Assert.CatchAsync(async () => await session.MData.ListKeysAsync(ref mdInfo));
      // TODO doesn't encode request if only Read is set to true
      var shareMdReq = new ShareMDataReq {
        App = authReq.App,
        MData = new List<ShareMData> {
          new ShareMData {Name = mdInfo.Name, TypeTag = mdInfo.TypeTag, Perms = new PermissionSet {Read = true, Insert = true}}
        }
      };
      var ipcMsg = await Session.EncodeShareMDataRequestAsync(shareMdReq);
      var response = await Utils.AuthenticateShareMDataRequest(locator, secret, ipcMsg.Item2, true);
      var responseMsg = await Session.DecodeIpcMessageAsync(response) as ShareMDataIpcMsg;
      Assert.NotNull(responseMsg);
      Assert.AreEqual(ipcMsg.Item1, responseMsg.ReqId);
      var keys = await session.MData.ListKeysAsync(ref mdInfo);
      Assert.AreEqual(1, keys.Count);
      session.Dispose();
    }

    [Test, Ignore("")]
    public async Task UnregisteredRequestTest() {
      var someRandomSession = await Utils.CreateTestApp();
      var publicMDataInfo = await Utils.PreparePublicDirectory(someRandomSession);
      someRandomSession.Dispose();
      var uniqueId = Utils.GetRandomString(10);
      var (reqId, req) = await Session.EncodeUnregisteredRequestAsync(uniqueId);
      var response = await Utils.AuthenticateUnregisteredRequest(req);
      var ipcMsg = await Session.DecodeIpcMessageAsync(response);
      Assert.AreEqual(typeof(UnregisteredIpcMsg), ipcMsg.GetType());
      var unregisteredClientResponse = ipcMsg as UnregisteredIpcMsg;
      Assert.NotNull(unregisteredClientResponse);
      Assert.AreEqual(reqId, unregisteredClientResponse.ReqId);
      var session = await Session.AppUnregisteredAsync(unregisteredClientResponse.SerialisedCfg);
      var mdInfo = new MDataInfo {
        Name = publicMDataInfo.Name,
        TypeTag = publicMDataInfo.TypeTag
      };
      await session.MData.GetValueAsync(mdInfo, "index.html".ToUtfBytes());
      session.Dispose();
    }
  }
}
