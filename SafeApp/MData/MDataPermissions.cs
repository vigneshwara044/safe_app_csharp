using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp.MData {
  [PublicAPI]
  public class MDataPermissions {
    private readonly IAppBindings _appBindings = AppResolver.Current;
    private IntPtr _appPtr;

    public MDataPermissions(IntPtr appPtr) {
      _appPtr = appPtr;
    }

    public Task FreeAsync(ulong permissionsH) {
      return _appBindings.MDataPermissionsFreeAsync(_appPtr, permissionsH);
    }

    public Task InsertAsync(NativeHandle permissionsH, NativeHandle forUserH, ref PermissionSet permissionSet) {
      return _appBindings.MDataPermissionsInsertAsync(_appPtr, permissionsH, forUserH, ref permissionSet);
    }

    public async Task<NativeHandle> NewAsync() {
      var permissionsH = await _appBindings.MDataPermissionsNewAsync(_appPtr);
      return new NativeHandle(permissionsH, FreeAsync);
    }
  }
}
