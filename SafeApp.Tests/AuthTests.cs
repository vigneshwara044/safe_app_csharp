using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.Utilities;

namespace SafeApp.Tests {
  [TestFixture]
  internal class AuthTest {
    private const string AuthUri =
      "safe-bmv0lm1hawrzywzllmv4yw1wbgvzlm1hawx0dxrvcmlhba:AQAAAPN-m2AAAAAAAAAAACAAAAAAAAAAMxWYyOUEdIA2VNGJTmAjKFvg9yCrceomf_dZC3AdguAgAAAAAAAAALXtHOU3NtIxjIfIX4hk-pTxOeDxMiQnrXxBOqF8sp70IAAAAAAAAACjjbSMVLfM6fKxHaUwOE9ToK0mKMCLNjpstT3TqZP9KkAAAAAAAAAAbAl79zGmd3Dq5vTSVusCT8JKtr_1yYu320p24viIDG2jjbSMVLfM6fKxHaUwOE9ToK0mKMCLNjpstT3TqZP9KiAAAAAAAAAA661BaoE9k13thtGd8MVVR9gLutkSM_NUq67dnmuGBEMgAAAAAAAAAFBM4tDw2fjP-O3PryNeEf2tu56-CA1LdTidE2GIMHKYAAAAAAAAAAAAAAAAAAAAAKtRs_fd7jF0mYyq8bNyDN86wCCiowPQRQi7Db3tfRpRmDoAAAAAAAAYAAAAAAAAACcJjCl2_amfKTMP53W_gMNUxq_YCzVH0wIAAAAAAAAAJwAAAAAAAABhcHBzL25ldC5tYWlkc2FmZS5leGFtcGxlcy5tYWlsdHV0b3JpYWxPDqh1LvjMfruqH92sGoOBa9pKbeoyCyQm8HElHtfbR5g6AAAAAAAAASAAAAAAAAAA_ZaHRjRZ9gfbqCd_ZKjNUYewymCf5sNybZKM5-cT0qgYAAAAAAAAAAn89-_PuZAMk57uoVXS1YNbHR6o3uv-RAAFAAAAAAAAAAAAAAABAAAAAgAAAAMAAAAEAAAADAAAAAAAAABfcHVibGljTmFtZXMjGtQ2VkRH4VMMsrpUChdxTK6U41KgA-FmGue5Z2-dMZg6AAAAAAAAASAAAAAAAAAAFuKBLte-nkyuuXV6ovGbsfaZRBr_OFf2CRLNZ2dyRrcYAAAAAAAAAGnXgCQRN9rBO-Eh8HNZ-SLeokyCPkBMegACAAAAAAAAAAAAAAABAAAA";

    private const string UnregisteredAuthUri =
      "safe-bmv0lm1hawrzywzllmv4yw1wbgvzlm1hawx0dxrvcmlhba:AQAAANdBP4gCAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";

    private const string ContainerResponseUri = "safe-bmv0lm1hawrzywzllmv4yw1wbgvzlm1hawx0dxrvcmlhba:AQAAANZtpGIBAAAAAAAAAA";

//    private const string ShareMDataResponseUri = "safe-bmv0lm1hawrzywzllmv4yw1wbgvzlndlymhvc3rpbmdtyw5hz2vy:AQAAAKY8y-wAAAAAAAAAACAAAAAAAAAAA29bsGLigcpbfsj8A_P9zMQsHM2dYVC11aE2juQJYZwgAAAAAAAAAOPTk8r3ThVoccb1ZU15U-rz2JPoAltta1PedqiHe4S2IAAAAAAAAACso4SQFLF6Xirfi7boTHlotq8sfq1uCY3Wld4E8kWTMkAAAAAAAAAAkr308V8c7tHVpt5t65gaZRAovEdE4cibhK90DoNlgOGso4SQFLF6Xirfi7boTHlotq8sfq1uCY3Wld4E8kWTMiAAAAAAAAAARK483_DDk3kOCtEwtJ1Cloo4eQ3CT2sB5issPvQPDzMgAAAAAAAAAMh20SEvg5xcgQx_ggGvx7dv3AqYMVXAQ6OcElSH45hOAAAAAAAAAAAAAAAAAAAAAFNdSUU4ZlhwB9iCNiv_XfeeVR-q2uIn9OAvkV-o_9hWmDoAAAAAAAAYAAAAAAAAAJGRW8SSrLGexAiA6ETQkJpOhV3TLq_UYwIAAAAAAAAADAAAAAAAAABfcHVibGljTmFtZXNe_yEfW_EmRDrQorUu0zfxFyqQhbgNK874tdvOotyxL5g6AAAAAAAAASAAAAAAAAAAJrC0-PrKT3YR6AVuOENalm6JlhGarqATQZxULxvsVgYYAAAAAAAAAC8-2s-W-7f2D4y78bQuDLYr60lZnVyFlAAEAAAAAAAAAAAAAAABAAAAAgAAAAMAAAAHAAAAAAAAAF9wdWJsaWNU6gkC4SN-XmdPG8xGQNGvA6ALK7yFB2X586lwcVDKqJg6AAAAAAAAAAAEAAAAAAAAAAAAAAABAAAAAgAAAAMAAAA";

