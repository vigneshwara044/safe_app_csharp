using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp.MData
{
    /// <summary>
    /// Holds a MDataInfo actions to be done to the MDataInfo.
    /// </summary>
    [PublicAPI]
    public class MDataInfoActions
    {
        private static readonly IAppBindings AppBindings = AppResolver.Current;
        private SafeAppPtr _appPtr;

        /// <summary>
        /// Initializes an MDataInfoActions object for the Session instance.
        /// The app pointer is required to perform network operations.
        /// </summary>
        /// <param name="appPtr"></param>
        internal MDataInfoActions(SafeAppPtr appPtr)
        {
            _appPtr = appPtr;
        }

        /// <summary>
        /// Decrypt the entry key/value provided as parameter
        /// with the encryption key contained in a Private MDataInfo.
        /// </summary>
        /// <param name="mDataInfo">MDataInfo to access mutable data.</param>
        /// <param name="cipherText">The data you want to decrypt.</param>
        /// <returns>The decrypted key/value.</returns>
        public Task<List<byte>> DecryptAsync(MDataInfo mDataInfo, List<byte> cipherText)
        {
            return AppBindings.MDataInfoDecryptAsync(ref mDataInfo, cipherText);
        }

        /// <summary>
        /// Create a new MDataInfo object from serialized value.
        /// </summary>
        /// <param name="serialisedData">Serialized value to create new MDataInfo object.</param>
        /// <returns>Newly create MDataInfo.</returns>
        public Task<MDataInfo> DeserialiseAsync(List<byte> serialisedData)
        {
            return AppBindings.MDataInfoDeserialiseAsync(serialisedData);
        }

        /// <summary>
        /// Encrypt the entry key provided as parameter with the encryption key
        /// contained in a Private MDataInfo.
        /// If the MutableData is public, the same (and uncrypted) value is returned.
        /// </summary>
        /// <param name="mDataInfo"></param>
        /// <param name="inputBytes"></param>
        /// <returns>The encrypted entry key.</returns>
        public Task<List<byte>> EncryptEntryKeyAsync(MDataInfo mDataInfo, List<byte> inputBytes)
        {
            return AppBindings.MDataInfoEncryptEntryKeyAsync(ref mDataInfo, inputBytes);
        }

        /// <summary>
        /// Encrypt the entry value provided as parameter with the encryption key
        /// contained in a Private MDataInfo.
        /// If the MutableData is Public, the same (and unencrypted) value is returned.
        /// </summary>
        /// <param name="mDataInfo"></param>
        /// <param name="inputBytes">The data you want to encrypt.</param>
        /// <returns>The encrypted entry value.</returns>
        public Task<List<byte>> EncryptEntryValueAsync(MDataInfo mDataInfo, List<byte> inputBytes)
        {
            return AppBindings.MDataInfoEncryptEntryValueAsync(ref mDataInfo, inputBytes);
        }

        /// <summary>
        /// Create a MDataInfo for MutableData at a given XOR address with private access.
        /// </summary>
        /// <param name="xorName">XOR address.</param>
        /// <param name="typeTag">The typeTag to use.</param>
        /// <param name="secEncKey">Secret encryption key.</param>
        /// <param name="nonce">nonce</param>
        /// <returns>Newly created MDataInfo.</returns>
        public Task<MDataInfo> NewPrivateAsync(byte[] xorName, ulong typeTag, byte[] secEncKey, byte[] nonce)
        {
            return AppBindings.MDataInfoNewPrivateAsync(xorName, typeTag, secEncKey, nonce);
        }

        /// <summary>
        /// Create a new MDataInfo for mutable data at a random address wih private access.
        /// </summary>
        /// <param name="typeTag">The typeTag to use.</param>
        /// <returns>Newly create MDataInfo.</returns>
        public Task<MDataInfo> RandomPrivateAsync(ulong typeTag)
        {
            return AppBindings.MDataInfoRandomPrivateAsync(typeTag);
        }

        /// <summary>
        /// Create a new MDataInfo for mutable data at a random address with public access.
        /// </summary>
        /// <param name="typeTag">The typeTag to use.</param>
        /// <returns>Newly create MDataInfo.</returns>
        public Task<MDataInfo> RandomPublicAsync(ulong typeTag)
        {
            return AppBindings.MDataInfoRandomPublicAsync(typeTag);
        }

        /// <summary>
        /// Serialize the MDataInfo.
        /// </summary>
        /// <param name="mDataInfo">MDataInfo to serialize.</param>
        /// <returns>List of serialized bytes.</returns>
        public async Task<List<byte>> SerialiseAsync(MDataInfo mDataInfo)
        {
            var byteArray = await AppBindings.MDataInfoSerialiseAsync(ref mDataInfo);
            return new List<byte>(byteArray);
        }
    }
}
