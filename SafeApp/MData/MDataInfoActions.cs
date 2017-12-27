using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp.MData {
  [PublicAPI]
  public static class MDataInfoActions {
    private static readonly IAppBindings AppBindings = AppResolver.Current;

    public static async Task<List<byte>> DecryptAsync(MDataInfo mDataInfo, List<byte> cipherText) {
      var byteArray = await AppBindings.MDataInfoDecryptAsync(ref mDataInfo, cipherText.ToArray());
      return new List<byte>(byteArray);
    }

    public static Task<MDataInfo> DeserialiseAsync(List<byte> serialisedData) {
      // TODO: Needs fixed
      throw new NotImplementedException();

      //return AppBindings.MDataInfoDeserialiseAsync(Session.AppPtr, serialisedData);
    }

    public static async Task<List<byte>> EncryptEntryKeyAsync(MDataInfo mDataInfo, List<byte> inputBytes) {
      var byteArray = await AppBindings.MDataInfoEncryptEntryKeyAsync(ref mDataInfo, inputBytes.ToArray());
      return new List<byte>(byteArray);
    }

    public static async Task<List<byte>> EncryptEntryValueAsync(MDataInfo mDataInfo, List<byte> inputBytes) {
      var byteArray = await AppBindings.MDataInfoEncryptEntryValueAsync(ref mDataInfo, inputBytes.ToArray());
      return new List<byte>(byteArray);
    }

    public static Task<MDataInfo> RandomPrivateAsync(ulong typeTag) {
      return AppBindings.MDataInfoRandomPrivateAsync(typeTag);
    }

    public static Task<MDataInfo> RandomPublicAsync(ulong typeTag) {
      return AppBindings.MDataInfoRandomPublicAsync(typeTag);
    }

    public static async Task<List<byte>> SerialiseAsync(MDataInfo mDataInfo) {
      var byteArray = await AppBindings.MDataInfoSerialiseAsync(ref mDataInfo);
      return new List<byte>(byteArray);
    }
  }
}
