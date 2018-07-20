using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction
namespace SafeApp
{
    /// <summary>
    /// Access Container APIs
    /// </summary>
    [PublicAPI]
    public class AccessContainer
    {
        private static readonly IAppBindings AppBindings = AppResolver.Current;
        private SafeAppPtr _appPtr;

        /// <summary>
        /// Initializes an AccessContainer object for the Session instance.
        /// The app pointer is required to perform network operations.
        /// </summary>
        /// <param name="appPtr">SafeApp pointer.</param>
        internal AccessContainer(SafeAppPtr appPtr)
        {
            _appPtr = appPtr;
        }

        /// <summary>
        /// Get the names of all containers found and app's granted.
        /// Permission for each of them.
        /// </summary>
        /// <returns>List of containers and permissions.</returns>
        public async Task<List<ContainerPermissions>> AccessContainerFetchAsync()
        {
            var array = await AppBindings.AccessContainerFetchAsync(_appPtr);
            return array.ToList();
        }

        /// <summary>
        /// Lookup and return the information necessary to access a container.
        /// </summary>
        /// <param name="containerId">Name of the container.</param>
        /// <returns>MDataInfo of access container's mutable data.</returns>
        public Task<MDataInfo> GetMDataInfoAsync(string containerId)
        {
            return AppBindings.AccessContainerGetContainerMDataInfoAsync(_appPtr, containerId);
        }

        /// <summary>
        /// Refresh the access persmissions from the network.
        /// Useful when you just connected or received a response from the authenticator in the IPC protocol.
        /// </summary>
        /// <returns></returns>
        public Task RefreshAccessInfoAsync()
        {
            return AppBindings.AccessContainerRefreshAccessInfoAsync(_appPtr);
        }
    }
}
