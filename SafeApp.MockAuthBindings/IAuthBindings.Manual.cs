using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SafeApp.Utilities;

namespace SafeApp.MockAuthBindings
{
    internal partial interface IAuthBindings
    {
      void CreateAccount(string locator, string secret, string invitation, Action disconnnectedCb, Action<FfiResult, IntPtr, GCHandle> cb);
      void Login(string locator, string secret, Action disconnnectedCb, Action<FfiResult, IntPtr, GCHandle> cb);
      Task<IpcReq> DecodeIpcMessage(IntPtr authPtr, string uri);
    }
}
