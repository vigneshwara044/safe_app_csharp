using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

// ReSharper disable InconsistentNaming

namespace SafeApp.IData
{
    /// <summary>
    /// Interact with Immutable Data of the Network through this Interface.
    /// </summary>
    [PublicAPI]
    public class IData
    {
        private static readonly IAppBindings AppBindings = AppResolver.Current;
        private SafeAppPtr _appPtr;

        /// <summary>
        /// Initializes an IData object for the Session instance.
        /// The app pointer will is required to perform network operations.
        /// </summary>
        /// <param name="appPtr">SafeApp pointer.</param>
        internal IData(SafeAppPtr appPtr)
        {
            _appPtr = appPtr;
        }

        /// <summary>
        /// Close and write the immutable Data to the network
        /// </summary>
        /// <param name="seH"></param>
        /// <param name="cipherOptH"></param>
        /// <returns>the address to the data written to the network</returns>
        public Task<byte[]> CloseSelfEncryptorAsync(ulong seH, NativeHandle cipherOptH)
        {
            return AppBindings.IDataCloseSelfEncryptorAsync(_appPtr, seH, cipherOptH);
        }

        /// <summary>
        /// Loop up an existing Immutable Data for the given address
        /// </summary>
        /// <param name="xorName">the XorName on the network</param>
        /// <returns>SE handle</returns>
        public async Task<NativeHandle> FetchSelfEncryptorAsync(byte[] xorName)
        {
            var sEReaderHandle = await AppBindings.IDataFetchSelfEncryptorAsync(_appPtr, xorName);
            return new NativeHandle(_appPtr, sEReaderHandle, SelfEncryptorReaderFreeAsync);
        }

        /// <summary>
        /// Create a new ImmutableDataInterface
        /// </summary>
        /// <returns></returns>
        public async Task<NativeHandle> NewSelfEncryptorAsync()
        {
            var sEWriterHandle = await AppBindings.IDataNewSelfEncryptorAsync(_appPtr);
            return new NativeHandle(_appPtr, sEWriterHandle, null);
        }

        /// <summary>
        /// Read the given amount of bytes from the network
        /// </summary>
        /// <param name="seHandle">SE handle.</param>
        /// <param name="fromPos">start position</param>
        /// <param name="len">end position or end of data</param>
        /// <returns></returns>
        public async Task<List<byte>> ReadFromSelfEncryptorAsync(NativeHandle seHandle, ulong fromPos, ulong len)
        {
            var dataArray = await AppBindings.IDataReadFromSelfEncryptorAsync(_appPtr, seHandle, fromPos, len);
            return new List<byte>(dataArray);
        }

        /// <summary>
        /// Free the reference of reader of the app on the native side
        /// </summary>
        /// <param name="sEReaderHandle"></param>
        /// <returns></returns>
        private Task SelfEncryptorReaderFreeAsync(ulong sEReaderHandle)
        {
            return AppBindings.IDataSelfEncryptorReaderFreeAsync(_appPtr, sEReaderHandle);
        }

        /// <summary>
        /// Free the reference of writer of the app on the native side.
        /// </summary>
        /// <param name="sEWriterHandle">SE Writer handle of immutable data.</param>
        /// <returns></returns>
        private Task SelfEncryptorWriterFreeAsync(ulong sEWriterHandle)
        {
            return AppBindings.IDataSelfEncryptorWriterFreeAsync(_appPtr, sEWriterHandle);
        }

        /// <summary>
        /// The size of the serialized Immutable Data on the network.
        /// </summary>
        /// <param name="xorName">XOR address of Immutable Data.</param>
        /// <returns>length in bytes.</returns>
        public Task<ulong> SerialisedSizeAsync(byte[] xorName)
        {
            return AppBindings.IDataSerialisedSizeAsync(_appPtr, xorName);
        }

        /// <summary>
        /// Get the size of the Immutable data on the network.
        /// </summary>
        /// <param name="seHandle">SE handle.</param>
        /// <returns>length in bytes.</returns>
        public Task<ulong> SizeAsync(NativeHandle seHandle)
        {
            return AppBindings.IDataSizeAsync(_appPtr, seHandle);
        }

        /// <summary>
        /// Append the data to Immutable data.
        /// </summary>
        /// <param name="seHandle">SE handle</param>
        /// <param name="data">Data to append in existing immutable data.</param>
        /// <returns></returns>
        public Task WriteToSelfEncryptorAsync(NativeHandle seHandle, List<byte> data)
        {
            return AppBindings.IDataWriteToSelfEncryptorAsync(_appPtr, seHandle, data);
        }
    }
}
