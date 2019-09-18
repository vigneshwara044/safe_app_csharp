using System;
using System.Runtime.InteropServices;
using SafeApp.Core;

namespace SafeApp.AppBindings
{
    public partial interface IAppBindings
    {
        void SafeConnect(
            string appId,
            string authCredentials,
            Action<FfiResult, IntPtr, GCHandle> oCb);
    }
}
