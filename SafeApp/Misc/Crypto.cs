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
  public class Crypto {
    private readonly IAppBindings _appBindings = AppResolver.Current;
    private IntPtr _appPtr;

    public Crypto(IntPtr appPtr)
    {
      _appPtr = appPtr;
    }

    public async Task<NativeHandle> AppPubSignKeyAsync() {
      var appPubSignKeyH = await _appBindings.AppPubSignKeyAsync(_appPtr);
      return new NativeHandle(appPubSignKeyH, SignPubKeyFreeAsync);
    }

    public async Task<List<byte>> DecryptSealedBoxAsync(List<byte> cipherText, NativeHandle pkHandle, NativeHandle skHandle) {
      var cipherTextPtr = cipherText.ToIntPtr();
      var byteArray = await _appBindings.DecryptSealedBoxAsync(_appPtr, cipherTextPtr, (IntPtr)cipherText.Count, pkHandle, skHandle);
      Marshal.FreeHGlobal(cipherTextPtr);
      return new List<byte>(byteArray);
    }

    public async Task<(NativeHandle, NativeHandle)> EncGenerateKeyPairAsync() {
      var (encPubKeyH, encSecKeyH) = await _appBindings.EncGenerateKeyPairAsync(_appPtr);
      return (new NativeHandle(encPubKeyH, EncPubKeyFreeAsync), new NativeHandle(encSecKeyH, EncSecretKeyFreeAsync));
    }

    public Task EncPubKeyFreeAsync(ulong encPubKeyH) {
      return _appBindings.EncPubKeyFreeAsync(_appPtr, encPubKeyH);
    }

    public Task<List<byte>> EncPubKeyGetAsync(NativeHandle encPubKeyH) {
      // TODO: needs fixed
      throw new NotImplementedException();
      //var byteArray = await _appBindings.EncPubKeyGetAsync(_appPtr, encPubKeyH);
      //return new List<byte>(byteArray);
    }

    public async Task<NativeHandle> EncPubKeyNewAsync(List<byte> asymPublicKeyBytes) {
      var asymPublicKeyPtr = asymPublicKeyBytes.ToIntPtr();
      var encryptPubKeyH = await _appBindings.EncPubKeyNewAsync(_appPtr, asymPublicKeyPtr);
      Marshal.FreeHGlobal(asymPublicKeyPtr);
      return new NativeHandle(encryptPubKeyH, EncPubKeyFreeAsync);
    }

    public async Task<List<byte>> EncryptSealedBoxAsync(List<byte> inputData, NativeHandle pkHandle) {
      var inputDataPtr = inputData.ToIntPtr();
      var byteArray = await _appBindings.EncryptSealedBoxAsync(_appPtr, inputDataPtr, (IntPtr)inputData.Count, pkHandle);
      Marshal.FreeHGlobal(inputDataPtr);
      return new List<byte>(byteArray);
    }

    public Task EncSecretKeyFreeAsync(ulong encSecKeyH) {
      return _appBindings.EncSecretKeyFreeAsync(_appPtr, encSecKeyH);
    }

    public Task<List<byte>> EncSecretKeyGetAsync(NativeHandle encSecKeyH) {
      // TODO: needs fixed
      throw new NotImplementedException();

      //var byteArray = await _appBindings.EncSecretKeyGetAsync(_appPtr, encSecKeyH);
      //return new List<byte>(byteArray);
    }

    public async Task<NativeHandle> EncSecretKeyNewAsync(List<byte> asymSecKeyBytes) {
      var asymSecKeyPtr = asymSecKeyBytes.ToIntPtr();
      var encSecKeyH = await _appBindings.EncSecretKeyNewAsync(_appPtr, asymSecKeyPtr);
      Marshal.FreeHGlobal(asymSecKeyPtr);
      return new NativeHandle(encSecKeyH, EncSecretKeyFreeAsync);
    }

    public Task SignPubKeyFreeAsync(ulong signKeyHandle) {
      return _appBindings.SignPubKeyFreeAsync(_appPtr, signKeyHandle);
    }
  }
}
