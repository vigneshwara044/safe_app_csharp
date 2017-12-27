using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

// ReSharper disable InconsistentNaming

namespace SafeApp.IData {
  [PublicAPI]
  public static class IData {
    private static readonly IAppBindings AppBindings = AppResolver.Current;

    public static async Task<List<byte>> CloseSelfEncryptorAsync(ulong seH, NativeHandle cipherOptH) {
      var xorNamePtr = await AppBindings.IDataCloseSelfEncryptorAsync(Session.AppPtr, seH, cipherOptH);
      return xorNamePtr.ToList<byte>((IntPtr)AppConstants.XorNameLen);
    }

    public static async Task<NativeHandle> FetchSelfEncryptorAsync(List<byte> xorName) {
      var xorNamePtr = xorName.ToIntPtr();
      var sEReaderHandle = await AppBindings.IDataFetchSelfEncryptorAsync(Session.AppPtr, xorNamePtr);
      Marshal.FreeHGlobal(xorNamePtr);
      return new NativeHandle(sEReaderHandle, SelfEncryptorReaderFreeAsync);
    }

    public static async Task<NativeHandle> NewSelfEncryptorAsync() {
      var sEWriterHandle = await AppBindings.IDataNewSelfEncryptorAsync(Session.AppPtr);
      return new NativeHandle(sEWriterHandle, null);
    }

    public static async Task<List<byte>> ReadFromSelfEncryptorAsync(NativeHandle seHandle, ulong fromPos, ulong len) {
      var dataArray = await AppBindings.IDataReadFromSelfEncryptorAsync(Session.AppPtr, seHandle, fromPos, len);
      return new List<byte>(dataArray);
    }

    public static Task SelfEncryptorReaderFreeAsync(ulong sEReaderHandle) {
      return AppBindings.IDataSelfEncryptorReaderFreeAsync(Session.AppPtr, sEReaderHandle);
    }

    public static Task SelfEncryptorWriterFreeAsync(ulong sEWriterHandle) {
      return AppBindings.IDataSelfEncryptorWriterFreeAsync(Session.AppPtr, sEWriterHandle);
    }

    public static Task<ulong> SizeAsync(NativeHandle seHandle) {
      return AppBindings.IDataSizeAsync(Session.AppPtr, seHandle);
    }

    public static Task WriteToSelfEncryptorAsync(NativeHandle seHandle, List<byte> data) {
      return AppBindings.IDataWriteToSelfEncryptorAsync(Session.AppPtr, seHandle, data.ToArray());
    }
  }
}
