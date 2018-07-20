using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp.MData
{
    /// <summary>
    /// Represent the entries of a mutable data network object.
    /// </summary>
    [PublicAPI]
    public class MDataEntries
    {
        private static readonly IAppBindings AppBindings = AppResolver.Current;
        private SafeAppPtr _appPtr;

        /// <summary>
        /// Initializes an MDataEntries object for the Session instance.
        /// The app pointer is required to perform network operations.
        /// </summary>
        /// <param name="appPtr">SafeApp pointer.</param>
        internal MDataEntries(SafeAppPtr appPtr)
        {
            _appPtr = appPtr;
        }

        /// <summary>
        /// Used to clean the mutable data keys references.
        /// </summary>
        /// <param name="entriesH">Mutable data entries handle.</param>
        /// <returns></returns>
        private Task FreeAsync(ulong entriesH)
        {
            return AppBindings.MDataEntriesFreeAsync(_appPtr, entriesH);
        }

        /// <summary>
        /// Look up the value of a specific key.
        /// </summary>
        /// <param name="entriesHandle">MDataEntries handle.</param>
        /// <param name="key">The key to lookup.</param>
        /// <returns>Ihe entry's value and the current version.</returns>
        public Task<(List<byte>, ulong)> GetAsync(NativeHandle entriesHandle, List<byte> key)
        {
            return AppBindings.MDataEntriesGetAsync(_appPtr, entriesHandle, key);
        }

        /// <summary>
        /// Get a handle to the entries associated with mutable data.
        /// </summary>
        /// <param name="mDataInfo">MDataInfo to access mutable data.</param>
        /// <returns>Handle to MDataEntries.</returns>
        public async Task<NativeHandle> GetHandleAsync(MDataInfo mDataInfo)
        {
            var handle = await AppBindings.MDataEntriesAsync(_appPtr, ref mDataInfo);
            return new NativeHandle(_appPtr, handle, FreeAsync);
        }

        /// <summary>
        /// Insert a new entry. Once you call mutable data put with this entry,
        /// it will fail if the entry already exists or
        /// the current app doesn't have the permissions to edit that mutable data.
        /// </summary>
        /// <param name="entriesH">Handle to MDataEntries.</param>
        /// <param name="entKey">The key you want to store the data under.</param>
        /// <param name="entVal">The value you want to store.</param>
        /// <returns></returns>
        public Task InsertAsync(NativeHandle entriesH, List<byte> entKey, List<byte> entVal)
        {
            return AppBindings.MDataEntriesInsertAsync(_appPtr, entriesH, entKey, entVal);
        }

        /// <summary>
        /// Get the total number of entries in the mutable data.
        /// </summary>
        /// <param name="entriesHandle">Handle to Mutable data entries.</param>
        /// <returns>Number of mutable data entries.</returns>
        public Task<ulong> LenAsync(NativeHandle entriesHandle)
        {
            return AppBindings.MDataEntriesLenAsync(_appPtr, entriesHandle);
        }

        /// <summary>
        /// Create a handle for new entry in mutable data.
        /// </summary>
        /// <returns>Newly created mutable data entry handle.</returns>
        public async Task<NativeHandle> NewAsync()
        {
            var entriesH = await AppBindings.MDataEntriesNewAsync(_appPtr);
            return new NativeHandle(_appPtr, entriesH, FreeAsync);
        }
    }
}
