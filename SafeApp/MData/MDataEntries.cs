using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp.MData {
  [PublicAPI]
  public class MDataEntries {
    private readonly IAppBindings _appBindings = AppResolver.Current;
    private IntPtr _appPtr;

    public MDataEntries(IntPtr appPtr) {
      _appPtr = appPtr;
    }

    public Task FreeAsync(ulong entriesH) {
      return _appBindings.MDataEntriesFreeAsync(_appPtr, entriesH);
    }

    public Task InsertAsync(NativeHandle entriesH, List<byte> entKey, List<byte> entVal) {
      return _appBindings.MDataEntriesInsertAsync(_appPtr, entriesH, entKey.ToArray(), entVal.ToArray());
    }

    public Task<ulong> LenAsync(NativeHandle entriesHandle) {
      // TODO needs fix
      throw new NotImplementedException();
      // return _appBindings.MDataEntriesLenAsync(_appPtr, entriesHandle);
    }

    public Task<(List<byte>, ulong)> GetAsync(NativeHandle entriesHandle, List<byte> key)
    {
      // TODO needs fix
       throw new NotImplementedException();
       // return _appBindings.MDataEntriesGetAsync(_appPtr, entriesHandle, key.ToArray());
    }

//    public Task<(List<byte>, ulong)> ForEachAsync(NativeHandle entriesHandle)
//    {
      // TODO needs fix
//      throw new NotImplementedException();
//    }

    public async Task<NativeHandle> NewAsync() {
      var entriesH = await _appBindings.MDataEntriesNewAsync(_appPtr);
      return new NativeHandle(entriesH, FreeAsync);
    }
  }
}
