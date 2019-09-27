using System;
using System.Threading.Tasks;
using SafeApp.AppBindings;
using SafeApp.Core;

namespace SafeApp.API
{
    public class Fetch
    {
        static readonly IAppBindings AppBindings = AppResolver.Current;
        IntPtr _appPtr;

        internal Fetch(SafeAppPtr appPtr)
            => _appPtr = appPtr;

        public Task<ISafeData> FetchAsync(string url)
            => AppBindings.FetchAsync(ref _appPtr, url);
    }
}
