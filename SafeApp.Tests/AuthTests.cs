using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
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

      authReq.Containers =
        new List<ContainerPermissions> {new ContainerPermissions {ContName = "someConatiner", Access = new PermissionSet()}};

      Assert.CatchAsync(async () => await Utils.CreateTestApp(authReq));
      // TODO - Fix Should throw an error
      //      authReq.App = new AppExchangeInfo { Id = "", Name = "", Scope = "", Vendor = "" };
      //      Assert.CatchAsync(async () => { await Utils.CreateTestApp(authReq); });
      // TODO - Fix should throw an error. It is crashing with unhandled exception
      //      authReq.App = new AppExchangeInfo();
      //      Assert.CatchAsync(async () => { await Utils.CreateTestApp(authReq); });
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
      var responseMsg = await Utils.AuthContainerRequest(locator, secret, msg);
      var ipcMsg = await Session.DecodeIpcMessageAsync(responseMsg);
      Assert.AreEqual(typeof(ContainersIpcMsg), ipcMsg.GetType());
      var containerResponse = ipcMsg as ContainersIpcMsg;
      Assert.AreEqual(reqId, containerResponse?.ReqId);
      await session.AccessContainer.RefreshAccessInfoAsync();
      var mDataInfo = await session.AccessContainer.GetMDataInfoAsync("_public");
      Assert.AreEqual(15000, mDataInfo.TypeTag);
      session.Dispose();
    }

    [Test]
    public async Task UnregisteredRequestTest() {
      var uniqueId = Utils.GetRandomString(10);
      var (reqId, req) = await Session.EncodeUnregisteredRequestAsync(uniqueId);
      var response = await Utils.AuthUnregisteredRequest(req);
      var ipcMsg = await Session.DecodeIpcMessageAsync(response);
      Assert.AreEqual(typeof(UnregisteredIpcMsg), ipcMsg.GetType());
      var unregisteredClientResponse = ipcMsg as UnregisteredIpcMsg;
      Assert.NotNull(unregisteredClientResponse);
      Assert.AreEqual(reqId, unregisteredClientResponse.ReqId);
      var session = await Session.AppUnregisteredAsync(unregisteredClientResponse.SerialisedCfg);
      session.Dispose();
    }
  }
}
