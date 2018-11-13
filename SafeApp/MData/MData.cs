using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp.MData
{
    /// <summary>
    /// Provides the Mutable Data APIs for the session.
    /// </summary>
    [PublicAPI]
    public class MData
    {
        private static readonly IAppBindings AppBindings = AppResolver.Current;
        private SafeAppPtr _appPtr;

        /// <summary>
        /// Initializes an MData object for the Session instance.
        /// The app pointer is required to perform network operations.
        /// </summary>
        /// <param name="appPtr">SafeApp pointer.</param>
        internal MData(SafeAppPtr appPtr)
        {
            _appPtr = appPtr;
        }

        /// <summary>
        /// Delete the permissions of a specific key.
        /// Directly commits to the networks.
        /// Required 'ManagePermissions'-Permission for the app.
        /// </summary>
        /// <param name="mDataInfo">MDataInfo to access mutable data.</param>
        /// <param name="userSignPubKey">the key to lookup for.</param>
        /// <param name="version">the version successor, to handle the concurrency issue.</param>
        /// <returns></returns>
        public Task DelUserPermissionsAsync(MDataInfo mDataInfo, NativeHandle userSignPubKey, ulong version)
        {
            return AppBindings.MDataDelUserPermissionsAsync(_appPtr, ref mDataInfo, userSignPubKey, version);
        }

        /// <summary>
        /// Encode the Mutable data metadata response.
        /// </summary>
        /// <param name="metadataResponse">Metadata for user mutable data.</param>
        /// <returns>Encoded metadata.</returns>
        public Task<List<byte>> EncodeMetadata(MetadataResponse metadataResponse)
        {
            return AppBindings.MDataEncodeMetadataAsync(ref metadataResponse);
        }

        /// <summary>
        /// Loop up the value of a specific key.
        /// </summary>
        /// <param name="mDataInfo">MDataInfo to access mutable data.</param>
        /// <param name="key">Mutable data entry key</param>
        /// <returns>Mutable data entry value and its current version</returns>
        public Task<(List<byte>, ulong)> GetValueAsync(MDataInfo mDataInfo, List<byte> key)
        {
            return AppBindings.MDataGetValueAsync(_appPtr, ref mDataInfo, key);
        }

        /// <summary>
        /// Loop up the Mutable data version on the network
        /// </summary>
        /// <param name="mDataInfo">MDataInfo to access mutable data.</param>
        /// <returns>Mutable data version.</returns>
        public Task<ulong> GetVersionAsync(MDataInfo mDataInfo)
        {
            return AppBindings.MDataGetVersionAsync(_appPtr, ref mDataInfo);
        }

        /// <summary>
        /// Get the list of Mutable data entries associated with handle.
        /// </summary>
        /// <param name="entriesHandle">Mutable data entry handle.</param>
        /// <returns>List of mutable data Entries.</returns>
        public Task<List<MDataEntry>> ListEntriesAsync(NativeHandle entriesHandle)
        {
            return AppBindings.MDataListEntriesAsync(_appPtr, entriesHandle);
        }

        /// <summary>
        /// Get the list of keys contained in mutable data.
        /// </summary>
        /// <param name="mDataInfo">MDataInfo to access mutable data.</param>
        /// <returns>List of mutable data keys.</returns>
        public Task<List<MDataKey>> ListKeysAsync(MDataInfo mDataInfo)
        {
            return AppBindings.MDataListKeysAsync(_appPtr, ref mDataInfo);
        }

        /// <summary>
        /// Get a handle to the permissions associated with mutable data.
        /// </summary>
        /// <param name="mDataInfo">MDataInfo to access mutable data.</param>
        /// <returns>Permission handle for mutable data.</returns>
        public async Task<NativeHandle> ListPermissionsAsync(MDataInfo mDataInfo)
        {
            var handle = await AppBindings.MDataListPermissionsAsync(_appPtr, ref mDataInfo);
            return new NativeHandle(_appPtr, handle, freeHandle => AppBindings.MDataPermissionsFreeAsync(_appPtr, freeHandle));
        }

        /// <summary>
        /// Get a handle to the permissions associated with MutableData for a specific public sign key.
        /// </summary>
        /// <param name="mDataInfo">MDataInfo to access mutable data.</param>
        /// <param name="userSignPubKey">User public signing key.</param>
        /// <returns>Permission set for user for a mutable data.</returns>
        public Task<PermissionSet> ListUserPermissionsAsync(MDataInfo mDataInfo, NativeHandle userSignPubKey)
        {
            return AppBindings.MDataListUserPermissionsAsync(_appPtr, ref mDataInfo, userSignPubKey);
        }

        /// <summary>
        /// Get the list of values from a MutableData.
        /// </summary>
        /// <param name="mDataInfo">MDataInfo to access mutable data.</param>
        /// <returns>List of mutable data values.</returns>
        public Task<List<MDataValue>> ListValuesAsync(MDataInfo mDataInfo)
        {
            return AppBindings.MDataListValuesAsync(_appPtr, ref mDataInfo);
        }

        /// <summary>
        /// Commit the transaction to the network.
        /// </summary>
        /// <param name="mDataInfo">MData info of mutable data.</param>
        /// <param name="entryActionsH">Entry action handle.</param>
        /// <returns></returns>
        public Task MutateEntriesAsync(MDataInfo mDataInfo, NativeHandle entryActionsH)
        {
            return AppBindings.MDataMutateEntriesAsync(_appPtr, ref mDataInfo, entryActionsH);
        }

        /// <summary>
        /// Commit this mutable data to the network.
        /// </summary>
        /// <param name="mDataInfo">MDataInfo to access mutable data.</param>
        /// <param name="permissionsH">permissions to create the mutable data with.</param>
        /// <param name="entriesH">Mutable data entries to create the mutable data with.</param>
        /// <returns></returns>
        public Task PutAsync(MDataInfo mDataInfo, NativeHandle permissionsH, NativeHandle entriesH)
        {
            return AppBindings.MDataPutAsync(_appPtr, ref mDataInfo, permissionsH, entriesH);
        }

        /// <summary>
        /// Get serialized size of mutable data.
        /// </summary>
        /// <param name="mDataInfo">MDataInfo to access mutable data.</param>
        /// <returns>Size of mutable data.</returns>
        public Task<ulong> SerialisedSizeAsync(MDataInfo mDataInfo)
        {
            return AppBindings.MDataSerialisedSizeAsync(_appPtr, ref mDataInfo);
        }

        /// <summary>
        /// Set the permissions of a specific key. Directly commits to the network.
        /// Requires 'ManagePermissions'-Permission for the app.
        /// </summary>
        /// <param name="mDataInfo">MDataInfo to access mutable data.</param>
        /// <param name="userSignPubKey">user public sign key.</param>
        /// <param name="permissionSet">permission set to set to.</param>
        /// <param name="version">version successor, to confirm you are actually asking for the right one.</param>
        /// <returns></returns>
        public Task SetUserPermissionsAsync(MDataInfo mDataInfo, NativeHandle userSignPubKey, PermissionSet permissionSet, ulong version)
        {
            return AppBindings.MDataSetUserPermissionsAsync(_appPtr, ref mDataInfo, userSignPubKey, ref permissionSet, version);
        }
    }
}
