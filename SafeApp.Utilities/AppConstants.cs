using JetBrains.Annotations;

namespace SafeApp.Utilities
{
    /// <summary>
    /// Constants used in SafeApp
    /// </summary>
    [PublicAPI]
    public static class AppConstants
    {
        /// <summary>
        /// Length of Asymmetric key nonce
        /// </summary>
        public const ulong AsymNonceLen = 24;

        /// <summary>
        /// Length of Asymmetric public key
        /// </summary>
        public const ulong AsymPublicKeyLen = 32;

        /// <summary>
        /// Length of Asymmetric secret key
        /// </summary>
        public const ulong AsymSecretKeyLen = 32;

        /// <summary>
        /// Length of Symmetric public sign key
        /// </summary>
        public const ulong SignPublicKeyLen = 32;

        /// <summary>
        /// Length of Symmetric secret sign key
        /// </summary>
        public const ulong SignSecretKeyLen = 64;

        /// <summary>
        /// Length of Symmetric key
        /// </summary>
        public const ulong SymKeyLen = 32;

        /// <summary>
        /// Length of Symmetric key nonce
        /// </summary>
        public const ulong SymNonceLen = 24;

        /// <summary>
        /// `MutableData` type tag for a directory.
        /// </summary>
        public const ulong DirTag = 15000;

        /// <summary>
        /// All Maidsafe tagging should positive-offset from this.
        /// </summary>
        public const ulong MaidsafeTag = 5483000;

        /// <summary>
        /// Entry key under which the metadata are stored.
        /// </summary>
        public const string MDataMetaDataKey = "_metadata";

        /// <summary>
        /// Vaule for null handle
        /// </summary>
        public const ulong NullObjectHandle = 0;

        /// <summary>
        /// Constant byte length of `XorName`
        /// </summary>
        public const ulong XorNameLen = 32;
    }
}
