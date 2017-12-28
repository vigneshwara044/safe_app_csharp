using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SafeApp.AppBindings;
using SafeApp.Misc;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp {
  [PublicAPI]
  public class Session {
    private Action _disconnected;
    private IntPtr _appPtr;
    private readonly IAppBindings _appBindings = AppResolver.Current;

    private AccessContainer _accessContainer;
    private Crypto _crypto;
    private CipherOpt _cipherOpt;
    private IData.IData _iData;
    private MData.MData _mData;
    private MData.MDataEntries _mDataEntries;
    private MData.MDataEntryActions _mDataEntryActions;
    private MData.MDataInfoActions _mDataInfoActions;
    private MData.MDataPermissions _mDataPermissions;

    protected Session(IntPtr appPtr, Action disconnectedAction)
    {
      _appPtr = appPtr;
      _disconnected = disconnectedAction;
    }

    public AccessContainer AccessContainer {
      get {
        if (_accessContainer != null) {
          return _accessContainer;
        }
        _accessContainer = new AccessContainer(_appPtr);
        return _accessContainer;
      }
    }

    public Crypto Crypto
    {
      get
      {
        if (_crypto != null)
        {
          return _crypto;
        }
        _crypto = new Crypto(_appPtr);
        return _crypto;
      }
    }

    public CipherOpt CipherOpt
    {
      get
      {
        if (_cipherOpt != null)
        {
          return _cipherOpt;
        }
        _cipherOpt = new CipherOpt(_appPtr);
        return _cipherOpt;
      }
    }

    public IData.IData IData
    {
      get
      {
        if (_iData != null)
        {
          return _iData;
        }
        _iData = new IData.IData(_appPtr);
        return _iData;
      }
    }

    public MData.MData MData
    {
      get
      {
        if (_mData != null)
        {
          return _mData;
        }
        _mData = new MData.MData(_appPtr);
        return _mData;
      }
    }

    public MData.MDataEntries MDataEntries
    {
      get
      {
        if (_mDataEntries != null)
        {
          return _mDataEntries;
        }
        _mDataEntries = new MData.MDataEntries(_appPtr);
        return _mDataEntries;
      }
    }

    public MData.MDataEntryActions MDataEntryActions
    {
      get
      {
        if (_mDataEntryActions != null)
        {
          return _mDataEntryActions;
        }
        _mDataEntryActions = new MData.MDataEntryActions(_appPtr);
        return _mDataEntryActions;
      }
    }

    public MData.MDataInfoActions MDataInfoActions
    {
      get
      {
        if (_mDataInfoActions != null)
        {
          return _mDataInfoActions;
        }
        _mDataInfoActions = new MData.MDataInfoActions(_appPtr);
        return _mDataInfoActions;
      }
    }

    public MData.MDataPermissions MDataPermissions
    {
      get
      {
        if (_mDataPermissions != null)
        {
          return _mDataPermissions;
        }
        _mDataPermissions = new MData.MDataPermissions(_appPtr);
        return _mDataPermissions;
      }
    }

    public Task ResetObjectCacheAsync() {
      return _appBindings.AppResetObjectCacheAsync(_appPtr);
    }

    public Task<string> AppContainerNameAsync(string appId) {
      return _appBindings.AppContainerNameAsync(appId);
    }

    public Task<AccountInfo> GetAccountInfoAsyc() {
      return _appBindings.AppAccountInfoAsync(_appPtr);
    }

    public Task ReconnectAsync() {
      return _appBindings.AppReconnectAsync(_appPtr);
    }

    public static Task<string> GetExeFileStemAsync() {
      return AppResolver.Current.AppExeFileStemAsync();
    }

    public static Task SetAdditionalSearchPathAsync(string path)
    {
      return AppResolver.Current.AppSetAdditionalSearchPathAsync(path);
    }

    public static Task SetLogOutputPathAsync(string outputFileName) {
      return AppResolver.Current.AppOutputLogPathAsync(outputFileName);
    }

    public static Task<Session> AppRegisteredAsync(string appId, AuthGranted authGranted, EventHandler disconnectedEventHandler) {
      return Task.Run(
        () => {
          var tcs = new TaskCompletionSource<Session>();
          Action networkDisconnectedNotifier = () => {
            disconnectedEventHandler.Invoke(null, EventArgs.Empty);
          };

          Action<FfiResult, IntPtr> acctCreatedCb = (result, ptr) => {
            if (result.ErrorCode != 0) {
              tcs.SetException(result.ToException());
              return;
            }

            var session = new Session(ptr, networkDisconnectedNotifier);
            tcs.SetResult(session);
          };

          AppResolver.Current.AppRegistered(appId, ref authGranted, networkDisconnectedNotifier, acctCreatedCb);
          return tcs.Task;
        });
    }

    public static Task<Session> AppUnregisteredAsync(List<byte> bootstrapConfig, EventHandler disconnectedEventHandler)
    {
      return Task.Run(
        () => {
          var tcs = new TaskCompletionSource<Session>();
          Action networkDisconnectedNotifier = () => {
            disconnectedEventHandler.Invoke(null, EventArgs.Empty);
          };

          Action<FfiResult, IntPtr> acctCreatedCb = (result, ptr) => {
            if (result.ErrorCode != 0)
            {
              tcs.SetException(result.ToException());
              return;
            }

            var session = new Session(ptr, networkDisconnectedNotifier);
            tcs.SetResult(session);
          };

          AppResolver.Current.AppUnregistered(bootstrapConfig.ToArray(), networkDisconnectedNotifier, acctCreatedCb);
          return tcs.Task;
        });
    }

    public static Task<IpcMsg> DecodeIpcMessageAsync(string encodedReq) {
      return AppResolver.Current.DecodeIpcMsgAsync(encodedReq);
    }

    /// <summary>
    ///   Encodes an authorisation request to send to Authenticator
    /// </summary>
    /// <param name="authReq"></param>
    /// <returns>RequestId, Encoded auth request</returns>
    public static Task<(uint, string)> EncodeAuthReqAsync(AuthReq authReq) {
      return AppResolver.Current.EncodeAuthReqAsync(ref authReq);
    }

    public static Task<(uint, string)> EncodeContainerRequestAsync(ContainersReq containersReq) {
      return AppResolver.Current.EncodeContainersReqAsync(ref containersReq);
    }

    public static Task<(uint, string)> EncodeShareMDataRequestAsync(ShareMDataReq shareMDataReq)
    {
      return AppResolver.Current.EncodeShareMDataReqAsync(ref shareMDataReq);
    }

    public static Task<(uint, string)> EncodeUnregisteredRequestAsync(string reqId) {
      return AppResolver.Current.EncodeUnregisteredReqAsync(Encoding.UTF8.GetBytes(reqId));
    }

    public static async Task InitLoggingAsync(string configFilesPath)
    {
      await AppResolver.Current.AppSetAdditionalSearchPathAsync(configFilesPath);
      await AppResolver.Current.AppInitLoggingAsync(null);
    }

    public void FreeApp() {
      _appBindings.AppFree(_appPtr);
    }

  }
}
