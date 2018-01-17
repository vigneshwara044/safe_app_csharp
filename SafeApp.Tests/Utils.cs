using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.MockAuthBindings;
using SafeApp.Utilities;

namespace SafeApp.Tests {
  internal static class Utils {
    private static readonly Random Random = new Random();

    public static Task<Session> CreateTestApp(AuthReq authReq) {
      var locator = GetRandomString(10);
      var secret = GetRandomString(10);
      return CreateTestApp(locator, secret, authReq);
    }

    public static async Task<Session> CreateTestApp(string locator, string secret, AuthReq authReq) {
      var authenticator = await Authenticator.CreateAccountAsync(locator, secret, "");
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

    public static async Task<string> AuthContainerRequest(string locator, string secret, string ipcMsg) {
      var authenticator = await Authenticator.LoginAsync(locator, secret);
      var ipcReq = await authenticator.DecodeIpcMessageAsync(ipcMsg);
      Assert.AreEqual(typeof(ContainersIpcReq), ipcReq.GetType());
      var response = await authenticator.EncodeContainersRespAsync(ipcReq as ContainersIpcReq, true);
      authenticator.Dispose();
      return response;
    }

    public static async Task<string> AuthUnregisteredRequest(string ipcMsg)
    {
      var authenticator = await Authenticator.CreateAccountAsync(Utils.GetRandomString(5), Utils.GetRandomString(5), Utils.GetRandomString(10));
      var ipcReq = await authenticator.DecodeIpcMessageAsync(ipcMsg);
      Assert.AreEqual(typeof(UnregisteredIpcReq), ipcReq.GetType());
      var response = await authenticator.EncodeUnregisteredRespAsync(((UnregisteredIpcReq)ipcReq).ReqId, true);
      authenticator.Dispose();
      return response;
    }

    public static string GetRandomString(int length) {
      const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
      return new string(Enumerable.Repeat(chars, length).Select(s => s[Random.Next(s.Length)]).ToArray());
    }
  }
}
