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

    public Task<ulong> GetVersionAsync(ref MDataInfo mDataInfo) {
      return _appBindings.MDataGetVersionAsync(_appPtr, ref mDataInfo);
    }

    public Task<ulong> SerialisedSizeAsync(ref MDataInfo mDataInfo) {
      return _appBindings.MDataSerialisedSizeAsync(_appPtr, ref mDataInfo);
    }

    public async Task<(List<byte>, ulong)> GetValueAsync(MDataInfo mDataInfo, List<byte> key) {
      var (dataArray, entryVersion) = await _appBindings.MDataGetValueAsync(_appPtr, ref mDataInfo, key.ToArray());
      return (new List<byte>(dataArray), entryVersion);
    }

    public async Task<NativeHandle> ListEntriesAsync(MDataInfo mDataInfo) {
      var mDataEntriesHandle = await _appBindings.MDataListEntriesAsync(_appPtr, ref mDataInfo);
      return new NativeHandle(mDataEntriesHandle, entriesH => _appBindings.MDataEntriesFreeAsync(_appPtr, entriesH));
    }

    public Task<List<MDataKey>> ListKeysAsync(ref MDataInfo mDataInfo) {
      // TODO: Needs fixed
      throw new NotImplementedException();

      /*var mDataEntKeysH = await _appBindings.MDataListKeysAsync(_appPtr, ref mDataInfo);
      return new NativeHandle(mDataEntKeysH, MDataKeys.FreeAsync);*/
    }

    public Task<List<(MDataValue, ulong)>> ListValuesAsync(ref MDataInfo mDataInfo) {
      // TODO: Needs fixed
      throw new NotImplementedException();
//      return _appBindings.MDataListValuesAsync(_appPtr, ref mDataInfo);
    }

    public Task MutateEntriesAsync(ref MDataInfo mDataInfo, NativeHandle entryActionsH) {
      return _appBindings.MDataMutateEntriesAsync(_appPtr, ref mDataInfo, entryActionsH);
    }

    public Task<ulong> ListPermissionsAsync(ref MDataInfo mDataInfo) {
      // TODO needs fix
      throw new NotImplementedException();
      // return _appBindings.MDataListPermissionsAsync(_appPtr, ref mDataInfo);
    }

    public Task<PermissionSet> ListUserPermissionsAsync(ref MDataInfo mDataInfo, NativeHandle userSignPubKey)
    {
       return _appBindings.MDataListUserPermissionsAsync(_appPtr, ref mDataInfo, userSignPubKey);
    }

    public Task SetUserPermissionsAsync(ref MDataInfo mDataInfo, NativeHandle userSignPubKey, PermissionSet permissionSet, ulong version)
    {
      return _appBindings.MDataSetUserPermissionsAsync(_appPtr, ref mDataInfo, userSignPubKey, ref permissionSet, version);
    }

    public Task DelUserPermissionsAsync(ref MDataInfo mDataInfo, NativeHandle userSignPubKey, ulong version)
    {
      return _appBindings.MDataDelUserPermissionsAsync(_appPtr, ref mDataInfo, userSignPubKey, version);
    }

    public Task PutAsync(ref MDataInfo mDataInfo, NativeHandle permissionsH, NativeHandle entriesH) {
      return _appBindings.MDataPutAsync(_appPtr, ref mDataInfo, permissionsH, entriesH);
    }

    public Task<List<byte>> EncodeMetadata(ref MetadataResponse metadataResponse) {
      // TODO needs fix
      throw new NotImplementedException();
      // return _appBindings.MDataEncodeMetadataAsync(ref metadataResponse);
    }
  }
}
