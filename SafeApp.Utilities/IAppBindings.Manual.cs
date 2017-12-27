using System;
using System.Threading.Tasks;

namespace SafeApp.Utilities {
  public partial interface IAppBindings {
    void AppRegistered(string appId, ref AuthGranted authGranted, Action oDisconnectNotifierCb, Action<FfiResult, IntPtr> oCb);
    void AppUnregistered(byte[] bootstrapConfig, Action oDisconnectNotifierCb, Action<FfiResult, IntPtr> oCb);
    Task<IpcMsg> DecodeIpcMsgAsync(string msg);
  }
}
