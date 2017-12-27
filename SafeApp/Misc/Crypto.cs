using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp.Misc {
  [PublicAPI]
  public static class Crypto {
    private static readonly IAppBindings AppBindings = AppResolver.Current;

    public static async Task<NativeHandle> AppPubSignKeyAsync() {
      var appPubSignKeyH = await AppBindings.AppPubSignKeyAsync(Session.AppPtr);
      return new NativeHandle(appPubSignKeyH, SignPubKeyFreeAsync);
    }

    public static async Task<List<byte>> DecryptSealedBoxAsync(List<byte> cipherText, NativeHandle pkHandle, NativeHandle skHandle) {
      var cipherTextPtr = cipherText.ToIntPtr();
      var byteArray = await AppBindings.DecryptSealedBoxAsync(Session.AppPtr, cipherTextPtr, (IntPtr)cipherText.Count, pkHandle, skHandle);
      Marshal.FreeHGlobal(cipherTextPtr);
      return new List<byte>(byteArray);
    }

    public static async Task<(NativeHandle, NativeHandle)> EncGenerateKeyPairAsync() {
      var (encPubKeyH, encSecKeyH) = await AppBindings.EncGenerateKeyPairAsync(Session.AppPtr);
      return (new NativeHandle(encPubKeyH, EncPubKeyFreeAsync), new NativeHandle(encSecKeyH, EncSecretKeyFreeAsync));
    }

    public static Task EncPubKeyFreeAsync(ulong encPubKeyH) {
      return AppBindings.EncPubKeyFreeAsync(Session.AppPtr, encPubKeyH);
    }

    public static Task<List<byte>> EncPubKeyGetAsync(NativeHandle encPubKeyH) {
      // TODO: needs fixed
      throw new NotImplementedException();
      //var byteArray = await AppBindings.EncPubKeyGetAsync(Session.AppPtr, encPubKeyH);
      //return new List<byte>(byteArray);
    }

    public static async Task<NativeHandle> EncPubKeyNewAsync(List<byte> asymPublicKeyBytes) {
      var asymPublicKeyPtr = asymPublicKeyBytes.ToIntPtr();
      var encryptPubKeyH = await AppBindings.EncPubKeyNewAsync(Session.AppPtr, asymPublicKeyPtr);
      Marshal.FreeHGlobal(asymPublicKeyPtr);
      return new NativeHandle(encryptPubKeyH, EncPubKeyFreeAsync);
    }

    public static async Task<List<byte>> EncryptSealedBoxAsync(List<byte> inputData, NativeHandle pkHandle) {
      var inputDataPtr = inputData.ToIntPtr();
      var byteArray = await AppBindings.EncryptSealedBoxAsync(Session.AppPtr, inputDataPtr, (IntPtr)inputData.Count, pkHandle);
      Marshal.FreeHGlobal(inputDataPtr);
      return new List<byte>(byteArray);
    }

    public static Task EncSecretKeyFreeAsync(ulong encSecKeyH) {
      return AppBindings.EncSecretKeyFreeAsync(Session.AppPtr, encSecKeyH);
    }

    public static Task<List<byte>> EncSecretKeyGetAsync(NativeHandle encSecKeyH) {
      // TODO: needs fixed
      throw new NotImplementedException();

      //var byteArray = await AppBindings.EncSecretKeyGetAsync(Session.AppPtr, encSecKeyH);
      //return new List<byte>(byteArray);
    }

    public static async Task<NativeHandle> EncSecretKeyNewAsync(List<byte> asymSecKeyBytes) {
      var asymSecKeyPtr = asymSecKeyBytes.ToIntPtr();
      var encSecKeyH = await AppBindings.EncSecretKeyNewAsync(Session.AppPtr, asymSecKeyPtr);
      Marshal.FreeHGlobal(asymSecKeyPtr);
      return new NativeHandle(encSecKeyH, EncSecretKeyFreeAsync);
    }

    public static Task SignPubKeyFreeAsync(ulong signKeyHandle) {
      return AppBindings.SignPubKeyFreeAsync(Session.AppPtr, signKeyHandle);
    }
  }
}
