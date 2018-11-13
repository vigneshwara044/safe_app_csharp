using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

[assembly: InternalsVisibleTo("SafeApp.AppBindings")]
[assembly: InternalsVisibleTo("SafeApp.MockAuthBindings")]

namespace SafeApp.Utilities
{
    /// <summary>
    /// Actions which can be performed on mutable data.
    /// </summary>
    [PublicAPI]
    public enum MDataAction
    {
        Insert,
        Update,
        Delete,
        ManagePermissions
    }

    /// <summary>
    /// Holds the information about the account.
    /// </summary>
    [PublicAPI]
    public struct AccountInfo
    {
        /// <summary>
        /// Number of mutations performed.
        /// </summary>
        public ulong MutationsDone;

        /// <summary>
        /// Number of remaining Mutations.
        /// </summary>
        public ulong MutationsAvailable;
    }

    /// <summary>
    /// Information allowing to locate and access Mutable Data on the network.
    /// </summary>
    [PublicAPI]
    public struct MDataInfo
    {
        /// <summary>
        /// Name of the data where the directory is stored.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.XorNameLen)]
        public byte[] Name;

        /// <summary>
        /// Type tag of the data where directory is stored.
        /// </summary>
        public ulong TypeTag;

        /// <summary>
        /// Flag indicating whether the encryption info (`enc_key` and `enc_nonce`) is set.
        /// </summary>
        [MarshalAs(UnmanagedType.U1)]
        public bool HasEncInfo;

        /// <summary>
        /// Key to encrypt/decrypt the content.
        /// Meaningful only if `HasEncInfo` is `true`.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.SymKeyLen)]
        public byte[] EncKey;

        /// <summary>
        /// Nonce to be used for encryption keys.
        /// Meaningful only if `HasEncInfo` is `true`.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.SymNonceLen)]
        public byte[] EncNonce;

        /// <summary>
        /// Flag indicating whether the new encryption info is set.
        /// </summary>
        [MarshalAs(UnmanagedType.U1)]
        public bool HasNewEncInfo;

        /// <summary>
        /// New encryption key(used for two-phase reencryption).
        /// Meaningful only if `HasNewEncInfo` is `true`.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.SymKeyLen)]
        public byte[] NewEncKey;

        /// <summary>
        /// New encryption nonce (used for two-phase data reencryption).
        /// Meaningful only if `HasNewEncInfo` is `true`.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.SymNonceLen)]
        public byte[] NewEncNonce;
    }

    /// <summary>
    /// Represents a requested set of changes to permissions of a mutable data.
    /// </summary>
    [PublicAPI]
    public struct PermissionSet
    {
        /// <summary>
        /// Read permission.
        /// </summary>
        [MarshalAs(UnmanagedType.U1)]
        public bool Read;

        /// <summary>
        /// Insert permission.
        /// </summary>
        [MarshalAs(UnmanagedType.U1)]
        public bool Insert;

        /// <summary>
        /// Update permission.
        /// </summary>
        [MarshalAs(UnmanagedType.U1)]
        public bool Update;

        /// <summary>
        /// Delete permission.
        /// </summary>
        [MarshalAs(UnmanagedType.U1)]
        public bool Delete;

        /// <summary>
        /// Manage permissions permission.
        /// </summary>
        [MarshalAs(UnmanagedType.U1)]
        public bool ManagePermissions;
    }

    /// <summary>
    /// Represents an wrapper for authorisation request.
    /// </summary>
    [PublicAPI]
    public struct AuthReq
    {
        /// <summary>
        /// The application identifier for this request
        /// </summary>
        public AppExchangeInfo App;

        /// <summary>
        /// true if the app wants dedicated container for itself,
        /// false otherwise.
        /// </summary>
        public bool AppContainer;

        /// <summary>
        /// The list of containers it wishes to access (and desired permissions).
        /// </summary>
        public List<ContainerPermissions> Containers;

        /// <summary>
        /// Initialize new auth request object from native auth request.
        /// </summary>
        /// <param name="native"></param>
        internal AuthReq(AuthReqNative native)
        {
            App = native.App;
            AppContainer = native.AppContainer;
            Containers = BindingUtils.CopyToObjectList<ContainerPermissions>(native.ContainersPtr, (int)native.ContainersLen);
        }

        /// <summary>
        /// Returns native authentication request.
        /// </summary>
        /// <returns></returns>
        internal AuthReqNative ToNative()
        {
            return new AuthReqNative
            {
                App = App,
                AppContainer = AppContainer,
                ContainersPtr = BindingUtils.CopyFromObjectList(Containers),
                ContainersLen = (UIntPtr)(Containers?.Count ?? 0),
                ContainersCap = UIntPtr.Zero
            };
        }
    }

