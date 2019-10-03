using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace SafeApp.Core
{
    /// <summary>
    /// Public and secret BLS key.
    /// </summary>
    public struct BlsKeyPair
    {
        /// <summary>
        /// Public key.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string PK;

        /// <summary>
        /// Secret key.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string SK;
    }

    [PublicAPI]
    public struct XorUrlEncoder
    {
        public ulong EncodingVersion;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.XorNameLen)]
        public byte[] Xorname;
        public ulong TypeTag;
        public ulong DataType;
        public ushort ContentType;
        [MarshalAs(UnmanagedType.LPStr)]
        public string Path;
        [MarshalAs(UnmanagedType.LPStr)]
        public string SubNames;
        public ulong ContentVersion;
    }

    public interface ISafeData
    {
    }

    [PublicAPI]
    public struct SafeKey : ISafeData
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.XorNameLen)]
        public byte[] Xorname;
        public NrsMapContainerInfo ResolvedFrom;
    }

    [PublicAPI]
    public struct Wallet : ISafeData
    {
        public byte[] Xorname;
        public ulong TypeTag;
        public WalletSpendableBalances Balances;
        public ulong DataType;
        public NrsMapContainerInfo ResolvedFrom;

        internal Wallet(WalletNative native)
        {
            Xorname = native.Xorname;
            TypeTag = native.TypeTag;
            Balances = new WalletSpendableBalances(native.Balances);
            DataType = native.DataType;
            ResolvedFrom = native.ResolvedFrom;
        }

        internal WalletNative ToNative()
        {
            return new WalletNative
            {
                Xorname = Xorname,
                TypeTag = TypeTag,
                Balances = Balances.ToNative(),
                DataType = DataType,
                ResolvedFrom = ResolvedFrom
            };
        }
    }

    internal struct WalletNative
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.XorNameLen)]
        public byte[] Xorname;
        public ulong TypeTag;
        public WalletSpendableBalancesNative Balances;
        public ulong DataType;
        public NrsMapContainerInfo ResolvedFrom;

        internal void Free()
        {
            Balances.Free();
        }
    }

    [PublicAPI]
    public struct FilesContainer : ISafeData
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.XorNameLen)]
        public byte[] Xorname;
        public ulong TypeTag;
        public ulong Version;
        [MarshalAs(UnmanagedType.LPStr)]
        public string FilesMap;
        public ulong DataType;
        public NrsMapContainerInfo ResolvedFrom;
    }

    [PublicAPI]
    public struct PublishedImmutableData : ISafeData
    {
        public byte[] Xorname;
        public byte[] Data;
        public NrsMapContainerInfo ResolvedFrom;
        public string MediaType;

        internal PublishedImmutableData(PublishedImmutableDataNative native)
        {
            Xorname = native.Xorname;
            Data = BindingUtils.CopyToByteArray(native.DataPtr, (int)native.DataLen);
            ResolvedFrom = native.ResolvedFrom;
            MediaType = native.MediaType;
        }

        internal PublishedImmutableDataNative ToNative()
        {
            return new PublishedImmutableDataNative
            {
                Xorname = Xorname,
                DataPtr = BindingUtils.CopyFromByteArray(Data),
                DataLen = (UIntPtr)(Data?.Length ?? 0),
                ResolvedFrom = ResolvedFrom,
                MediaType = MediaType
            };
        }
    }

    internal struct PublishedImmutableDataNative
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.XorNameLen)]
        public byte[] Xorname;
        public IntPtr DataPtr;
        public UIntPtr DataLen;
        public NrsMapContainerInfo ResolvedFrom;
        [MarshalAs(UnmanagedType.LPStr)]
        public string MediaType;

        internal void Free()
        {
            BindingUtils.FreeList(ref DataPtr, ref DataLen);
        }
    }

    [PublicAPI]
    public struct SafeDataFetchFailed : ISafeData
    {
        public readonly int Code;
        public readonly string Description;

        public SafeDataFetchFailed(int code, string description)
        {
            Code = code;
            Description = description;
        }
    }

    /// <summary>
    /// Wallet Spendable balance.
    /// </summary>"
    [PublicAPI]
    public struct WalletSpendableBalance
    {
        /// <summary>
        /// XOR Url
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string XorUrl;

        /// <summary>
        /// Secret Key
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string Sk;
    }

    /// <summary>
    /// Spendable Wallet balance.
    /// </summary>
    [PublicAPI]
    public struct WalletSpendableBalanceInfo
    {
        /// <summary>
        /// Wallet name.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string WalletName;

        /// <summary>
        /// Flag indicating whether the wallet is default.
        /// </summary>
        [MarshalAs(UnmanagedType.U1)]
        public bool IsDefault;

        /// <summary>
        /// Spendable wallet balance.
        /// </summary>
        public WalletSpendableBalance Balance;
    }

    /// <summary>
    /// Wallet spendable balances.
    /// </summary>
    [PublicAPI]
    public struct WalletSpendableBalances
    {
        /// <summary>
        /// List of spendable wallet balances.
        /// </summary>
        public List<WalletSpendableBalanceInfo> WalletBalances;

        /// <summary>
        /// Initialise a new wallet spendable balances object from native wallet spendable balances.
        /// </summary>
        /// <param name="native"></param>
        internal WalletSpendableBalances(WalletSpendableBalancesNative native)
        {
            WalletBalances = BindingUtils.CopyToObjectList<WalletSpendableBalanceInfo>(
                native.WalletBalancesPtr,
                (int)native.WalletBalancesLen);
        }

        /// <summary>
        /// Returns a native wallet spendable balance.
        /// </summary>
        /// <returns></returns>
        internal WalletSpendableBalancesNative ToNative()
        {
            return new WalletSpendableBalancesNative
            {
                WalletBalancesPtr = BindingUtils.CopyFromObjectList(WalletBalances),
                WalletBalancesLen = (UIntPtr)(WalletBalances?.Count ?? 0),
                WalletBalancesCap = UIntPtr.Zero
            };
        }
    }

    /// <summary>
    /// Represents native wallet spendable balances.
    /// </summary>
    internal struct WalletSpendableBalancesNative
    {
        /// <summary>
        /// Wallet spendable balances pointer.
        /// </summary>
        public IntPtr WalletBalancesPtr;

        /// <summary>
        /// Wallet balances length.
        /// </summary>
        public UIntPtr WalletBalancesLen;

        /// <summary>
        /// Wallet balances capacity.
        /// </summary>
        // ReSharper disable once NotAccessedField.Compiler
        public UIntPtr WalletBalancesCap;

        /// <summary>
        /// Free the wallet spendable balances pointer.
        /// </summary>
        internal void Free()
        {
            BindingUtils.FreeList(ref WalletBalancesPtr, ref WalletBalancesLen);
        }
    }

    /// <summary>
    /// Represents the processed file.
    /// </summary>
    [PublicAPI]
    public struct ProcessedFile
    {
        /// <summary>
        /// File name.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string FileName;

        /// <summary>
        /// File meta data.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string FileMetaData;

        /// <summary>
        /// File XorUrl.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string FileXorurl;
    }

    /// <summary>
    /// Processed files.
    /// </summary>
    [PublicAPI]
    public struct ProcessedFiles
    {
        /// <summary>
        /// Files.
        /// </summary>
        public List<ProcessedFile> Files;

        /// <summary>
        /// Initialises the processed file from native file info.
        /// </summary>
        /// <param name="native"></param>
        internal ProcessedFiles(ProcessedFilesNative native)
        {
            Files = BindingUtils.CopyToObjectList<ProcessedFile>(
                native.ProcessedFilesPtr,
                (int)native.ProcessedFilesLen);
        }

        /// <summary>
        /// Returns the native processed file.
        /// </summary>
        /// <returns></returns>
        internal ProcessedFilesNative ToNative()
        {
            return new ProcessedFilesNative
            {
                ProcessedFilesPtr = BindingUtils.CopyFromObjectList(Files),
                ProcessedFilesLen = (UIntPtr)(Files?.Count ?? 0),
                ProcessedFilesCap = UIntPtr.Zero
            };
        }
    }

    /// <summary>
    /// Represents the native processed file.
    /// </summary>
    internal struct ProcessedFilesNative
    {
        /// <summary>
        /// Processed files pointer.
        /// </summary>
        public IntPtr ProcessedFilesPtr;

        /// <summary>
        /// Processed files length.
        /// </summary>
        public UIntPtr ProcessedFilesLen;

        /// <summary>
        /// Processed files capacity.
        /// </summary>
        // ReSharper disable once NotAccessedField.Compiler
        public UIntPtr ProcessedFilesCap;

        /// <summary>
        /// Free the processed file pointer.
        /// </summary>
        internal void Free()
        {
            BindingUtils.FreeList(ref ProcessedFilesPtr, ref ProcessedFilesLen);
        }
    }

    [PublicAPI]
    public struct NrsMapContainerInfo
    {
        [MarshalAs(UnmanagedType.LPStr)]
        public string PublicName;
        [MarshalAs(UnmanagedType.LPStr)]
        public string Xorurl;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.XorNameLen)]
        public byte[] Xorname;
        public ulong TypeTag;
        public ulong Version;
        [MarshalAs(UnmanagedType.LPStr)]
        public string NrsMap;
        public ulong DataType;
    }
}
