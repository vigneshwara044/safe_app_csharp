using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp.MData
{
    /// <summary>
    /// Mutable data entries APIs.
    /// </summary>
    [PublicAPI]
    public class MDataEntries
    {
        private static readonly IAppBindings AppBindings = AppResolver.Current;
        private SafeAppPtr _appPtr;

        /// <summary>
        /// Initialises an MDataEntries object for the Session instance.
        /// The app pointer is required to perform network operations.
        /// </summary>
        /// <param name="appPtr">SafeApp pointer.</param>
        internal MDataEntries(SafeAppPtr appPtr)
        {
            _appPtr = appPtr;
        }

        /// <summary>
        /// Free the entries from memory.
        /// </summary>
        /// <param name="entriesH">Mutable data entries handle.</param>
        /// <returns></returns>
        private Task FreeAsync(ulong entriesH)
        {
            return AppBindings.MDataEntriesFreeAsync(_appPtr, entriesH);
        }

        /// <summary>
        /// Get the value for the specified key.
        /// </summary>
        /// <param name="entriesHandle">MDataEntries handle.</param>
        /// <param name="key">The key to lookup.</param>
        /// <returns>Value corresponding to the specified key and it's version.</returns>
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
        /// Insert a new entry.
        /// it will fail if the entry already exists or
        /// the current app doesn't have the permissions to edit that mutable data.
        /// </summary>
        /// <param name="entriesH">Handle to MDataEntries.</param>
        /// <param name="entKey">The key to store the data under.</param>
        /// <param name="entVal">The value to be stored.</param>
        /// <returns></returns>
        public Task InsertAsync(NativeHandle entriesH, List<byte> entKey, List<byte> entVal)
        {
            return AppBindings.MDataEntriesInsertAsync(_appPtr, entriesH, entKey, entVal);
        }

        /// <summary>
        /// Get the total number of entries in the mutable data.
        /// Deleted entries are also included in total number.
        /// </summary>
        /// <param name="entriesHandle">Handle to Mutable data entries.</param>
        /// <returns>Number of mutable data entries.</returns>
        public Task<ulong> LenAsync(NativeHandle entriesHandle)
        {
            return AppBindings.MDataEntriesLenAsync(_appPtr, entriesHandle);
        }

        /// <summary>
        /// Create a new entry handle to add entries in a MutableData.
        /// </summary>
        /// <returns>Newly created mutable data entry handle.</returns>
        public async Task<NativeHandle> NewAsync()
        {
            var entriesH = await AppBindings.MDataEntriesNewAsync(_appPtr);
            return new NativeHandle(_appPtr, entriesH, FreeAsync);
        }
    }
}
