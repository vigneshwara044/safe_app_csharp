using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp.Misc {
  [PublicAPI]
  public static class NativeUtils {
    private static readonly IAppBindings AppBindings = AppResolver.Current;

    public static Task<byte[]> GenerateNonceAsync() {
      return AppBindings.GenerateNonceAsync();
    }

    public static Task<List<byte>> Sha3HashAsync(List<byte> source) {
      return AppBindings.Sha3HashAsync(source);
    }
  }
}
