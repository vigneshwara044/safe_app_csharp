using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace SafeApp.Core
{
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
        [MarshalAs(UnmanagedType.LPStr)]
        public string ResolvedFrom;
    }

    [PublicAPI]
    public struct Wallet : ISafeData
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.XorNameLen)]
        public byte[] Xorname;
        public ulong TypeTag;
        [MarshalAs(UnmanagedType.LPStr)]
        public string Balances;
        public ulong DataType;
        [MarshalAs(UnmanagedType.LPStr)]
        public string ResolvedFrom;
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
        [MarshalAs(UnmanagedType.LPStr)]
        public string ResolvedFrom;
    }

    [PublicAPI]
    public struct PublishedImmutableData : ISafeData
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.XorNameLen)]
        public byte[] Xorname;
        public byte[] Data;
        public string ResolvedFrom;
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
        public byte[] Xorname;
        public IntPtr DataPtr;
        public UIntPtr DataLen;
        [MarshalAs(UnmanagedType.LPStr)]
        public string ResolvedFrom;
        [MarshalAs(UnmanagedType.LPStr)]
        public string MediaType;

        internal void Free()
        {
            BindingUtils.FreeList(DataPtr, DataLen);
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
        public string Sk;
    }

    /// <summary>
    /// Spendable Wallet balance.
    /// </summary>
    [PublicAPI]
    public struct SpendableWalletBalance
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
        public List<SpendableWalletBalance> WalletBalances;

        /// <summary>
        /// Initialise a new wallet spendable balances object from native wallet spendable balances.
        /// </summary>
        /// <param name="native"></param>
        internal WalletSpendableBalances(WalletSpendableBalancesNative native)
        {
            WalletBalances = BindingUtils.CopyToObjectList<SpendableWalletBalance>(
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
            BindingUtils.FreeList(WalletBalancesPtr, WalletBalancesLen);
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
    /// ProcessedFiles
    /// </summary>
    [PublicAPI]
    public struct ProcessedFiles
    {
        /// <summary>
        /// 
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
        /// Processed Files length.
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
            BindingUtils.FreeList(ProcessedFilesPtr, ProcessedFilesLen);
        }
    }

    /// <summary>
    /// Represents the file item.
    /// </summary>
    [PublicAPI]
    public struct FileItem
    {
        /// <summary>
        /// Meta data of the file.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string FileMetaData;

        /// <summary>
        /// The XorUrl of tthe file.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string Xorurl;
    }

    /// <summary>
    /// Represents the file info.
    /// </summary>
    [PublicAPI]
    public struct FileInfo
    {
        /// <summary>
        /// File name.
        /// </summary>
        public string FileName;

        /// <summary>
        /// List of file items.
        /// </summary>
        public List<FileItem> FileItems;

        /// <summary>
        /// Initialises the file info from native file info.
        /// </summary>
        /// <param name="native"></param>
        internal FileInfo(FileInfoNative native)
        {
            FileName = native.FileName;
            FileItems = BindingUtils.CopyToObjectList<FileItem>(native.FileItemsPtr, (int)native.FileItemsLen);
        }

        /// <summary>
        /// Returns the native file info.
        /// </summary>
        /// <returns></returns>
        internal FileInfoNative ToNative()
        {
            return new FileInfoNative
            {
                FileName = FileName,
                FileItemsPtr = BindingUtils.CopyFromObjectList(FileItems),
                FileItemsLen = (UIntPtr)(FileItems?.Count ?? 0),
                FileItemsCap = UIntPtr.Zero
            };
        }
    }

    /// <summary>
    /// Represents the native file info.
    /// </summary>
    internal struct FileInfoNative
    {
        /// <summary>
        /// File name.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string FileName;

        /// <summary>
        /// Files items pointer.
        /// </summary>
        public IntPtr FileItemsPtr;

        /// <summary>
        /// Files item length.
        /// </summary>
        public UIntPtr FileItemsLen;

        /// <summary>
        /// File items capacity.
        /// </summary>
        // ReSharper disable once NotAccessedField.Compiler
        public UIntPtr FileItemsCap;

        /// <summary>
        /// Free the file info pointer.
        /// </summary>
        internal void Free()
        {
            BindingUtils.FreeList(FileItemsPtr, FileItemsLen);
        }
    }

    /// <summary>
    /// Represents the files map.
    /// </summary>
    [PublicAPI]
    public struct FilesMap
    {
        /// <summary>
        /// List of file info.
        /// </summary>
        public List<FileInfo> FileItems;

        /// <summary>
        /// Initialise the new FilesMap.
        /// </summary>
        /// <param name="native"></param>
        internal FilesMap(FilesMapNative native)
        {
            FileItems = BindingUtils.CopyToObjectList<FileInfo>(native.FileItemsPtr, (int)native.FileItemsLen);
        }

        /// <summary>
        /// Returns the native files map.
        /// </summary>
        /// <returns></returns>
        internal FilesMapNative ToNative()
        {
            return new FilesMapNative
            {
                FileItemsPtr = BindingUtils.CopyFromObjectList(FileItems),
                FileItemsLen = (UIntPtr)(FileItems?.Count ?? 0),
                FileItemsCap = UIntPtr.Zero
            };
        }
    }

    /// <summary>
    /// Represents the native files map.
    /// </summary>
    internal struct FilesMapNative
    {
        /// <summary>
        /// Files items pointer.
        /// </summary>
        public IntPtr FileItemsPtr;

        /// <summary>
        /// Files item length.
        /// </summary>
        public UIntPtr FileItemsLen;

        /// <summary>
        /// File items capacity.
        /// </summary>
        // ReSharper disable once NotAccessedField.Compiler
        public UIntPtr FileItemsCap;

        /// <summary>
        /// Free file items pointer.
        /// </summary>
        internal void Free()
        {
            BindingUtils.FreeList(FileItemsPtr, FileItemsLen);
        }
    }
}
