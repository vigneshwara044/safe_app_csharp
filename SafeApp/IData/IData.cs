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
  public class IData {
    private readonly IAppBindings _appBindings = AppResolver.Current;
    private IntPtr _appPtr;

    public IData(IntPtr appPtr)
    {
      _appPtr = appPtr;
    }

    public async Task<List<byte>> CloseSelfEncryptorAsync(ulong seH, NativeHandle cipherOptH) {
      var xorNamePtr = await _appBindings.IDataCloseSelfEncryptorAsync(_appPtr, seH, cipherOptH);
      return xorNamePtr.ToList<byte>((IntPtr)AppConstants.XorNameLen);
    }

    public async Task<NativeHandle> FetchSelfEncryptorAsync(List<byte> xorName) {
      var xorNamePtr = xorName.ToIntPtr();
      var sEReaderHandle = await _appBindings.IDataFetchSelfEncryptorAsync(_appPtr, xorNamePtr);
      Marshal.FreeHGlobal(xorNamePtr);
      return new NativeHandle(sEReaderHandle, SelfEncryptorReaderFreeAsync);
    }

    public async Task<NativeHandle> NewSelfEncryptorAsync() {
      var sEWriterHandle = await _appBindings.IDataNewSelfEncryptorAsync(_appPtr);
      return new NativeHandle(sEWriterHandle, null);
    }

    public async Task<List<byte>> ReadFromSelfEncryptorAsync(NativeHandle seHandle, ulong fromPos, ulong len) {
      var dataArray = await _appBindings.IDataReadFromSelfEncryptorAsync(_appPtr, seHandle, fromPos, len);
      return new List<byte>(dataArray);
    }

    public Task SelfEncryptorReaderFreeAsync(ulong sEReaderHandle) {
      return _appBindings.IDataSelfEncryptorReaderFreeAsync(_appPtr, sEReaderHandle);
    }

    public Task SelfEncryptorWriterFreeAsync(ulong sEWriterHandle) {
      return _appBindings.IDataSelfEncryptorWriterFreeAsync(_appPtr, sEWriterHandle);
    }

    public Task<ulong> SizeAsync(NativeHandle seHandle) {
      return _appBindings.IDataSizeAsync(_appPtr, seHandle);
    }

    public Task WriteToSelfEncryptorAsync(NativeHandle seHandle, List<byte> data) {
      return _appBindings.IDataWriteToSelfEncryptorAsync(_appPtr, seHandle, data.ToArray());
    }
  }
}
