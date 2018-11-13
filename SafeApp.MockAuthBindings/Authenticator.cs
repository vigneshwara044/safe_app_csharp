using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SafeApp.Utilities;

namespace SafeApp.MockAuthBindings
{
    /// <summary>
    /// The Authenticator contains all authentication related functionality for the network.
    /// </summary>
    // ReSharper disable ConvertToLocalFunction
    // ReSharper disable UnusedMember.Global
    // ReSharper disable MemberCanBePrivate.Global
    public class Authenticator : IDisposable
    {
        private static readonly IAuthBindings NativeBindings = MockAuthResolver.Current;

        // ReSharper disable once UnassignedField.Global

        /// <summary>
        /// Event triggered if session is disconnected from network.
        /// </summary>
#pragma warning disable SA1401 // Fields should be private
        public static EventHandler Disconnected;
#pragma warning restore SA1401 // Fields should be private
        private IntPtr _authPtr;
        private GCHandle _disconnectedHandle;

        /// <summary>
        /// Returns true if current network connection state is DISCONNECTED.
        /// </summary>
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public bool IsDisconnected { get; private set; }

        /// <summary>
        /// Returns true if the underlying library was compiled against mock-routing.
        /// </summary>
        /// <returns>True if compiled again mock-routing otherwise false.</returns>
        public static bool IsMockBuild()
        {
            return NativeBindings.IsMockBuild();
        }

        /// <summary>
        /// Returns the expected name for the authenticator executable without an extension.
        /// </summary>
        /// <returns>Authenticator executable name.</returns>
        public static Task<string> AuthExeFileStemAsync()
        {
            return NativeBindings.AuthExeFileStemAsync();
        }

        /// <summary>
        /// Generate the log path for the provided filename.
        /// If the filename provided is null, then it returns the path of where the safe_core log file is located.
        /// </summary>
        /// <param name="outputFileName">Log file name.</param>
        /// <returns></returns>
        public static Task AuthInitLoggingAsync(string outputFileName)
        {
            return NativeBindings.AuthInitLoggingAsync(outputFileName);
        }

        /// <summary>
        /// Generate the log path for the provided filename.
        /// If the filename provided is null, then it returns the path of where the safe_core log file is located.
        /// </summary>
        /// <param name="outputFileName">Log file name.</param>
        /// <returns></returns>
        public static Task AuthOutputLogPathAsync(string outputFileName)
        {
            return NativeBindings.AuthOutputLogPathAsync(outputFileName);
        }

        /// <summary>
        /// Create new Account with a provided set of keys.
        /// </summary>
        /// <param name="locator">Account username/locator.</param>
        /// <param name="secret">Account password/key.</param>
        /// <param name="invitation">Invitation token.</param>
        /// <returns>New Authenticator object.</returns>
        public static Task<Authenticator> CreateAccountAsync(string locator, string secret, string invitation)
        {
            return Task.Run(
              () =>
              {
                  var authenticator = new Authenticator();
                  var tcs = new TaskCompletionSource<Authenticator>(TaskCreationOptions.RunContinuationsAsynchronously);
                  Action disconnect = () => { OnDisconnected(authenticator); };
                  Action<FfiResult, IntPtr, GCHandle> cb = (result, ptr, disconnectHandle) =>
                  {
                      if (result.ErrorCode != 0)
                      {
                          if (disconnectHandle.IsAllocated)
                          {
                              disconnectHandle.Free();
                          }

                          tcs.SetException(result.ToException());
                          return;
                      }

                      authenticator.Init(ptr, disconnectHandle);
                      tcs.SetResult(authenticator);
                  };
                  NativeBindings.CreateAccount(locator, secret, invitation, disconnect, cb);
                  return tcs.Task;
              });
        }

