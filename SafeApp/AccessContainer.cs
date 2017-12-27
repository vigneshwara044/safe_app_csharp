using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp {
  [PublicAPI]
  public class AccessContainer {
    private readonly IAppBindings _appBindings = AppResolver.Current;
    private IntPtr _appPtr;

    public AccessContainer(IntPtr appPtr) {
      _appPtr = appPtr;
    }

    public Task<MDataInfo> GetMDataInfoAsync(string containerId) {
      return _appBindings.AccessContainerGetContainerMDataInfoAsync(_appPtr, containerId);
    }
  }
}
