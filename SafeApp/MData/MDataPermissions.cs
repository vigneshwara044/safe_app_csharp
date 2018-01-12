using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp.MData {
  [PublicAPI]
  public class MDataPermissions {
    private static readonly IAppBindings AppBindings = AppResolver.Current;
    private SafeAppPtr _appPtr;

    internal MDataPermissions(SafeAppPtr appPtr) {
      _appPtr = appPtr;
    }

    private Task FreeAsync(ulong permissionsH) {
      return Equals(_appPtr.Value, IntPtr.Zero) ?
        Task.FromResult<object>(null) :
        AppBindings.MDataPermissionsFreeAsync(_appPtr, permissionsH);
    }

    public Task<PermissionSet> GetAsync(NativeHandle permissionsHandle, NativeHandle userPubSignKey) {
      return AppBindings.MDataPermissionsGetAsync(_appPtr, permissionsHandle, userPubSignKey);
    }

    public Task InsertAsync(NativeHandle permissionsH, NativeHandle forUserH, ref PermissionSet permissionSet) {
      return AppBindings.MDataPermissionsInsertAsync(_appPtr, permissionsH, forUserH, ref permissionSet);
    }

    public Task<ulong> LenAsync(NativeHandle permissionsHandle) {
      return AppBindings.MDataPermissionsLenAsync(_appPtr, permissionsHandle);
    }

    public Task<List<UserPermissionSet>> ListAsync(NativeHandle permissionHandle) {
      return AppBindings.MDataListPermissionSetsAsync(_appPtr, permissionHandle);
    }

    public async Task<NativeHandle> NewAsync() {
      var permissionsH = await AppBindings.MDataPermissionsNewAsync(_appPtr);
      return new NativeHandle(_appPtr, permissionsH, FreeAsync);
    }
  }
}