    /// <summary>
    /// Represents native authentication request.
    /// </summary>
    internal struct AuthReqNative
    {
        /// <summary>
        /// The application identifier for this request
        /// </summary>
        public AppExchangeInfo App;

        /// <summary>
        /// true if the app wants dedicated container for itself,
        /// false otherwise.
        /// </summary>
        [MarshalAs(UnmanagedType.U1)]
        public bool AppContainer;

        /// <summary>
        /// Pointer to containers.
        /// </summary>
        public IntPtr ContainersPtr;

        /// <summary>
        /// Length of containers.
        /// </summary>
        public UIntPtr ContainersLen;

        /// <summary>
        /// Internal field used by rust memory allocator.
        /// </summary>
        // ReSharper disable once NotAccessedField.Compiler
        public UIntPtr ContainersCap;

        /// <summary>
        /// Free the container pointer.
        /// </summary>
        internal void Free()
        {
            BindingUtils.FreeList(ref ContainersPtr, ref ContainersLen);
        }
    }

    /// <summary>
    /// Represents a containers request.
    /// </summary>
    [PublicAPI]
    public struct ContainersReq
    {
        /// <summary>
        /// App exchange info.
        /// </summary>
        public AppExchangeInfo App;

        /// <summary>
        /// Requested containers.
        /// </summary>
        public List<ContainerPermissions> Containers;

        /// <summary>
        /// Initialize a new container request object from native container request.
        /// </summary>
        /// <param name="native"></param>
        internal ContainersReq(ContainersReqNative native)
        {
            App = native.App;
            Containers = BindingUtils.CopyToObjectList<ContainerPermissions>(native.ContainersPtr, (int)native.ContainersLen);
        }

        /// <summary>
        /// Returns native container request.
        /// </summary>
        /// <returns></returns>
        internal ContainersReqNative ToNative()
        {
            return new ContainersReqNative
            {
                App = App,
                ContainersPtr = BindingUtils.CopyFromObjectList(Containers),
                ContainersLen = (UIntPtr)(Containers?.Count ?? 0),
                ContainersCap = UIntPtr.Zero
            };
        }
    }

    /// <summary>
    /// Represents native container request.
    /// </summary>
    internal struct ContainersReqNative
    {
        /// <summary>
        /// App exchange info.
        /// </summary>
        public AppExchangeInfo App;

        /// <summary>
        /// Pointer to containers.
        /// </summary>
        public IntPtr ContainersPtr;

        /// <summary>
        /// Length of containers.
        /// </summary>
        public UIntPtr ContainersLen;

        /// <summary>
        /// Internal field used by rust memory allocator.
        /// </summary>
        // ReSharper disable once NotAccessedField.Compiler
        public UIntPtr ContainersCap;

        /// <summary>
        /// Free the container pointer.
        /// </summary>
        internal void Free()
        {
            BindingUtils.FreeList(ref ContainersPtr, ref ContainersLen);
        }
    }

    /// <summary>
    /// Represents an application ID in the process of asking permissions.
    /// </summary>
    [PublicAPI]
    public struct AppExchangeInfo
    {
        /// <summary>
        /// Application identifier.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string Id;

        /// <summary>
        /// Application scope, null if not present.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string Scope;

        /// <summary>
        /// Application name.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string Name;

        /// <summary>
        /// Application provider/vendor (e.g. MaidSafe).
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string Vendor;
    }

    /// <summary>
    /// Represents the set of permissions for a given container.
    /// </summary>
    [PublicAPI]
    public struct ContainerPermissions
    {
        /// <summary>
        /// Container id.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string ContName;

        /// <summary>
        /// Requested permission set.
        /// </summary>
        public PermissionSet Access;
    }

    /// <summary>
    /// Represents a wrapper for native request to share mutable data.
    /// </summary>
    [PublicAPI]
    public struct ShareMDataReq
    {
        /// <summary>
        /// Info about the app requesting shared access.
        /// </summary>
        public AppExchangeInfo App;

        /// <summary>
        /// List of ShareMData (contains MD names,
        /// type tags and permissions that need to be shared).
        /// </summary>
        public List<ShareMData> MData;