        /// <summary>
        /// Decode the incoming unregistered client authentication message.
        /// </summary>
        /// <param name="msg">Message string.</param>
        /// <returns>New IpcReq object.</returns>
        public static Task<IpcReq> UnRegisteredDecodeIpcMsgAsync(string msg)
        {
            return NativeBindings.UnRegisteredDecodeIpcMsgAsync(msg);
        }

        /// <summary>
        /// Encode unregistered client authentication response.
        /// </summary>
        /// <param name="reqId">Request id.</param>
        /// <param name="allow">Pass true to allow unregistered client authentication request. False to deny.</param>
        /// <returns>Encoded unregistered client authentication response string.</returns>
        public static Task<string> EncodeUnregisteredRespAsync(uint reqId, bool allow)
        {
            return NativeBindings.EncodeUnregisteredRespAsync(reqId, allow);
        }

        /// <summary>
        /// Sets the additional path in `config_file_handler` to search configuration files.
        /// </summary>
        /// <param name="newPath">Configuration file path.</param>
        /// <returns></returns>
        public static Task AuthSetAdditionalSearchPathAsync(string newPath)
        {
            return NativeBindings.AuthSetAdditionalSearchPathAsync(newPath);
        }

        private Authenticator()
        {
            IsDisconnected = true;
            _authPtr = IntPtr.Zero;
        }

