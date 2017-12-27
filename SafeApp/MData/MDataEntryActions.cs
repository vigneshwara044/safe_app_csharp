using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp.MData {
  [PublicAPI]
  public static class MDataEntryActions {
    private static readonly IAppBindings AppBindings = AppResolver.Current;

    public static Task FreeAsync(ulong entryActionsH) {
      return AppBindings.MDataEntryActionsFreeAsync(Session.AppPtr, entryActionsH);
    }

    public static Task InsertAsync(NativeHandle entryActionsH, List<byte> entKey, List<byte> entVal) {
      return AppBindings.MDataEntryActionsInsertAsync(Session.AppPtr, entryActionsH, entKey.ToArray(), entVal.ToArray());
    }

    public static async Task<NativeHandle> NewAsync() {
      var entryActionsH = await AppBindings.MDataEntryActionsNewAsync(Session.AppPtr);
      return new NativeHandle(entryActionsH, FreeAsync);
    }
  }
}