        /// <summary>
        /// Initialize a new mutable data share request object from native request.
        /// </summary>
        /// <param name="native"></param>
        internal ShareMDataReq(ShareMDataReqNative native)
        {
            App = native.App;
            MData = BindingUtils.CopyToObjectList<ShareMData>(native.MDataPtr, (int)native.MDataLen);
        }

        /// <summary>
        /// Returns ShareMDataReqNative
        /// </summary>
        /// <returns></returns>
        internal ShareMDataReqNative ToNative()
        {
            return new ShareMDataReqNative
            {
                App = App,
                MDataPtr = BindingUtils.CopyFromObjectList(MData),
                MDataLen = (UIntPtr)(MData?.Count ?? 0),
                MDataCap = UIntPtr.Zero
            };
        }
    }

    /// <summary>
    /// Native request to share mutable data.
    /// </summary>
    internal struct ShareMDataReqNative
    {
        /// <summary>
        /// Info about the app requesting shared access.
        /// </summary>
        public AppExchangeInfo App;

        /// <summary>
        /// Pointer to mutable data
        /// </summary>
        public IntPtr MDataPtr;

        /// <summary>
        /// Length of MData array.
        /// </summary>
        public UIntPtr MDataLen;

        /// <summary>
        /// Capacity of MData vec.
        /// </summary>
        // ReSharper disable once NotAccessedField.Compiler
        public UIntPtr MDataCap;

        /// <summary>
        /// Free mutable data pointer.
        /// </summary>
        internal void Free()
        {
            BindingUtils.FreeList(ref MDataPtr, ref MDataLen);
        }
    }

    /// <summary>
    /// Represents a specific mutable data that is being shared.
    /// </summary>
    [PublicAPI]
    public struct ShareMData
    {
        /// <summary>
        /// The mutable data type.
        /// </summary>
        public ulong TypeTag;

        /// <summary>
        /// The mutable data name.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.XorNameLen)]
        public byte[] Name;

        /// <summary>
        /// The permissions being requested.
        /// </summary>
        public PermissionSet Perms;
    }

    /// <summary>
    /// Represents the wrapper for native authentication response.
    /// </summary>
    [PublicAPI]
    public struct AuthGranted
    {
        /// <summary>
        /// The access keys.
        /// </summary>
        public AppKeys AppKeys;

        /// <summary>
        /// Access container info.
        /// </summary>
        public AccessContInfo AccessContainerInfo;

        /// <summary>
        /// Access container entry.
        /// </summary>
        public AccessContainerEntry AccessContainerEntry;

        /// <summary>
        /// Crust's bootstrap config
        /// </summary>
        public List<byte> BootstrapConfig;

        /// <summary>
        /// Initialize a new auth response object from native auth response.
        /// </summary>
        /// <param name="native"></param>
        internal AuthGranted(AuthGrantedNative native)
        {
            AppKeys = native.AppKeys;
            AccessContainerInfo = native.AccessContainerInfo;
            AccessContainerEntry = new AccessContainerEntry(native.AccessContainerEntry);
            BootstrapConfig = BindingUtils.CopyToByteList(native.BootstrapConfigPtr, (int)native.BootstrapConfigLen);
        }

        /// <summary>
        /// Returns native authentication response (AuthGrantedNative).
        /// </summary>
        /// <returns></returns>
        internal AuthGrantedNative ToNative()
        {
            return new AuthGrantedNative
            {
                AppKeys = AppKeys,
                AccessContainerInfo = AccessContainerInfo,
                AccessContainerEntry = AccessContainerEntry.ToNative(),
                BootstrapConfigPtr = BindingUtils.CopyFromByteList(BootstrapConfig),
                BootstrapConfigLen = (UIntPtr)(BootstrapConfig?.Count ?? 0),
                BootstrapConfigCap = UIntPtr.Zero
            };
        }
    }

    /// <summary>
    /// Represents the authentication response.
    /// </summary>
    internal struct AuthGrantedNative
    {
        /// <summary>
        /// The access keys.
        /// </summary>
        public AppKeys AppKeys;

        /// <summary>
        /// Access container info.
        /// </summary>
        public AccessContInfo AccessContainerInfo;

        /// <summary>
        /// Access container entry.
        /// </summary>
        public AccessContainerEntryNative AccessContainerEntry;

        /// <summary>
        /// Crust's bootstrap config.
        /// </summary>
        public IntPtr BootstrapConfigPtr;

        /// <summary>
        /// `bootstrap_config`'s length.
        /// </summary>
        public UIntPtr BootstrapConfigLen;