    private AppExchangeInfo GetExchangeInfo() {
      return new AppExchangeInfo {Id = "net.maidsafe.example", Name = "Test App", Vendor = "Maidsafe Ltd."};
    }

    [Test]
    public async Task ConnectAsRegisteredApp() {
      var ipcMsg = await Session.DecodeIpcMessageAsync(AuthUri.Split(':')[1]);
      Assert.AreEqual(ipcMsg.GetType(), typeof(AuthIpcMsg));
      var result = (AuthIpcMsg)ipcMsg;
      var session = await Session.AppRegisteredAsync("bmv0lm1hawrzywzllmv4yw1wbgvzlm1hawx0dxrvcmlhba", result.AuthGranted);
      Assert.AreEqual(false, session.IsDisconnected);
      session.Dispose();
    }

    [Test]
    public async Task ConnectAsUnregisteredApp() {
      var ipcMsg = await Session.DecodeIpcMessageAsync(UnregisteredAuthUri.Split(':')[1]);
      Assert.AreEqual(ipcMsg.GetType(), typeof(UnregisteredIpcMsg));
      var result = (UnregisteredIpcMsg)ipcMsg;
      var session = await Session.AppUnregisteredAsync(result.SerialisedCfg);
      Assert.AreEqual(false, session.IsDisconnected);
      session.Dispose();
    }

    [Test]
    public async Task DecodeContainerResponse() {
      var ipcMsg = await Session.DecodeIpcMessageAsync(ContainerResponseUri.Split(':')[1]);
      Assert.AreEqual(ipcMsg.GetType(), typeof(ContainersIpcMsg));
    }

//    [Test]
//    public async Task DecodeShareMDataResponse()
//    {
//      var ipcMsg = await Session.DecodeIpcMessageAsync(ShareMDataResponseUri.Split(':')[1]);
//      Assert.AreEqual(ipcMsg.GetType(), typeof(ShareMDataIpcMsg));
//    }

    [Test]
    public async Task EncodeAuthRequest() {
      var req = new AuthReq {AppContainer = true, App = GetExchangeInfo(), Containers = new List<ContainerPermissions>()};
      var encodeResponse = await Session.EncodeAuthReqAsync(req);
      Assert.AreEqual(true, encodeResponse.Item1 > 0);
      Assert.IsNotNull(encodeResponse.Item2);
      // AuthRequest with AppContainer = false
      req.AppContainer = false;
      encodeResponse = await Session.EncodeAuthReqAsync(req);
      Assert.AreEqual(true, encodeResponse.Item1 > 0);
      Assert.IsNotNull(encodeResponse.Item2);
      // AuthRequest with Containers is null
      req.Containers = null;
      encodeResponse = await Session.EncodeAuthReqAsync(req);
      Assert.AreEqual(true, encodeResponse.Item1 > 0);
      Assert.IsNotNull(encodeResponse.Item2);
      // AuthRequest with one Container
      var containerPermissions = new List<ContainerPermissions> {
        new ContainerPermissions {
          ContName = "_public",
          Access = new PermissionSet {Read = true, Insert = true, Update = true, Delete = true, ManagePermissions = true}
        }
      };
      req.Containers = containerPermissions;
      encodeResponse = await Session.EncodeAuthReqAsync(req);
      Assert.AreEqual(true, encodeResponse.Item1 > 0);
      Assert.IsNotNull(encodeResponse.Item2);
      // AuthRequest with more than one Container
      containerPermissions.Add(
        new ContainerPermissions {
          ContName = "_pictures",
          Access = new PermissionSet {Read = true, Insert = true, Update = true, Delete = true, ManagePermissions = true}
        });
      encodeResponse = await Session.EncodeAuthReqAsync(req);
      Assert.AreEqual(true, encodeResponse.Item1 > 0);
      Assert.IsNotNull(encodeResponse.Item2);
    }
  }
}
