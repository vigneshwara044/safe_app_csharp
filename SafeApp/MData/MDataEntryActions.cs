using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp.MData
{
    /// <summary>
    /// Holds a mutation action to be done to the MutableData entries within one transaction on the network.
    /// You need this whenever you want to change the content of the entries.
    /// </summary>
    [PublicAPI]
    public class MDataEntryActions
    {
        private static readonly IAppBindings AppBindings = AppResolver.Current;
        private SafeAppPtr _appPtr;

        /// <summary>
        /// Initializes an MDataEntryAction object for the Session instance.
        /// The app pointer is required to perform network operations.
        /// </summary>
        /// <param name="appPtr">SafeApp pointer.</param>
        internal MDataEntryActions(SafeAppPtr appPtr)
        {
            _appPtr = appPtr;
        }

        /// <summary>
        /// Store a new Remove-Action in the transaction to remove an existing entry.
        /// </summary>
        /// <param name="entryActionsH">EntryActions handle.</param>
        /// <param name="entKey">The key you want to remove.</param>
        /// <param name="version">The version successor, to confirm you are actually asking for the right version.</param>
        /// <returns></returns>
        public Task DeleteAsync(NativeHandle entryActionsH, List<byte> entKey, ulong version)
        {
            return AppBindings.MDataEntryActionsDeleteAsync(_appPtr, entryActionsH, entKey, version);
        }

        private Task FreeAsync(ulong entryActionsH)
        {
            return AppBindings.MDataEntryActionsFreeAsync(_appPtr, entryActionsH);
        }

        /// <summary>
        /// Store a new Insert-Action in the transaction to store a new entry.
        /// </summary>
        /// <param name="entryActionsH">EntryActions handle.</param>
        /// <param name="entKey">The key you want to insert.</param>
        /// <param name="entVal">The value you want to insert.</param>
        /// <returns></returns>
        public Task InsertAsync(NativeHandle entryActionsH, List<byte> entKey, List<byte> entVal)
        {
            return AppBindings.MDataEntryActionsInsertAsync(_appPtr, entryActionsH, entKey, entVal);
        }

        /// <summary>
        /// Get a new handle to perform mutable data entry transaction actions.
        /// </summary>
        /// <returns>New Handle to perform entry actions.</returns>
        public async Task<NativeHandle> NewAsync()
        {
            var entryActionsH = await AppBindings.MDataEntryActionsNewAsync(_appPtr);
            return new NativeHandle(_appPtr, entryActionsH, FreeAsync);
        }

        /// <summary>
        /// Store a Update-Action in the transaction to update an existing entry.
        /// </summary>
        /// <param name="entryActionsH">EntryActions handle.</param>
        /// <param name="entKey">the key for the entry you want to update</param>
        /// <param name="entVal">the value to update to</param>
        /// <param name="version">the version successor, to confirm you are actually asking for the right version</param>
        /// <returns></returns>
        public Task UpdateAsync(NativeHandle entryActionsH, List<byte> entKey, List<byte> entVal, ulong version)
        {
            return AppBindings.MDataEntryActionsUpdateAsync(_appPtr, entryActionsH, entKey, entVal, version);
        }
    }
}
