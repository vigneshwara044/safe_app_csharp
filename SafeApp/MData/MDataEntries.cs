using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp.MData {
  [PublicAPI]
  public static class MDataEntries {
    private static readonly IAppBindings AppBindings = AppResolver.Current;

    public static Task FreeAsync(ulong entriesH) {
      return AppBindings.MDataEntriesFreeAsync(Session.AppPtr, entriesH);
    }

    public static Task InsertAsync(NativeHandle entriesH, List<byte> entKey, List<byte> entVal) {
      return AppBindings.MDataEntriesInsertAsync(Session.AppPtr, entriesH, entKey.ToArray(), entVal.ToArray());
    }

    public static Task<IntPtr> LenAsync(NativeHandle entriesHandle) {
      return AppBindings.MDataEntriesLenAsync(Session.AppPtr, entriesHandle);
    }

    public static async Task<NativeHandle> NewAsync() {
      var entriesH = await AppBindings.MDataEntriesNewAsync(Session.AppPtr);
      return new NativeHandle(entriesH, FreeAsync);
    }
  }
}
