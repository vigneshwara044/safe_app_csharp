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
  public static class NativeUtils {
    private static readonly IAppBindings AppBindings = AppResolver.Current;

    public static async Task<List<byte>> Sha3HashAsync(List<byte> source) {
      var sourcePtr = source.ToIntPtr();
      var byteArray = await AppBindings.Sha3HashAsync(sourcePtr, (IntPtr)source.Count);
      Marshal.FreeHGlobal(sourcePtr);
      return new List<byte>(byteArray);
    }

    public static Task<List<byte>> GenerateNonceAsync()
    {
      // TODO needs fix
      throw new NotImplementedException();
//      return AppBindings.GenerateNonceAsync();
    }
  }
}
