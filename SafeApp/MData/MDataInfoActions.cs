using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp.MData {
  [PublicAPI]
  public class MDataInfoActions {
    private readonly IAppBindings _appBindings = AppResolver.Current;
    private IntPtr _appPtr;

    public MDataInfoActions(IntPtr appPtr) {
      _appPtr = appPtr;
    }

    public Task<MDataInfo> NewPrivateAsync(List<byte> xorName, long typeTag, List<byte> secEncKey, List<byte> nonce) {
      // TODO needs fix
      throw new NotImplementedException();
      // return _appBindings.MDataInfoNewPrivateAsync(xorName, typeTag, secEncKey, nonce);
    }

    public async Task<List<byte>> DecryptAsync(MDataInfo mDataInfo, List<byte> cipherText) {
      var byteArray = await _appBindings.MDataInfoDecryptAsync(ref mDataInfo, cipherText.ToArray());
      return new List<byte>(byteArray);
    }

    public async Task<List<byte>> EncryptEntryKeyAsync(MDataInfo mDataInfo, List<byte> inputBytes) {
      var byteArray = await _appBindings.MDataInfoEncryptEntryKeyAsync(ref mDataInfo, inputBytes.ToArray());
      return new List<byte>(byteArray);
    }

    public async Task<List<byte>> EncryptEntryValueAsync(MDataInfo mDataInfo, List<byte> inputBytes) {
      var byteArray = await _appBindings.MDataInfoEncryptEntryValueAsync(ref mDataInfo, inputBytes.ToArray());
      return new List<byte>(byteArray);
    }

    public Task<MDataInfo> RandomPrivateAsync(ulong typeTag) {
      return _appBindings.MDataInfoRandomPrivateAsync(typeTag);
    }

    public Task<MDataInfo> RandomPublicAsync(ulong typeTag) {
      return _appBindings.MDataInfoRandomPublicAsync(typeTag);
    }

    public async Task<List<byte>> SerialiseAsync(MDataInfo mDataInfo) {
      var byteArray = await _appBindings.MDataInfoSerialiseAsync(ref mDataInfo);
      return new List<byte>(byteArray);
    }


    public Task<MDataInfo> DeserialiseAsync(List<byte> serialisedData)
    {
      // TODO: Needs fixed
      throw new NotImplementedException();

      //return AppBindings.MDataInfoDeserialiseAsync(_appPtr, serialisedData);
    }
  }
}
