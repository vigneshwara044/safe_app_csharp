using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp.MData {
  [PublicAPI]
  public class MDataEntryActions {
    private readonly IAppBindings _appBindings = AppResolver.Current;
    private IntPtr _appPtr;

    public MDataEntryActions(IntPtr appPtr) {
      _appPtr = appPtr;
    }

    public Task FreeAsync(ulong entryActionsH) {
      return _appBindings.MDataEntryActionsFreeAsync(_appPtr, entryActionsH);
    }

    public Task InsertAsync(NativeHandle entryActionsH, List<byte> entKey, List<byte> entVal) {
      return _appBindings.MDataEntryActionsInsertAsync(_appPtr, entryActionsH, entKey.ToArray(), entVal.ToArray());
    }

    public async Task<NativeHandle> NewAsync() {
      var entryActionsH = await _appBindings.MDataEntryActionsNewAsync(_appPtr);
      return new NativeHandle(entryActionsH, FreeAsync);
    }
  }
}
