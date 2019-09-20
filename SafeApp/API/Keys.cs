using System;
using System.Threading.Tasks;
using SafeApp.AppBindings;
using SafeApp.Core;

namespace SafeApp.API
{
    /// <summary>
    /// Keys API.
    /// </summary>
    public class Keys
    {
        static readonly IAppBindings AppBindings = AppResolver.Current;
        IntPtr _appPtr;

        /// <summary>
        /// Initializes an Keys object for the Session instance.
        /// The app pointer is required to perform network operations.
        /// </summary>
        /// <param name="appPtr">SafeApp pointer.</param>
        internal Keys(SafeAppPtr appPtr)
            => _appPtr = appPtr;

        /// <summary>
        /// Keys API.
        /// </summary>
        public Task<BlsKeyPair> GenerateKeyPairAsync()
            => AppBindings.GenerateKeyPairAsync(ref _appPtr);
    }
}
