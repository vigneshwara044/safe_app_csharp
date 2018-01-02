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

    public async Task<(NativeHandle, NativeHandle)> SignGenerateKeyPairAsync() {
      var (publicKeyHandle, secretKeyHandle) = await _appBindings.SignGenerateKeyPairAsync(_appPtr);
      return (new NativeHandle(publicKeyHandle, SignPubKeyFreeAsync), new NativeHandle(secretKeyHandle, SignSecKeyFreeAsync));
    }

    public Task<NativeHandle> SignPubKeyNewAsync(List<byte> rawPubSignKey) {
      // TODO fix in bindings to accept List<byte>
      throw new NotImplementedException();
//      var handle = await _appBindings.SignPubKeyNewAsync(_appPtr, rawPubSignKey);
//      return new NativeHandle(handle, SignPubKeyFreeAsync);
    }

    public Task<List<byte[]>> SignPubKeyGetAsync(NativeHandle pubSignKey) {
      // TODO fix in bindings to return List<byte>
      throw new NotImplementedException();
      // return _appBindings.SignPubKeyGetAsync(_appPtr, pubSignKey);
    }

    public Task<NativeHandle> SignSecKeyNewAsync(List<byte> rawSecSignKey)
    {
      // TODO fix in bindings to accept List<byte>
      throw new NotImplementedException();
      //      var handle = await _appBindings.SignSecKeyNewAsync(_appPtr, rawSecSignKey);
      //      return new NativeHandle(handle, SignSecKeyFreeAsync);
    }

    public Task<List<byte[]>> SignSecKeyGetAsync(NativeHandle secSignKey)
    {
      // TODO fix in bindings to return List<byte>
      throw new NotImplementedException();
      // return _appBindings.SignSecKeyGetAsync(_appPtr, pubSignKey);
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

    public Task SignPubKeyFreeAsync(ulong pubSignKeyHandle) {
      return _appBindings.SignPubKeyFreeAsync(_appPtr, pubSignKeyHandle);
    }

    public Task SignSecKeyFreeAsync(ulong secSignKeyHandle) {
      return _appBindings.SignSecKeyFreeAsync(_appPtr, secSignKeyHandle);
    }

    public Task<List<byte>> SignAsync(List<byte> data, NativeHandle signSecKey) {
      //TODO needs fix
      throw new NotImplementedException();
//      return _appBindings.SignAsync(_appPtr, data, signSecKey);
    }

    public Task<List<byte>> VerifyAsync(List<byte> signedData, NativeHandle signPubKey)
    {
      //TODO needs fix
      throw new NotImplementedException();
      //      return _appBindings.VerifyAsync(_appPtr, signedData, signPubKey);
    }

    public Task<List<byte>> EncryptAsync(List<byte> data, NativeHandle encPubKey, NativeHandle encSecKey)
    {
      //TODO needs fix
      throw new NotImplementedException();
//            return _appBindings.EncryptAsync(_appPtr, data, encPubKey, encSecKey);
    }

    public Task<List<byte>> DecryptAsync(List<byte> cipherText, NativeHandle encPubKey, NativeHandle encSecKey)
    {
      //TODO needs fix
      throw new NotImplementedException();
      //            return _appBindings.DecryptAsync(_appPtr, data, encPubKey, encSecKey);
    }

  }
}
