using System;
using System.Collections.Generic;

namespace SafeApp.Utilities
{
  internal struct RegisteredApp
  {
    public AppExchangeInfo AppInfo;
    public List<ContainerPermissions> Containers;

    internal RegisteredApp(RegisteredAppNative native)
    {
      AppInfo = native.AppInfo;
      Containers = BindingUtils.CopyToObjectList<ContainerPermissions>(native.ContainersPtr, (int)native.ContainersLen);
    }

    internal RegisteredAppNative ToNative()
    {
      return new RegisteredAppNative()
      {
        AppInfo = AppInfo,
        ContainersPtr = BindingUtils.CopyFromObjectList(Containers),
        ContainersLen = (ulong)(Containers?.Count ?? 0),
        ContainersCap = 0
      };
    }
  }

  internal struct RegisteredAppNative
  {
    public AppExchangeInfo AppInfo;
    public IntPtr ContainersPtr;
    public ulong ContainersLen;
    public ulong ContainersCap;

    internal void Free()
    {
      BindingUtils.FreeList(ref ContainersPtr, ref ContainersLen);
    }
  }

}