        /// <summary>
        /// Used by Rust memory allocator.
        /// </summary>
        // ReSharper disable once NotAccessedField.Compiler
        public UIntPtr BootstrapConfigCap;

        /// <summary>
        /// Free bootstrap config pointer.
        /// </summary>
        internal void Free()
        {
            AccessContainerEntry.Free();
            BindingUtils.FreeList(ref BootstrapConfigPtr, ref BootstrapConfigLen);
        }
    }

    /// <summary>
    /// Represents the needed keys to work with the data.
    /// </summary>
    [PublicAPI]
    public struct AppKeys
    {
        /// <summary>
        /// Owner signing public key.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.SignPublicKeyLen)]
        public byte[] OwnerKey;

        /// <summary>
        /// Data symmetric encryption key.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.SymKeyLen)]
        public byte[] EncKey;

        /// <summary>
        /// Asymmetric sign public key.
        /// This is the identity of the App in the network.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.SignPublicKeyLen)]
        public byte[] SignPk;

        /// <summary>
        /// Asymmetric sign private key.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.SignSecretKeyLen)]
        public byte[] SignSk;

        /// <summary>
        /// Asymmetric encryption public key.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.AsymPublicKeyLen)]
        public byte[] EncPk;

        /// <summary>
        /// Asymmetric encryption private key.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.AsymSecretKeyLen)]
        public byte[] EncSk;
    }

    /// <summary>
    /// Represents container info.
    /// </summary>
    [PublicAPI]
    public struct AccessContInfo
    {
        /// <summary>
        /// Container Id.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.XorNameLen)]
        public byte[] Id;

        /// <summary>
        /// Type tag.
        /// </summary>
        public ulong Tag;

