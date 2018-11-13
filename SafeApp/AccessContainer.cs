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
        /// Get the list of container access permissions granted for the app.
        /// Permission for each of them.
        /// </summary>
        /// <returns>List of ContainerPermissions.</returns>
        public async Task<List<ContainerPermissions>> AccessContainerFetchAsync()
        {
            var array = await AppBindings.AccessContainerFetchAsync(_appPtr);
            return array.ToList();
        }

        /// <summary>
        /// Get the access information for a specific container.
        /// Would throw exception if the container specified is not granted access.
        /// </summary>
        /// <param name="containerId">Name of the container.</param>
        /// <returns>MDataInfo of access container's mutable data.</returns>
        public Task<MDataInfo> GetMDataInfoAsync(string containerId)
        {
            return AppBindings.AccessContainerGetContainerMDataInfoAsync(_appPtr, containerId);
        }

        /// <summary>
        /// Refresh the access permissions from the network.
        /// Invoked to keep the access container information updated.
        /// </summary>
        /// <returns></returns>
        public Task RefreshAccessInfoAsync()
        {
            return AppBindings.AccessContainerRefreshAccessInfoAsync(_appPtr);
        }
    }
}
