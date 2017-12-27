using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp.MData {
  [PublicAPI]
  public static class MData {
    private static readonly IAppBindings AppBindings = AppResolver.Current;

    public static async Task<(List<byte>, ulong)> GetValueAsync(MDataInfo mDataInfo, List<byte> key) {
      var (dataArray, entryVersion) = await AppBindings.MDataGetValueAsync(Session.AppPtr, ref mDataInfo, key.ToArray());
      return (new List<byte>(dataArray), entryVersion);
    }

    public static async Task<NativeHandle> ListEntriesAsync(MDataInfo mDataInfo) {
      var mDataEntriesHandle = await AppBindings.MDataListEntriesAsync(Session.AppPtr, ref mDataInfo);
      return new NativeHandle(mDataEntriesHandle, MDataEntries.FreeAsync);
    }

    public static Task<List<MDataKey>> ListKeysAsync(MDataInfo mDataInfo) {
      // TODO: Needs fixed
      throw new NotImplementedException();

      /*var mDataEntKeysH = await AppBindings.MDataListKeysAsync(Session.AppPtr, ref mDataInfo);
      return new NativeHandle(mDataEntKeysH, MDataKeys.FreeAsync);*/
    }

    public static Task MutateEntriesAsync(ref MDataInfo mDataInfo, NativeHandle entryActionsH) {
      return AppBindings.MDataMutateEntriesAsync(Session.AppPtr, ref mDataInfo, entryActionsH);
    }

    public static Task PutAsync(ref MDataInfo mDataInfo, NativeHandle permissionsH, NativeHandle entriesH) {
      return AppBindings.MDataPutAsync(Session.AppPtr, ref mDataInfo, permissionsH, entriesH);
    }
  }
}
