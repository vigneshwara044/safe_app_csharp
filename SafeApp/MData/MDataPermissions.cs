using System.Threading.Tasks;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp.MData {
  public static class MDataPermissions {
    private static readonly IAppBindings AppBindings = AppResolver.Current;

    public static Task FreeAsync(ulong permissionsH) {
      var tcs = new TaskCompletionSource<object>();
      MDataPermissionsFreeCb callback = (_, result) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(null);
      };

      AppBindings.MDataPermissionsFree(Session.AppPtr, permissionsH, callback);

      return tcs.Task;
    }

    public static Task InsertAsync(NativeHandle permissionsH, NativeHandle forUserH, NativeHandle permissionSetH) {
      var tcs = new TaskCompletionSource<object>();

      MDataPermissionsInsertCb callback = (_, result) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(null);
      };

      AppBindings.MDataPermissionsInsert(Session.AppPtr, permissionsH, forUserH, permissionSetH, callback);

      return tcs.Task;
    }

    public static Task<NativeHandle> NewAsync() {
      var tcs = new TaskCompletionSource<NativeHandle>();

      MDataPermissionsNewCb callback = (_, result, permissionsH) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(new NativeHandle(permissionsH, FreeAsync));
      };

      AppBindings.MDataPermissionsNew(Session.AppPtr, callback);

      return tcs.Task;
    }
  }
}
