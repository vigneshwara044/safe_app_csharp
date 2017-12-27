using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp.MData {
  [PublicAPI]
  public class MData {
    private readonly IAppBindings _appBindings = AppResolver.Current;
    private IntPtr _appPtr;

    public MData(IntPtr appPtr) {
      _appPtr = appPtr;
    }

    public async Task<(List<byte>, ulong)> GetValueAsync(MDataInfo mDataInfo, List<byte> key) {
      var (dataArray, entryVersion) = await _appBindings.MDataGetValueAsync(_appPtr, ref mDataInfo, key.ToArray());
      return (new List<byte>(dataArray), entryVersion);
    }

    public async Task<NativeHandle> ListEntriesAsync(MDataInfo mDataInfo) {
      var mDataEntriesHandle = await _appBindings.MDataListEntriesAsync(_appPtr, ref mDataInfo);
      return new NativeHandle(mDataEntriesHandle, MDataEntries.FreeAsync);
    }

    public Task<List<MDataKey>> ListKeysAsync(MDataInfo mDataInfo) {
      // TODO: Needs fixed
      throw new NotImplementedException();

      /*var mDataEntKeysH = await _appBindings.MDataListKeysAsync(_appPtr, ref mDataInfo);
      return new NativeHandle(mDataEntKeysH, MDataKeys.FreeAsync);*/
    }

    public Task MutateEntriesAsync(ref MDataInfo mDataInfo, NativeHandle entryActionsH) {
      return _appBindings.MDataMutateEntriesAsync(_appPtr, ref mDataInfo, entryActionsH);
    }

    public Task PutAsync(ref MDataInfo mDataInfo, NativeHandle permissionsH, NativeHandle entriesH) {
      return _appBindings.MDataPutAsync(_appPtr, ref mDataInfo, permissionsH, entriesH);
    }
  }
}
