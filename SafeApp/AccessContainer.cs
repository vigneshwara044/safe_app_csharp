using System;
using System.Collections.Generic;
using System.Linq;
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

    public Task RefreshAccessInfoAsync() {
      return _appBindings.AccessContainerRefreshAccessInfoAsync(_appPtr);
    }

    public async Task<List<ContainerPermissions>> AccessContainerFetchAsync() {
      var array = await _appBindings.AccessContainerFetchAsync(_appPtr);
      return array.ToList();
    }

  }
}