        /// <summary>
        /// Public implementation of Dispose pattern callable by developers.
        /// </summary>
        public void Dispose()
        {
            FreeAuth();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Returns account information.
        /// e.g. number of mutations done and available.
        /// </summary>
        /// <returns>New AccountInfo object.</returns>
        public Task<AccountInfo> AuthAccountInfoAsync()
        {
            return NativeBindings.AuthAccountInfoAsync(_authPtr);
        }

        /// <summary>
        /// Return a list of apps having access to an arbitrary mutable data object.
        /// name and typeTag together correspond to a single mutable data.
        /// </summary>
        /// <param name="name">Mutable data name.</param>
        /// <param name="typeTag">Mutable data tagType.</param>
        /// <returns>List of Apps having access to mutable data.</returns>
        public Task<List<AppAccess>> AuthAppsAccessingMutableDataAsync(byte[] name, ulong typeTag)
        {
            return NativeBindings.AuthAppsAccessingMutableDataAsync(_authPtr, name, typeTag);
        }

        /// <summary>
        /// Flush the revocation queue and verify apps get revoked.
        /// </summary>
        /// <returns></returns>
        public Task AuthFlushAppRevocationQueueAsync()
        {
            return NativeBindings.AuthFlushAppRevocationQueueAsync(_authPtr);
        }

        /// <summary>
        /// Try to restore a failed connection with the network.
        /// </summary>
        /// <returns></returns>
        public Task AuthReconnectAsync()
        {
            return NativeBindings.AuthReconnectAsync(_authPtr);
        }

        /// <summary>
        /// Get the list of apps which was granted access by the user.
        /// </summary>
        /// <returns>List of registered apps.</returns>
        public Task<List<RegisteredApp>> AuthRegisteredAppsAsync()
        {
            return NativeBindings.AuthRegisteredAppsAsync(_authPtr);
        }

        /// <summary>
        ///  Revoke app access.
        /// </summary>
        /// <param name="appId">App id.</param>
        /// <returns></returns>
        public Task AuthRevokeAppAsync(string appId)
        {
            return NativeBindings.AuthRevokeAppAsync(_authPtr, appId);
        }

        /// <summary>
        /// Get a list of apps revoked from authenticator.
        /// </summary>
        /// <returns>List of revoked app's exchange info.</returns>
        public Task<List<AppExchangeInfo>> AuthRevokedAppsAsync()
        {
            return NativeBindings.AuthRevokedAppsAsync(_authPtr);
        }

        /// <summary>
        /// Removes a revoked app from the authenticator config.
        /// </summary>
        /// <param name="appId">App id.</param>
        /// <returns></returns>
        public Task AuthRmRevokedAppAsync(string appId)
        {
            return NativeBindings.AuthRmRevokedAppAsync(_authPtr, appId);
        }

        /// <summary>
        /// Decodes a given encoded IPC message.
        /// </summary>
        /// <param name="msg">Message string.</param>
        /// <returns>New IpcReq object.</returns>
        public Task<IpcReq> DecodeIpcMessageAsync(string msg)
        {
            return NativeBindings.DecodeIpcMessage(_authPtr, msg);
        }

        /// <summary>
        /// Provides and encodes an Authenticator response.
        /// </summary>
        /// <param name="authIpcReq">Authentication IPC request.</param>
        /// <param name="allow">true/false.</param>
        /// <returns></returns>
        public Task<string> EncodeAuthRespAsync(AuthIpcReq authIpcReq, bool allow)
        {
            return NativeBindings.EncodeAuthRespAsync(_authPtr, ref authIpcReq.AuthReq, authIpcReq.ReqId, allow);
        }

        /// <summary>
        /// Update containers permissions for an App.
        /// </summary>
        /// <param name="req">Containers IPC request.</param>
        /// <param name="allow">Pass true to accept the Container request and false to deny.</param>
        /// <returns>Encoded Containers permissions Response for an App.</returns>
        public Task<string> EncodeContainersRespAsync(ContainersIpcReq req, bool allow)
        {
            return NativeBindings.EncodeContainersRespAsync(_authPtr, ref req.ContainersReq, req.ReqId, allow);
        }

        /// <summary>
        /// Encode share mutable data response.
        /// </summary>
        /// <param name="req">Share mutable data IPC request.</param>
        /// <param name="allow">Pass true to accept the ShareMData request and false to deny.</param>
        /// <returns>Encoded ShareMData response.</returns>
        public Task<string> EncodeShareMdataRespAsync(ShareMDataIpcReq req, bool allow)
        {
            return NativeBindings.EncodeShareMDataRespAsync(_authPtr, ref req.ShareMDataReq, req.ReqId, allow);
        }

        /// <summary>
        /// Class destructor.
        /// </summary>
        ~Authenticator()
        {
            FreeAuth();
        }

        private void FreeAuth()
        {
            if (_disconnectedHandle.IsAllocated)
            {
                _disconnectedHandle.Free();
            }

            if (_authPtr == IntPtr.Zero)
            {
                return;
            }

            NativeBindings.AuthFree(_authPtr);
            _authPtr = IntPtr.Zero;
        }

        private void Init(IntPtr authPtr, GCHandle disconnectedHandle)
        {
            _authPtr = authPtr;
            _disconnectedHandle = disconnectedHandle;
            IsDisconnected = false;
        }

        /// <summary>
        /// Log-in to a registered account.
        /// </summary>
        /// <param name="locator">Account location/username.</param>
        /// <param name="secret">Account key/password.</param>
        /// <returns>New authenticator object.</returns>
        public static Task<Authenticator> LoginAsync(string locator, string secret)
        {
            return Task.Run(
              () =>
              {
                  var authenticator = new Authenticator();
                  var tcs = new TaskCompletionSource<Authenticator>(TaskCreationOptions.RunContinuationsAsynchronously);
                  Action disconnect = () => { OnDisconnected(authenticator); };
                  Action<FfiResult, IntPtr, GCHandle> cb = (result, ptr, disconnectHandle) =>
                  {
                      if (result.ErrorCode != 0)
                      {
                          if (disconnectHandle.IsAllocated)
                          {
                              disconnectHandle.Free();
                          }

                          tcs.SetException(result.ToException());
                          return;
                      }

                      authenticator.Init(ptr, disconnectHandle);
                      tcs.SetResult(authenticator);
                  };
                  NativeBindings.Login(locator, secret, disconnect, cb);
                  return tcs.Task;
              });
        }

        private static void OnDisconnected(Authenticator authenticator)
        {
            authenticator.IsDisconnected = true;
            Disconnected?.Invoke(authenticator, EventArgs.Empty);
        }
    }
}
