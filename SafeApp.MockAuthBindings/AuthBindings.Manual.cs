using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SafeApp.Utilities;

namespace SafeApp.MockAuthBindings {
  internal partial class AuthBindings {
#if __IOS__
    [MonoPInvokeCallback(typeof(NoneCb))]
#endif
    private static void OnAuthenticatorDisconnectCb(IntPtr userData) {
      var (action, _) = BindingUtils.FromHandlePtr<(Action, Action<FfiResult, IntPtr, GCHandle>)>(userData, false);

      action();
    }

#if __IOS__
    [MonoPInvokeCallback(typeof(FfiResultAuthenticatorCb))]
#endif
    private static void OnAuthenticatorCreateCb(IntPtr userData, IntPtr result, IntPtr app) {
      var (_, action) = BindingUtils.FromHandlePtr<(Action, Action<FfiResult, IntPtr, GCHandle>)>(userData, false);

      action(Marshal.PtrToStructure<FfiResult>(result), app, GCHandle.FromIntPtr(userData));
    }

    public void CreateAccount(
      string locator,
      string secret,
      string invitation,
      Action disconnnectedCb,
      Action<FfiResult, IntPtr, GCHandle> cb) {
      var userData = BindingUtils.ToHandlePtr((disconnnectedCb, cb));
      CreateAccNative(locator, secret, invitation, userData, OnAuthenticatorDisconnectCb, OnAuthenticatorCreateCb);
    }

    public void Login(
      string locator,
      string secret,
      Action disconnnectedCb,
      Action<FfiResult, IntPtr, GCHandle> cb)
    {
      var userData = BindingUtils.ToHandlePtr((disconnnectedCb, cb));
      LoginNative(locator, secret, userData, OnAuthenticatorDisconnectCb, OnAuthenticatorCreateCb);
    }

#if __IOS__
    [MonoPInvokeCallback(typeof(UIntAuthReqCb))]
#endif
    private static void OnDecodeIpcReqAuthCb(IntPtr userData, uint reqId, IntPtr authReq)
    {
      var tcs = BindingUtils.FromHandlePtr<TaskCompletionSource<IpcReq>>(userData);
      tcs.SetResult(new AuthIpcReq(reqId, new AuthReq(Marshal.PtrToStructure<AuthReqNative>(authReq))));
    }

#if __IOS__
    [MonoPInvokeCallback(typeof(UIntContainersReqCb))]
#endif
    private static void OnDecodeIpcReqContainersCb(IntPtr userData, uint reqId, IntPtr authReq)
    {
      var tcs = BindingUtils.FromHandlePtr<TaskCompletionSource<IpcReq>>(userData);
      tcs.SetResult(new ContainersIpcReq(reqId, new ContainersReq(Marshal.PtrToStructure<ContainersReqNative>(authReq))));
    }

#if __IOS__
    [MonoPInvokeCallback(typeof(UIntByteListCb))]
#endif
    private static void OnDecodeIpcReqUnregisteredCb(IntPtr userData, uint reqId, IntPtr authReq, ulong size) {
      var tcs = BindingUtils.FromHandlePtr<TaskCompletionSource<IpcReq>>(userData);
      tcs.SetResult(new UnregisteredIpcReq(reqId, authReq, size));
    }

#if __IOS__
    [MonoPInvokeCallback(typeof(UIntShareMDataReqMetadataResponseCb))]
#endif
    private static void OnDecodeIpcReqShareMDataCb(IntPtr userData, uint reqId, IntPtr authReq, IntPtr metadata)
    {
      var tcs = BindingUtils.FromHandlePtr<TaskCompletionSource<IpcReq>>(userData);
      var shareMdReq = new ShareMDataReq(Marshal.PtrToStructure<ShareMDataReqNative>(authReq));Marshal.PtrToStructure<ShareMDataReqNative>(authReq);
      var metadataList = metadata.ToList<string>((IntPtr) shareMdReq.MData.Count);
      tcs.SetResult(new ShareMDataIpcReq(reqId, shareMdReq, metadataList));
    }
#if __IOS__
    [MonoPInvokeCallback(typeof(FfiResultstringCb))]
#endif
    private static void OnDecodeIpcReqErrorCb(IntPtr userData, IntPtr ffiResult, string msg)
    {
      var tcs = BindingUtils.FromHandlePtr<TaskCompletionSource<IpcReq>>(userData);
      var res = Marshal.PtrToStructure<FfiResult>(ffiResult);
      tcs.SetException(new IpcReqException(msg, res.ErrorCode, res.Description));
    }

    public Task<IpcReq> DecodeIpcMessage(IntPtr authPtr, string msg) {
      var (task, userData) = BindingUtils.PrepareTask<IpcReq>();
      AuthDecodeIpcMsgNative(authPtr, msg, userData, OnDecodeIpcReqAuthCb, OnDecodeIpcReqContainersCb, OnDecodeIpcReqUnregisteredCb, OnDecodeIpcReqShareMDataCb, OnDecodeIpcReqErrorCb);
      return task;
    }
  }
}
