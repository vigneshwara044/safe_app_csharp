using System.Threading.Tasks;
using SafeApp.AppBindings;
using SafeApp.Core;

namespace SafeApp.API
{
    /// <summary>
    /// Content Fetch API.
    /// </summary>
    public class Fetch
    {
        static readonly IAppBindings AppBindings = AppResolver.Current;
        readonly SafeAppPtr _appPtr;

        /// <summary>
        /// Initialise a Fetch object for the Session instance.
        /// The app pointer is required to perform network operations.
        /// </summary>
        /// <param name="appPtr"></param>
        internal Fetch(SafeAppPtr appPtr)
            => _appPtr = appPtr;

        /// <summary>
        /// Fetch content from the SAFE Network.
        /// </summary>
        /// <param name="url">safe:// url to fetch the content.</param>
        /// <returns>New ISafeData instance based on the content type</returns>
        public Task<ISafeData> FetchAsync(string url)
            => AppBindings.FetchAsync(_appPtr, url);
    }
}