        /// <summary>
        /// Nonce
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.SymNonceLen)]
        public byte[] Nonce;
    }

    /// <summary>
    /// Represents access container entry for a single app.
    /// </summary>
    [PublicAPI]
    public struct AccessContainerEntry
    {
        /// <summary>
        /// List of containers (name, 'MDataInfo' and permissions).
        /// </summary>
        public List<ContainerInfo> Containers;

        /// <summary>
        /// Initialize a new access container entry object from native access container entry
        /// </summary>
        /// <param name="native"></param>
        internal AccessContainerEntry(AccessContainerEntryNative native)
        {
            Containers = BindingUtils.CopyToObjectList<ContainerInfo>(native.ContainersPtr, (int)native.ContainersLen);
        }

        /// <summary>
        /// Returns native access container entry.
        /// </summary>
        /// <returns></returns>
        internal AccessContainerEntryNative ToNative()
        {
            return new AccessContainerEntryNative
            {
                ContainersPtr = BindingUtils.CopyFromObjectList(Containers),
                ContainersLen = (UIntPtr)(Containers?.Count ?? 0),
                ContainersCap = UIntPtr.Zero
            };
        }
    }

    /// <summary>
    /// Represents native access container entry for a single app.
    /// </summary>
    internal struct AccessContainerEntryNative
    {
        /// <summary>
        /// Pointer to the array of 'ContainerInfo'.
        /// </summary>
        public IntPtr ContainersPtr;

        /// <summary>
        /// Size of the array.
        /// </summary>
        public UIntPtr ContainersLen;

        /// <summary>
        /// Internal field used by rust memory allocator.
        /// </summary>
        // ReSharper disable once NotAccessedField.Compiler
        public UIntPtr ContainersCap;

        /// <summary>
        /// Free the container pointer.
        /// </summary>
        internal void Free()
        {
            BindingUtils.FreeList(ref ContainersPtr, ref ContainersLen);
        }
    }

    /// <summary>
    /// Information about the container.
    /// </summary>
    [PublicAPI]
    public struct ContainerInfo
    {
        /// <summary>
        /// Container name
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string Name;

        /// <summary>
        /// Container's MDataInfo.
        /// </summary>
        public MDataInfo MDataInfo;

        /// <summary>
        /// App's permissions in the container.
        /// </summary>
        public PermissionSet Permissions;
    }

    /// <summary>
    /// Information about an application that has access to an MD through SignKey
    /// </summary>
    [PublicAPI]
    public struct AppAccess
    {
        /// <summary>
        /// App's or user's public key.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.SignPublicKeyLen)]
        public byte[] SignKey;

        /// <summary>
        /// A list of permissions.
        /// </summary>
        public PermissionSet Permissions;

        /// <summary>
        /// App's user-facing name.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string Name;

        /// <summary>
        /// App id.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string AppId;
    }

    /// <summary>
    /// User metadata for mutable data.
    /// </summary>
    [PublicAPI]
    public struct MetadataResponse
    {
        /// <summary>
        /// Name or purpose of mutable data.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string Name;

        /// <summary>
        /// Description of how this mutable data should or should not be shared.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string Description;

        /// <summary>
        /// Xor name of this struct's corresponding mutable data
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.XorNameLen)]
        public byte[] XorName;

        /// <summary>
        /// Type tag of this struct's corresponding mutable data.
        /// </summary>
        public ulong TypeTag;
    }

    /// <summary>
    /// Represents an FFI-Safe mutable data key.
    /// </summary>
    [PublicAPI]
    public struct MDataKey
    {
        /// <summary>
        /// Key value in byte array format.
        /// </summary>
        public List<byte> Val;

        /// <summary>
        /// Initialize new mutable data key from native key
        /// </summary>
        /// <param name="native"></param>
        internal MDataKey(MDataKeyNative native)
        {
            Val = BindingUtils.CopyToByteList(native.ValPtr, (int)native.ValLen);
        }

        /// <summary>
        /// Returns a native FFI-Safe mutable data key.
        /// </summary>
        /// <returns></returns>
        internal MDataKeyNative ToNative()
        {
            return new MDataKeyNative { ValPtr = BindingUtils.CopyFromByteList(Val), ValLen = (UIntPtr)(Val?.Count ?? 0) };
        }
    }

    /// <summary>
    /// Represents a native FFI-Safe mutable data key.
    /// </summary>
    internal struct MDataKeyNative
    {
        /// <summary>
        /// key value pointer.
        /// </summary>
        public IntPtr ValPtr;

        /// <summary>
        /// key length.
        /// </summary>
        public UIntPtr ValLen;

        /// <summary>
        /// Free the pointer to mutable data key.
        /// </summary>
        internal void Free()
        {
            BindingUtils.FreeList(ref ValPtr, ref ValLen);
        }
    }

    /// <summary>
    /// Represents the FFI-safe mutable data value.
    /// </summary>
    [PublicAPI]
    public struct MDataValue
    {
        /// <summary>
        /// Value content in byte list format.
        /// </summary>
        public List<byte> Content;

        /// <summary>
        /// Entry version.
        /// </summary>
        public ulong EntryVersion;

        /// <summary>
        /// Initialize new mutable data value
        /// </summary>
        /// <param name="native"></param>
        internal MDataValue(MDataValueNative native)
        {
            Content = BindingUtils.CopyToByteList(native.ContentPtr, (int)native.ContentLen);
            EntryVersion = native.EntryVersion;
        }

        /// <summary>
        /// Returns the native FFI-safe mutable data value.
        /// </summary>
        /// <returns></returns>
        internal MDataValueNative ToNative()
        {
            return new MDataValueNative
            {
                ContentPtr = BindingUtils.CopyFromByteList(Content),
                ContentLen = (UIntPtr)(Content?.Count ?? 0),
                EntryVersion = EntryVersion
            };
        }
    }

    /// <summary>
    /// Represents the native FFI-safe mutable data value.
    /// </summary>
    internal struct MDataValueNative
    {
        /// <summary>
        /// Content pointer.
        /// </summary>
        public IntPtr ContentPtr;

        /// <summary>
        /// Content length.
        /// </summary>
        public UIntPtr ContentLen;

        /// <summary>
        /// Entry version.
        /// </summary>
        public ulong EntryVersion;

        /// <summary>
        /// Free mutable data value (content) pointer.
        /// </summary>
        internal void Free()
        {
            BindingUtils.FreeList(ref ContentPtr, ref ContentLen);
        }
    }

    /// <summary>
    /// Represents the FFI-safe mutable data entry (key, value).
    /// </summary>
    [PublicAPI]
    public struct MDataEntry
    {
        /// <summary>
        /// Mutable data key.
        /// </summary>
        public MDataKey Key;

        /// <summary>
        /// Mutable data value.
        /// </summary>
        public MDataValue Value;

        /// <summary>
        /// Initialize new mutable data entry from native entry
        /// </summary>
        /// <param name="native"></param>
        internal MDataEntry(MDataEntryNative native)
        {
            Key = new MDataKey(native.Key);
            Value = new MDataValue(native.Value);
        }

        /// <summary>
        /// Returns native FFI-safe mutable data entry.
        /// </summary>
        /// <returns></returns>
        internal MDataEntryNative ToNative()
        {
            return new MDataEntryNative { Key = Key.ToNative(), Value = Value.ToNative() };
        }
    }

    /// <summary>
    /// Represents the native FFI-safe mutable data entry (key, value).
    /// </summary>
    internal struct MDataEntryNative
    {
        /// <summary>
        /// Native FFI-safe mutable data key.
        /// </summary>
        public MDataKeyNative Key;

        /// <summary>
        /// Native FFI-safe mutable data value.
        /// </summary>
        public MDataValueNative Value;

        /// <summary>
        /// Free the mutable data key and value pointers.
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        internal void Free()
        {
            Key.Free();
            Value.Free();
        }
    }

    /// <summary>
    /// Represents a wrapper for native file.
    /// </summary>
    [PublicAPI]
    public struct File
    {
        /// <summary>
        /// File size in bytes.
        /// </summary>
        public ulong Size;

        /// <summary>
        /// Creation time (seconds part).
        /// </summary>
        public long CreatedSec;

        /// <summary>
        /// Creation time (nanoseconds part).
        /// </summary>
        public uint CreatedNsec;

        /// <summary>
        /// Modification time (seconds part).
        /// </summary>
        public long ModifiedSec;

        /// <summary>
        /// Modification time (nanoseconds part).
        /// </summary>
        public uint ModifiedNsec;

        /// <summary>
        /// Pointer to user metadata.
        /// </summary>
        public List<byte> UserMetadata;

        /// <summary>
        /// Name of ImmutableData containing the content of the file.
        /// </summary>
        public byte[] DataMapName;

        /// <summary>
        /// Initialize a new file object from native file FFI-Wrapper.
        /// </summary>
        /// <param name="native"></param>
        internal File(FileNative native)
        {
            Size = native.Size;
            CreatedSec = native.CreatedSec;
            CreatedNsec = native.CreatedNsec;
            ModifiedSec = native.ModifiedSec;
            ModifiedNsec = native.ModifiedNsec;
            UserMetadata = BindingUtils.CopyToByteList(native.UserMetadataPtr, (int)native.UserMetadataLen);
            DataMapName = native.DataMapName;
        }

        /// <summary>
        /// Returns native FFI-Wrapper for File (FileNative).
        /// </summary>
        /// <returns></returns>
        internal FileNative ToNative()
        {
            return new FileNative
            {
                Size = Size,
                CreatedSec = CreatedSec,
                CreatedNsec = CreatedNsec,
                ModifiedSec = ModifiedSec,
                ModifiedNsec = ModifiedNsec,
                UserMetadataPtr = BindingUtils.CopyFromByteList(UserMetadata),
                UserMetadataLen = (UIntPtr)(UserMetadata?.Count ?? 0),
                UserMetadataCap = UIntPtr.Zero,
                DataMapName = DataMapName
            };
        }
    }

    /// <summary>
    /// Represents native FFI-wrapper for File.
    /// </summary>
    internal struct FileNative
    {
        /// <summary>
        /// File size in bytes.
        /// </summary>
        public ulong Size;

        /// <summary>
        /// Creation time (seconds part).
        /// </summary>
        public long CreatedSec;

        /// <summary>
        /// Creation time (nanoseconds part).
        /// </summary>
        public uint CreatedNsec;

        /// <summary>
        /// Modification time (seconds part).
        /// </summary>
        public long ModifiedSec;

        /// <summary>
        /// Modification time (nanoseconds part).
        /// </summary>
        public uint ModifiedNsec;

        /// <summary>
        /// Pointer to user metadata.
        /// </summary>
        public IntPtr UserMetadataPtr;

        /// <summary>
        /// Size of user metadata.
        /// </summary>
        public UIntPtr UserMetadataLen;

        /// <summary>
        /// Capacity of user metadata.
        /// </summary>
        // ReSharper disable once NotAccessedField.Compiler
        public UIntPtr UserMetadataCap;

        /// <summary>
        /// Name of ImmutableData containing the content of the file.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.XorNameLen)]
        public byte[] DataMapName;

        /// <summary>
        /// Free user metadata pointer.
        /// </summary>
        internal void Free()
        {
            BindingUtils.FreeList(ref UserMetadataPtr, ref UserMetadataLen);
        }
    }

    /// <summary>
    /// Represents the User Permission (key, PermissionSet).
    /// </summary>
    [PublicAPI]
    public struct UserPermissionSet
    {
        /// <summary>
        /// User's public signing key.
        /// </summary>
        public ulong UserH;

        /// <summary>
        /// User's permission set.
        /// </summary>
        public PermissionSet PermSet;
    }
}
