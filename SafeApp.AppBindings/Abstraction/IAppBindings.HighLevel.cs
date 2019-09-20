using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SafeApp.Core;

namespace SafeApp.AppBindings
{
    public partial interface IAppBindings
    {
        void SafeConnect(
            string appId,
            string authCredentials,
            Action<FfiResult, IntPtr, GCHandle> oCb);

        #region Keys

        Task<BlsKeyPair> GenerateKeyPairAsync(ref IntPtr app);

        #endregion Keys
    }
}
