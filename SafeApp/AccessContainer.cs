using System.Threading.Tasks;
using JetBrains.Annotations;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp {
  [PublicAPI]
  public static class AccessContainer {
    private static readonly IAppBindings AppBindings = AppResolver.Current;

    public static Task<MDataInfo> GetMDataInfoAsync(string containerId) {
      return AppBindings.AccessContainerGetContainerMDataInfoAsync(Session.AppPtr, containerId);
    }
  }
}
