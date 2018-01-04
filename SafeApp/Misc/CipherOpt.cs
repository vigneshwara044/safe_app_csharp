using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp.Misc {
  [PublicAPI]
  public class CipherOpt {
    private readonly IAppBindings _appBindings = AppResolver.Current;
    private IntPtr _appPtr;

    public CipherOpt(IntPtr appPtr)
    {
      _appPtr = appPtr;
    }

    public Task FreeAsync(ulong cipherOptHandle) {
      return _appBindings.CipherOptFreeAsync(_appPtr, cipherOptHandle);
    }
    /// <summary>
    /// Create a new Asymmetric CipherOpt handle
    /// </summary>
    /// <param name="encPubKeyH">NativeHandle</param>
    /// <returns>AsymmetricCipherOpt NativeHandle</returns>
    public async Task<NativeHandle> NewAsymmetricAsync(NativeHandle encPubKeyH) {
      var cipherOptH = await _appBindings.CipherOptNewAsymmetricAsync(_appPtr, encPubKeyH);
      return new NativeHandle(cipherOptH, FreeAsync);
    }
    /// <summary>
    /// Create a new Plain text CipherOpt handle
    /// </summary>
    /// <returns>Plain text NativeHandle</returns>
    public async Task<NativeHandle> NewPlaintextAsync() {
      var cipherOptH = await _appBindings.CipherOptNewPlaintextAsync(_appPtr);
      return new NativeHandle(cipherOptH, FreeAsync);
    }
    /// <summary>
    /// Create a new Symmetric CipherOpt handle
    /// </summary>
    /// <returns>Symmetric NativeHandle</returns>
    public async Task<NativeHandle> NewSymmetricAsync() {
      var cipherOptH = await _appBindings.CipherOptNewSymmetricAsync(_appPtr);
      return new NativeHandle(cipherOptH, FreeAsync);
    }
  }
}
