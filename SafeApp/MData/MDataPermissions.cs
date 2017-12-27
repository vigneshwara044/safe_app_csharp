using System.Threading.Tasks;
using JetBrains.Annotations;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp.MData {
  [PublicAPI]
  public static class MDataPermissions {
    private static readonly IAppBindings AppBindings = AppResolver.Current;

    public static Task FreeAsync(ulong permissionsH) {
      return AppBindings.MDataPermissionsFreeAsync(Session.AppPtr, permissionsH);
    }

    public static Task InsertAsync(NativeHandle permissionsH, NativeHandle forUserH, ref PermissionSet permissionSet) {
      return AppBindings.MDataPermissionsInsertAsync(Session.AppPtr, permissionsH, forUserH, ref permissionSet);
    }

    public static async Task<NativeHandle> NewAsync() {
      var permissionsH = await AppBindings.MDataPermissionsNewAsync(Session.AppPtr);
      return new NativeHandle(permissionsH, FreeAsync);
    }
  }
}
