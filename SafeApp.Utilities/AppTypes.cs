using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

[assembly: InternalsVisibleTo("SafeApp.AppBindings")]

namespace SafeApp.Utilities
{
  [PublicAPI]
  public enum MDataAction {
    Insert,
    Update,
    Delete,
    ManagePermissions,
  }

  [PublicAPI]
  public struct AccountInfo {
    public ulong MutationsDone;
    public ulong MutationsAvailable;
  }

  [PublicAPI]
  public struct MDataInfo {
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int) AppConstants.XorNameLen)]
    public byte[] Name;
    public ulong TypeTag;
    [MarshalAs(UnmanagedType.U1)]
    public bool HasEncInfo;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int) AppConstants.SymKeyLen)]
    public byte[] EncKey;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int) AppConstants.SymNonceLen)]
    public byte[] EncNonce;
    [MarshalAs(UnmanagedType.U1)]
    public bool HasNewEncInfo;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int) AppConstants.SymKeyLen)]
    public byte[] NewEncKey;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int) AppConstants.SymNonceLen)]
    public byte[] NewEncNonce;
  }

  [PublicAPI]
  public struct PermissionSet {
    [MarshalAs(UnmanagedType.U1)]
    public bool Read;
    [MarshalAs(UnmanagedType.U1)]
    public bool Insert;
    [MarshalAs(UnmanagedType.U1)]
    public bool Update;
    [MarshalAs(UnmanagedType.U1)]
    public bool Delete;
    [MarshalAs(UnmanagedType.U1)]
    public bool ManagePermissions;
  }

  [PublicAPI]
  public struct AuthReq {
    public AppExchangeInfo App;
    public bool AppContainer;
    public List<ContainerPermissions> Containers;

    internal AuthReq(AuthReqNative native) {
      App = native.App;
      AppContainer = native.AppContainer;
      Containers = BindingUtils.CopyToObjectList<ContainerPermissions>(native.ContainersPtr, native.ContainersLen);
    }

    internal AuthReqNative ToNative() {
      return new AuthReqNative() {
        App = App,
        AppContainer = AppContainer,
        ContainersPtr = BindingUtils.CopyFromObjectList(Containers),
        ContainersLen = (IntPtr) Containers.Count,
        ContainersCap = (IntPtr) 0
      };
    }
  }

  internal struct AuthReqNative {
    public AppExchangeInfo App;
    [MarshalAs(UnmanagedType.U1)]
    public bool AppContainer;
    public IntPtr ContainersPtr;
    public IntPtr ContainersLen;
    public IntPtr ContainersCap;

    internal void Free() {
      BindingUtils.FreeList(ref ContainersPtr, ref ContainersLen);
    }
  }

  [PublicAPI]
  public struct ContainersReq {
    public AppExchangeInfo App;
    public List<ContainerPermissions> Containers;

    internal ContainersReq(ContainersReqNative native) {
      App = native.App;
      Containers = BindingUtils.CopyToObjectList<ContainerPermissions>(native.ContainersPtr, native.ContainersLen);
    }

    internal ContainersReqNative ToNative() {
      return new ContainersReqNative() {
        App = App,
        ContainersPtr = BindingUtils.CopyFromObjectList(Containers),
        ContainersLen = (IntPtr) Containers.Count,
        ContainersCap = (IntPtr) 0
      };
    }
  }

  internal struct ContainersReqNative {
    public AppExchangeInfo App;
    public IntPtr ContainersPtr;
    public IntPtr ContainersLen;
    public IntPtr ContainersCap;

    internal void Free() {
      BindingUtils.FreeList(ref ContainersPtr, ref ContainersLen);
    }
  }

  [PublicAPI]
  public struct AppExchangeInfo {
    [MarshalAs(UnmanagedType.LPStr)]
    public string Id;
    [MarshalAs(UnmanagedType.LPStr)]
    public string Scope;
    [MarshalAs(UnmanagedType.LPStr)]
    public string Name;
    [MarshalAs(UnmanagedType.LPStr)]
    public string Vendor;
  }

  [PublicAPI]
  public struct ContainerPermissions {
    [MarshalAs(UnmanagedType.LPStr)]
    public string ContName;
    public PermissionSet Access;
  }

  [PublicAPI]
  public struct ShareMDataReq {
    public AppExchangeInfo App;
    public List<ShareMData> Mdata;

    internal ShareMDataReq(ShareMDataReqNative native) {
      App = native.App;
      Mdata = BindingUtils.CopyToObjectList<ShareMData>(native.MdataPtr, native.MdataLen);
    }

    internal ShareMDataReqNative ToNative() {
      return new ShareMDataReqNative() {
        App = App,
        MdataPtr = BindingUtils.CopyFromObjectList(Mdata),
        MdataLen = (IntPtr) Mdata.Count,
        MdataCap = (IntPtr) 0
      };
    }
  }

  internal struct ShareMDataReqNative {
    public AppExchangeInfo App;
    public IntPtr MdataPtr;
    public IntPtr MdataLen;
    public IntPtr MdataCap;

    internal void Free() {
      BindingUtils.FreeList(ref MdataPtr, ref MdataLen);
    }
  }

  [PublicAPI]
  public struct ShareMData {
    public ulong TypeTag;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int) AppConstants.XorNameLen)]
    public byte[] Name;
    public PermissionSet Perms;
  }

  [PublicAPI]
  public struct AuthGranted {
    public AppKeys AppKeys;
    public AccessContInfo AccessContainerInfo;
    public AccessContainerEntry AccessContainerEntry;
    public List<byte> BootstrapConfig;

    internal AuthGranted(AuthGrantedNative native) {
      AppKeys = native.AppKeys;
      AccessContainerInfo = native.AccessContainerInfo;
      AccessContainerEntry = native.AccessContainerEntry;
      BootstrapConfig = BindingUtils.CopyToByteList(native.BootstrapConfigPtr, native.BootstrapConfigLen);
    }

    internal AuthGrantedNative ToNative() {
      return new AuthGrantedNative() {
        AppKeys = AppKeys,
        AccessContainerInfo = AccessContainerInfo,
        AccessContainerEntry = AccessContainerEntry,
        BootstrapConfigPtr = BindingUtils.CopyFromByteList(BootstrapConfig),
        BootstrapConfigLen = (IntPtr) BootstrapConfig.Count,
        BootstrapConfigCap = (IntPtr) 0
      };
    }
  }

  internal struct AuthGrantedNative {
    public AppKeys AppKeys;
    public AccessContInfo AccessContainerInfo;
    public AccessContainerEntry AccessContainerEntry;
    public IntPtr BootstrapConfigPtr;
    public IntPtr BootstrapConfigLen;
    public IntPtr BootstrapConfigCap;

    internal void Free() {
      BindingUtils.FreeList(ref BootstrapConfigPtr, ref BootstrapConfigLen);
    }
  }

  [PublicAPI]
  public struct AppKeys {
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int) AppConstants.SignPublicKeyLen)]
    public byte[] OwnerKey;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int) AppConstants.SymKeyLen)]
    public byte[] EncKey;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int) AppConstants.SignPublicKeyLen)]
    public byte[] SignPk;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int) AppConstants.SignSecretKeyLen)]
    public byte[] SignSk;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int) AppConstants.AsymPublicKeyLen)]
    public byte[] EncPk;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int) AppConstants.AsymSecretKeyLen)]
    public byte[] EncSk;
  }

  [PublicAPI]
  public struct AccessContInfo {
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int) AppConstants.XorNameLen)]
    public byte[] Id;
    public ulong Tag;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int) AppConstants.SymNonceLen)]
    public byte[] Nonce;
  }

  [PublicAPI]
  public struct AccessContainerEntry {
    public IntPtr Ptr;
    public IntPtr Len;
    public IntPtr Cap;
  }

  [PublicAPI]
  public struct ContainerInfo {
    [MarshalAs(UnmanagedType.LPStr)]
    public string Name;
    public MDataInfo MdataInfo;
    public PermissionSet Permissions;
  }

  [PublicAPI]
  public struct AppAccess {
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int) AppConstants.SignPublicKeyLen)]
    public byte[] SignKey;
    public PermissionSet Permissions;
    [MarshalAs(UnmanagedType.LPStr)]
    public string Name;
    [MarshalAs(UnmanagedType.LPStr)]
    public string AppId;
  }

  [PublicAPI]
  public struct MetadataResponse {
    [MarshalAs(UnmanagedType.LPStr)]
    public string Name;
    [MarshalAs(UnmanagedType.LPStr)]
    public string Description;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int) AppConstants.XorNameLen)]
    public byte[] XorName;
    public ulong TypeTag;
  }

  [PublicAPI]
  public struct MDataValue {
    public List<byte> Content;
    public ulong EntryVersion;

    internal MDataValue(MDataValueNative native) {
      Content = BindingUtils.CopyToByteList(native.ContentPtr, native.ContentLen);
      EntryVersion = native.EntryVersion;
    }

    internal MDataValueNative ToNative() {
      return new MDataValueNative() {
        ContentPtr = BindingUtils.CopyFromByteList(Content),
        ContentLen = (IntPtr) Content.Count,
        EntryVersion = EntryVersion
      };
    }
  }

  internal struct MDataValueNative {
    public IntPtr ContentPtr;
    public IntPtr ContentLen;
    public ulong EntryVersion;

    internal void Free() {
      BindingUtils.FreeList(ref ContentPtr, ref ContentLen);
    }
  }

  [PublicAPI]
  public struct MDataKey {
    public List<byte> Val;

    internal MDataKey(MDataKeyNative native) {
      Val = BindingUtils.CopyToByteList(native.ValPtr, native.ValLen);
    }

    internal MDataKeyNative ToNative() {
      return new MDataKeyNative() {
        ValPtr = BindingUtils.CopyFromByteList(Val),
        ValLen = (IntPtr) Val.Count
      };
    }
  }

  internal struct MDataKeyNative {
    public IntPtr ValPtr;
    public IntPtr ValLen;

    internal void Free() {
      BindingUtils.FreeList(ref ValPtr, ref ValLen);
    }
  }

  [PublicAPI]
  public struct File {
    public ulong Size;
    public long CreatedSec;
    public uint CreatedNsec;
    public long ModifiedSec;
    public uint ModifiedNsec;
    public List<byte> UserMetadata;
    public byte[] DataMapName;

    internal File(FileNative native) {
      Size = native.Size;
      CreatedSec = native.CreatedSec;
      CreatedNsec = native.CreatedNsec;
      ModifiedSec = native.ModifiedSec;
      ModifiedNsec = native.ModifiedNsec;
      UserMetadata = BindingUtils.CopyToByteList(native.UserMetadataPtr, native.UserMetadataLen);
      DataMapName = native.DataMapName;
    }

    internal FileNative ToNative() {
      return new FileNative() {
        Size = Size,
        CreatedSec = CreatedSec,
        CreatedNsec = CreatedNsec,
        ModifiedSec = ModifiedSec,
        ModifiedNsec = ModifiedNsec,
        UserMetadataPtr = BindingUtils.CopyFromByteList(UserMetadata),
        UserMetadataLen = (IntPtr) UserMetadata.Count,
        UserMetadataCap = (IntPtr) 0,
        DataMapName = DataMapName
      };
    }
  }

  internal struct FileNative {
    public ulong Size;
    public long CreatedSec;
    public uint CreatedNsec;
    public long ModifiedSec;
    public uint ModifiedNsec;
    public IntPtr UserMetadataPtr;
    public IntPtr UserMetadataLen;
    public IntPtr UserMetadataCap;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int) AppConstants.XorNameLen)]
    public byte[] DataMapName;

    internal void Free() {
      BindingUtils.FreeList(ref UserMetadataPtr, ref UserMetadataLen);
    }
  }

  [PublicAPI]
  public struct UserPermissionSet {
    public ulong UserH;
    public PermissionSet PermSet;
  }

}
