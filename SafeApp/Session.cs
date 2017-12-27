using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp {
  [PublicAPI]
  public static class Session {
    public static event EventHandler Disconnected;
    private static IntPtr _appPtr;
    private static volatile bool _isDisconnected;
    private static readonly IAppBindings AppBindings = AppResolver.Current;

    public static bool IsDisconnected { get => _isDisconnected; private set => _isDisconnected = value; }

    public static IntPtr AppPtr {
      set {
        if (_appPtr == value) {
          return;
        }

        if (_appPtr != IntPtr.Zero) {
          AppBindings.AppFree(_appPtr);
        }

        _appPtr = value;
      }
      get {
        if (_appPtr == IntPtr.Zero) {
          throw new ArgumentNullException(nameof(AppPtr));
        }
        return _appPtr;
      }
    }

    static Session() {
      AppPtr = IntPtr.Zero;
    }

    public static Task AppRegisteredAsync(string appId, AuthGranted authGranted) {
      return Task.Run(
        () => {
          var tcs = new TaskCompletionSource<object>();

          Action<FfiResult, IntPtr> acctCreatedCb = (result, ptr) => {
            if (result.ErrorCode != 0) {
              tcs.SetException(result.ToException());
              return;
            }

            AppPtr = ptr;
            IsDisconnected = false;
            tcs.SetResult(null);
          };

          AppBindings.AppRegistered(appId, ref authGranted, OnNetworkObserverCb, acctCreatedCb);
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

    public static void FreeApp() {
      IsDisconnected = false;
      AppPtr = IntPtr.Zero;
    }

    public static async Task InitLoggingAsync(string configFilesPath) {
      await AppBindings.AppSetAdditionalSearchPathAsync(configFilesPath);
      await AppBindings.AppInitLoggingAsync(null);
    }

    private static void OnDisconnected(EventArgs e) {
      Disconnected?.Invoke(null, e);
    }

    /// <summary>
    ///   Network State Callback
    /// </summary>
    private static void OnNetworkObserverCb() {
      IsDisconnected = true;
      OnDisconnected(EventArgs.Empty);
    }
  }
}
