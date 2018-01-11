using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SafeApp.AppBindings;
using SafeApp.MData;
using SafeApp.Misc;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp {
  [PublicAPI]
  public sealed class Session : IDisposable {
    private static GCHandle _disconnectedEventGcHandle;
    private static bool _isDisconnected = true;

    private static Action _networkDisconnectedNotifier = () => {
      _isDisconnected = true;
      OnDisconnectedHandler?.Invoke(null, null);
    };

    private static readonly IAppBindings AppBindings = AppResolver.Current;

    public static EventHandler OnDisconnectedHandler;
    private SafeAppPtr _appPtr;

    public bool IsDisconnected { get; }

    /// <summary>
    ///   AccessConatiner API
    /// </summary>
    public AccessContainer AccessContainer { get; }

    /// <summary>
    ///   Crypto API
    /// </summary>
    public Crypto Crypto { get; }

    /// <summary>
    ///   CipherOpt API
    /// </summary>
    public CipherOpt CipherOpt { get; }

    /// <summary>
    ///   ImmutableData API
    /// </summary>
    public IData.IData IData { get; }

    /// <summary>
    ///   MutableData API
    /// </summary>
    public MData.MData MData { get; }

    /// <summary>
    ///   Mutable Data Entries API
    /// </summary>
    public MDataEntries MDataEntries { get; }

    /// <summary>
    ///   Mutable Data Entry Actions API
    /// </summary>
    public MDataEntryActions MDataEntryActions { get; }

    /// <summary>
    ///   MDataInfo API
    /// </summary>
    public MDataInfoActions MDataInfoActions { get; }

    /// <summary>
    ///   Mutable Data Permissions API
    /// </summary>
    public MDataPermissions MDataPermissions { get; }

    public Session(IntPtr appPtr) {
      _appPtr = new SafeAppPtr(appPtr);
      AccessContainer = new AccessContainer(_appPtr);
      Crypto = new Crypto(_appPtr);
      CipherOpt = new CipherOpt(_appPtr);
      IData = new IData.IData(_appPtr);
      MData = new MData.MData(_appPtr);
      MDataEntries = new MDataEntries(_appPtr);
      MDataEntryActions = new MDataEntryActions(_appPtr);
      MDataInfoActions = new MDataInfoActions(_appPtr);
      MDataPermissions = new MDataPermissions(_appPtr);
      _isDisconnected = false;
    }

    public void Dispose() {
      ReleaseUnmanagedResources();
      GC.SuppressFinalize(this);
    }

    /// <summary>
    ///   Invoked to fetch the app's root container name
    /// </summary>
    /// <param name="appId"></param>
    /// <returns>string</returns>
    public Task<string> AppContainerNameAsync(string appId) {
      return AppBindings.AppContainerNameAsync(appId);
    }

    public static Task<Session> AppRegisteredAsync(string appId, AuthGranted authGranted, EventHandler disconnectedEventHandler) {
      return Task.Run(
        () => {
          var tcs = new TaskCompletionSource<Session>();
          Action<FfiResult, IntPtr, GCHandle> acctCreatedCb = (result, ptr, disconnectedEventGcHandle) => {
            if (result.ErrorCode != 0) {
              disconnectedEventGcHandle.Free();
              tcs.SetException(result.ToException());
              return;
            }

            _disconnectedEventGcHandle = disconnectedEventGcHandle;
            var session = new Session(ptr);
            tcs.SetResult(session);
          };

          AppBindings.AppRegistered(appId, ref authGranted, _networkDisconnectedNotifier, acctCreatedCb);
          return tcs.Task;
        });
    }

    public static Task<Session> AppUnregisteredAsync(List<byte> bootstrapConfig) {
      return Task.Run(
        () => {
          var tcs = new TaskCompletionSource<Session>();
          Action<FfiResult, IntPtr, GCHandle> acctCreatedCb = (result, ptr, disconnectedEventGcHandle) => {
            if (result.ErrorCode != 0) {
              disconnectedEventGcHandle.Free();
              tcs.SetException(result.ToException());
              return;
            }

            _disconnectedEventGcHandle = disconnectedEventGcHandle;
            var session = new Session(ptr);
            tcs.SetResult(session);
          };

          AppBindings.AppUnregistered(bootstrapConfig, _networkDisconnectedNotifier, acctCreatedCb);
          return tcs.Task;
        });
    }

    public static Task<IpcMsg> DecodeIpcMessageAsync(string encodedReq) {
      return AppBindings.DecodeIpcMsgAsync(encodedReq);
    }

    /// <summary>
    ///   Encodes an authorisation request to send to Authenticator
    /// </summary>
    /// <param name="authReq"></param>
    /// <returns>RequestId, Encoded auth request</returns>
    public static Task<(uint, string)> EncodeAuthReqAsync(AuthReq authReq) {
      return AppBindings.EncodeAuthReqAsync(ref authReq);
    }

    public static Task<(uint, string)> EncodeContainerRequestAsync(ContainersReq containersReq) {
      return AppBindings.EncodeContainersReqAsync(ref containersReq);
    }

    public static Task<(uint, string)> EncodeShareMDataRequestAsync(ShareMDataReq shareMDataReq) {
      return AppBindings.EncodeShareMDataReqAsync(ref shareMDataReq);
    }

    public static Task<(uint, string)> EncodeUnregisteredRequestAsync(string reqId) {
      return AppBindings.EncodeUnregisteredReqAsync(Encoding.UTF8.GetBytes(reqId).ToList());
    }

    ~Session() {
      ReleaseUnmanagedResources();
    }

    /// <summary>
    ///   Returns the AccountInfo of the current Session
    /// </summary>
    /// <returns>AccountInfo</returns>
    public Task<AccountInfo> GetAccountInfoAsyc() {
      return AppBindings.AppAccountInfoAsync(_appPtr);
    }

    public static Task<string> GetExeFileStemAsync() {
      return AppBindings.AppExeFileStemAsync();
    }

    public static async Task InitLoggingAsync(string configFilesPath) {
      await AppBindings.AppSetAdditionalSearchPathAsync(configFilesPath);
      await AppBindings.AppInitLoggingAsync(null);
    }

    /// <summary>
    ///   Invoked after Disconnect callback is fired to reconnect the session with the network
    /// </summary>
    public Task ReconnectAsync() {
      return Task.Run(
        async () => {
          await AppBindings.AppReconnectAsync(_appPtr);
          _isDisconnected = false;
        });
    }

    private void ReleaseUnmanagedResources() {
      AppBindings.AppFree(_appPtr);
      _appPtr.Value = IntPtr.Zero;
//      _disconnectedEventGcHandle.Free();
    }

    /// <summary>
    ///   Resets the Object Cache for the session
    /// </summary>
    /// <returns>Void</returns>
    public Task ResetObjectCacheAsync() {
      return AppBindings.AppResetObjectCacheAsync(_appPtr);
    }

    public static Task SetAdditionalSearchPathAsync(string path) {
      return AppBindings.AppSetAdditionalSearchPathAsync(path);
    }

    public static Task SetLogOutputPathAsync(string outputFileName) {
      return AppBindings.AppOutputLogPathAsync(outputFileName);
    }
  }
}
