using System.Threading.Tasks;
using JetBrains.Annotations;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp.Misc {
  [PublicAPI]
  public static class CipherOpt {
    private static readonly IAppBindings AppBindings = AppResolver.Current;

    public static Task FreeAsync(ulong cipherOptHandle) {
      return AppBindings.CipherOptFreeAsync(Session.AppPtr, cipherOptHandle);
    }

    public static async Task<NativeHandle> NewAsymmetricAsync(NativeHandle encPubKeyH) {
      var cipherOptH = await AppBindings.CipherOptNewAsymmetricAsync(Session.AppPtr, encPubKeyH);
      return new NativeHandle(cipherOptH, FreeAsync);
    }

    public static async Task<NativeHandle> NewPlaintextAsync() {
      var cipherOptH = await AppBindings.CipherOptNewPlaintextAsync(Session.AppPtr);
      return new NativeHandle(cipherOptH, FreeAsync);
    }

    public static async Task<NativeHandle> NewSymmetricAsync() {
      var cipherOptH = await AppBindings.CipherOptNewSymmetricAsync(Session.AppPtr);
      return new NativeHandle(cipherOptH, FreeAsync);
    }
  }
}
